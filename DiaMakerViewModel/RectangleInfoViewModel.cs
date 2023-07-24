using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiaMakerModel.Tracing;
using System.Windows.Media;

namespace DiaMakerViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class RectangleInfoViewModel
    //----------------------------------------------------------------------------------------------------------------------
    public class RectangleInfoViewModel : DiaElementViewModel
    {
        private SectionVertex vertex;
//        private int strokeThickness;
        private bool isSelected;
        private static SolidColorBrush commonColor;
        private static SolidColorBrush selectedColor;
        public static double MultiplicationElementHeightDiaPanel = 1;
        public static double MultiplicationElementWidthDiaPanel = 1;
        //----------------------------------------------------------------------------------------------------------------------
        public RectangleInfoViewModel(SectionVertex pVertex)
        {
            vertex = pVertex;
        }

        //--------------------------------------------------------------------------------------
        static RectangleInfoViewModel()
        {
            commonColor = new SolidColorBrush();
            commonColor.Color = Colors.Silver;
            selectedColor = new SolidColorBrush();
            selectedColor.Color = Colors.White;
        }

        //--------------------------------------------------------------------------------------
        public int StrokeThickness
        {
            get { return IsSelected ? 2 : 0; }
            set { }
        }

        //--------------------------------------------------------------------------------------
        public SolidColorBrush RectangleFill
        {
            get
            {
                var c = IsSelected ? selectedColor : commonColor;
                return c;
            }
            set { }
        }

        //--------------------------------------------------------------------------------------
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
                OnPropertyChanged("StrokeThickness");
                OnPropertyChanged("RectangleFill");
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public string Name
        {
            get { return vertex.Name; }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public double Left
        {
            get { return vertex.Left*MultiplicationElementWidthDiaPanel; }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public double Top
        {
            get { return vertex.Top*MultiplicationElementHeightDiaPanel; }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public double Height
        {
            get { return vertex.Height*MultiplicationElementHeightDiaPanel; }
        }

        //----------------------------------------------------------------------------------------------------------------------
        public double Width
        {
            get { return vertex.Width*MultiplicationElementWidthDiaPanel; }
        }

        //--------------------------------------------------------------------------------------
        public override string ToString()
        {
            return vertex.ToString();
        }

        //----------------------------------------------------------------------------------------------------------------------
        public override void NotifyUI()
        {
            OnPropertyChanged("Height");
            OnPropertyChanged("Width");
            OnPropertyChanged("Left");
            OnPropertyChanged("Top");
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
