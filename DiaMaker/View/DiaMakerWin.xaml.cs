using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DiaMakerViewModel;
using System.ServiceProcess;
using CommonLib.Interfaces;

namespace DiaMaker.View
{
    /// <summary>
    /// Interaction logic for DiaMakerWin.xaml
    /// </summary>
    //-------------------------------------------------------------------------------------------------------------------
    // class DiaMakerWin
    //-------------------------------------------------------------------------------------------------------------------
    public partial class DiaMakerWin : Window
    {
        private bool isLoaded = false;
        //-------------------------------------------------------------------------------------------------------------------
        public DiaMakerWin()
        {
            string dbServiceName = "MSSQL$LRA";
            if (Environment.MachineName.ToUpper() == "LAPTOP-UI2QOUED")
                dbServiceName = "MSSQL$SQLEXPRESS";
            if (!DiaMakerWin.StartService(dbServiceName, 60000))
            {
                Close();
                return;
            }
            InitializeComponent();
            svDia.SizeChanged += Panel_SizeChanged;

        }
        //-------------------------------------------------------------------------------------------------------------------
        public static DependencyProperty SetSelectedCommandProperty = DependencyProperty.RegisterAttached(
                                                                        "SetSelectedCommand",
                                                                        typeof(ICommand),
                                                                        typeof(DiaMakerWin));
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand SetSelectedCommand
        {
            get { return (ICommand)GetValue(SetSelectedCommandProperty); }
            set { SetValue(SetSelectedCommandProperty, value); }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            var viewModel = new DiaViewModel();

            DataContext = viewModel;
            //           btnGetDBList.Command = viewModel.GetDataBasesCommand;

            Binding widthBinding = new Binding("ActualPanelWidth");
            widthBinding.Source = DataContext;
            widthBinding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(this, DiaMakerWin.PanelWidthProperty, widthBinding);

            Binding heightBinding = new Binding("ActualPanelHeight");
            heightBinding.Source = DataContext;
            heightBinding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(this, DiaMakerWin.PanelHeightProperty, heightBinding);

            Binding origineWidthBinding = new Binding("OriginePanelWidth");
            origineWidthBinding.Source = DataContext;
            origineWidthBinding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(this, DiaMakerWin.OriginePanelWidthProperty, origineWidthBinding);

            Binding origineHeightBinding = new Binding("OriginePanelHeight");
            origineHeightBinding.Source = DataContext;
            origineHeightBinding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(this, DiaMakerWin.OriginePanelHeightProperty, origineHeightBinding);

            OriginePanelHeight = svDia.ViewportHeight;
            OriginePanelWidth = svDia.ViewportWidth;

            PanelHeight = icDia.ActualHeight;
            PanelWidth = icDia.ActualWidth;

            Binding setSelectedBinding = new Binding("SetSelectedCommand");
            setSelectedBinding.Source = DataContext;
            BindingOperations.SetBinding(this, DiaMakerWin.SetSelectedCommandProperty, setSelectedBinding);

            isLoaded = true;
            IScrollIntoViewAction scrollIntoViewAction = (IScrollIntoViewAction)DataContext;
            scrollIntoViewAction.MainGridScrollIntoView += o =>
            {
                ScrollIntoView(o);
            };
            scrollIntoViewAction.DataGridCommitEdit += () =>
            {
                DataGrid_CommitEdit();
            };

            LayoutUpdated += DiaMakerWin_LayoutUpdated;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void DataGrid_CommitEdit()
        {
            //            System.Windows.Forms.SendKeys.SendWait("{ESC}");
            var key = Key.Escape;                    // Key to send
            var target = Keyboard.FocusedElement;    // Target element
            var routedEvent = Keyboard.KeyDownEvent; // Event to send
            Visual vtarget = target as Visual;

            if (target != null && vtarget != null)
            {
                target.RaiseEvent(
                    new KeyEventArgs(
                    Keyboard.PrimaryDevice,
                    PresentationSource.FromVisual(vtarget),
                    0,
                    key) { RoutedEvent = routedEvent }

                    );
            }
            dgTables.CancelEdit();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void ScrollIntoView(object pSelectedItem)
        {
            if (pSelectedItem != null) 
                dgTables.ScrollIntoView(dgTables.SelectedItem);
        }
        //-------------------------------------------------------------------------------------------------------------------
        void Panel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(OriginePanelHeight - svDia.ViewportHeight) > 0.001)
                OriginePanelHeight = svDia.ViewportHeight;
            if (Math.Abs(OriginePanelWidth - svDia.ViewportWidth) > 0.001)
                OriginePanelWidth = svDia.ViewportWidth;
            //((DiaViewModel)DataContext).PanelHeight = PanelHeight;
            //((DiaViewModel)DataContext).PanelWidth = PanelWidth;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public double PanelWidth
        {
            get { return (double)GetValue(PanelWidthProperty); }
            set { SetValue(PanelWidthProperty, value); }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty PanelWidthProperty =
          DependencyProperty.Register("PanelWidth", typeof(double), typeof(DiaMakerWin),
          new PropertyMetadata(0d));
        //-------------------------------------------------------------------------------------------------------------------
        public double PanelHeight
        {
            get { return (double)GetValue(PanelHeightProperty); }
            set { SetValue(PanelHeightProperty, value); }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty PanelHeightProperty =
          DependencyProperty.Register("PanelHeight", typeof(double), typeof(DiaMakerWin),
          new PropertyMetadata(0d));
        //-------------------------------------------------------------------------------------------------------------------
        public double OriginePanelWidth
        {
            get { return (double)GetValue(OriginePanelWidthProperty); }
            set { SetValue(OriginePanelWidthProperty, value); }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty OriginePanelWidthProperty =
          DependencyProperty.Register("OriginePanelWidth", typeof(double), typeof(DiaMakerWin),
          new PropertyMetadata(0d));
        //-------------------------------------------------------------------------------------------------------------------
        public double OriginePanelHeight
        {
            get { return (double)GetValue(OriginePanelHeightProperty); }
            set { SetValue(OriginePanelHeightProperty, value); }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty OriginePanelHeightProperty =
          DependencyProperty.Register("OriginePanelHeight", typeof(double), typeof(DiaMakerWin),
          new PropertyMetadata(0d));
        //-------------------------------------------------------------------------------------------------------------------
        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (SetSelectedCommand != null && sender != null && sender is Rectangle)
                SetSelectedCommand.Execute((sender as Rectangle).DataContext);
            else if (SetSelectedCommand != null && e.Source != null && e.Source is Rectangle)
                SetSelectedCommand.Execute((e.Source as Rectangle).DataContext);
            else if (SetSelectedCommand != null && e.Source != null && e.Source is TextBlock)
                SetSelectedCommand.Execute((e.Source as TextBlock).DataContext);
        }
        //-------------------------------------------------------------------------------------------------------------------
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (isLoaded)
            {
                if (e.Property == PanelWidthProperty)
                {

                    if (Math.Abs(icDia.ActualWidth - PanelWidth) > 0.001)
                        icDia.Width = PanelWidth;
                }
                if (e.Property == PanelHeightProperty)
                {

                    if (Math.Abs(icDia.ActualHeight - PanelHeight) > 0.001)
                        icDia.Height = PanelHeight;
                }
            }

        }
        //-------------------------------------------------------------------------------------------------------------------
        private void DiaMakerWin_LayoutUpdated(object sender, EventArgs e)
        {
            /*
            if (isLoaded)
            {

                    if (Math.Abs(icDia.ActualWidth - PanelWidth) > 0.001)
                        icDia.Width = PanelWidth;
                    if (Math.Abs(icDia.ActualHeight - PanelHeight) > 0.001)
                        icDia.Height = PanelHeight;
            }
             * */
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static bool StartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot start service " + serviceName + "  " + ex.ToString());
                    return false;
                }
            }
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------------
    // class VisibilityConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visibility = (bool)value;
            if (visibility)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------------
}
