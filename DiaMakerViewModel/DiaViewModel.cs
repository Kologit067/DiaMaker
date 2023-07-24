using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiaMakerModel;
using System.Collections.ObjectModel;
using DiaMakerModel.Tracing;
using Common.CommonLib.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using CommonLib;
using CommonLib.Tools;
using CommonLib.Interfaces;

namespace DiaMakerViewModel
{
    /*
     1  Scrolling
     2  Issue: equal count of itteration of ALL algorithm
     7  Option to tune maximum of execution time, appropriate change in UI and VM (ui control, property, command and etc) 
     6  united algorithm (better exact and approximate)
     8  Selection vertices (path length < 2 from current) 
     9  Test method with parameter defining set of subgraph for testing for example (1,5,7)
     10 Test for asinchronous algorithm
     11 Handling exception in new asinchronous algorithm
     14 add checking fo optimal route into general test method
     3  algorithm with previous sorting of vertices (starting from vertix with more weight)
     4  algorithm with previous sorting of place (starting with central place)
     12 Possibility to change size of working panel
     18 show on UI binary presentation of selected subset of tables set 
     15 store procedure for saving statistics to archive
     18 size of window have to be constant
     23 show number of selected tables
     24 extend textblock for binary presentation of selected set
     21 filter for selected table
     22 search of table
     16 usage parallel lynq
     13 calculation of minimal size of working panel
     20 incorrect layout for graph with count >= 10
     17 details of algorithm performance (option for saving)
     19 algoryth with forced elimination
     -------------------------------
     1 define better PresentationData value for optimal relation horisontal and vertical sizes      
     2 Fint size in rectangle
     3 change calculation of count of rows and columns in matrix 
     4 Error unnessasary intresection in tracing database AXISMAT from AdminTemplate (aproximately algorithm)
     5 Selecting better variant of algorithm
     6 Incorrect changes size after working of splitter
      
    */
    //----------------------------------------------------------------------------------------------------------------------
// class DiaViewModel
//----------------------------------------------------------------------------------------------------------------------
    public class DiaViewModel : ViewModelBase, IScrollIntoViewAction
    {
        private ObservableCollection<EmptyOrderEnum> emptyOrderList;
        private ObservableCollection<BaseEnumerationEnum> baseEnumerationList;
        private ObservableCollection<UsingLowestimationEnum> usingLowestimationList;
        private ObservableCollection<EliminationEnum> eliminationList;
        private ObservableCollection<IsSortedEnum> isSortedList;
        private ObservableCollection<SortedTypeEnum> sortedTypeList;

        private EmptyOrderEnum selectedEmptyOrder;
        private BaseEnumerationEnum selectedBaseEnumeration;
        private UsingLowestimationEnum selectedUsingLowestimation;
        private EliminationEnum selectedElimination;
        private IsSortedEnum selectedIsSorted;
        private SortedTypeEnum selectedSortedType;
        
        private ObservableCollection<DiaMakerModel.DataBase> dataBases;
        private ObservableCollection<DiaElementViewModel> diaElements;
        private ObservableCollection<Table> tables;
        private ObservableCollection<SectionVertex> vertex;
        private ObservableCollection<ConnectingLine> connectingLines;
        private ObservableCollection<AlgorithmInfo> verteciesInTableAlgorithms;

        private AlgorithmInfo selectedVerteciesInTableAlgorithm;
        private Table selectedTable;
        private TableKeys tableKeys;
        private string connectionString;
        private string searchTablePattern;
        private string manualName;

        private double panelHeight;
        private double panelWidth;
        private double originePanelHeight;
        private double originePanelWidth;
        private double startOriginePanelHeight;
        private double startOriginePanelWidth;
        private double multiplicationHeightDiaPanel = 1;
        private double multiplicationWidthDiaPanel = 1;
        private double multiplicationElementHeightDiaPanel = 1;
        private double multiplicationElementWidthDiaPanel = 1;
        private double minimalMultiplicationHeightDiaPanel;
        private double minimalMultiplicationWidthDiaPanel;
        
        private int changeOptimalNumber;
        private int weightOfCurrentRoute;
        private int positionInBestRoutes;
        private int maxDurability = 120;
        private int binSubsetPresentation;
        private int countOfSelected;
        private IGraph<IVertex> fullGraph;
        private List<IGraph<IVertex>> subGraphList;
        private Tracing tracing;
        private RectangleInfoViewModel selectedRectangle;
        private bool isColored = true;
        public event Action<Object> MainGridScrollIntoView;
        public event Action DataGridCommitEdit;
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<EmptyOrderEnum> EmptyOrderList
        {
            get
            {
                return emptyOrderList;
            }
            set
            {
                emptyOrderList = value;
                OnPropertyChanged("EmptyOrderList");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<BaseEnumerationEnum> BaseEnumerationList
        {
            get
            {
                return baseEnumerationList;
            }
            set
            {
                baseEnumerationList = value;
                OnPropertyChanged("BaseEnumerationList");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<UsingLowestimationEnum> UsingLowestimationList
        {
            get
            {
                return usingLowestimationList;
            }
            set
            {
                usingLowestimationList = value;
                OnPropertyChanged("UsingLowestimationList");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<EliminationEnum> EliminationList
        {
            get
            {
                return eliminationList;
            }
            set
            {
                eliminationList = value;
                OnPropertyChanged("EliminationList");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<IsSortedEnum> IsSortedList
        {
            get
            {
                return isSortedList;
            }
            set
            {
                isSortedList = value;
                OnPropertyChanged("IsSortedList");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<SortedTypeEnum> SortedTypeList
        {
            get
            {
                return sortedTypeList;
            }
            set
            {
                sortedTypeList = value;
                OnPropertyChanged("SortedTypeList");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public EmptyOrderEnum SelectedEmptyOrder
        {
            get
            {
                return selectedEmptyOrder;
            }
            set
            {
                selectedEmptyOrder = value;
                OnPropertyChanged("SelectedEmptyOrder");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public BaseEnumerationEnum SelectedBaseEnumeration
        {
            get
            {
                return selectedBaseEnumeration;
            }
            set
            {
                selectedBaseEnumeration = value;
                OnPropertyChanged("SelectedBaseEnumeration");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public UsingLowestimationEnum SelectedUsingLowestimation
        {
            get
            {
                return selectedUsingLowestimation;
            }
            set
            {
                selectedUsingLowestimation = value;
                OnPropertyChanged("SelectedUsingLowestimation");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public IsSortedEnum SelectedIsSorted
        {
            get
            {
                return selectedIsSorted;
            }
            set
            {
                selectedIsSorted = value;
                OnPropertyChanged("SelectedIsSorted");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public SortedTypeEnum SelectedSortedType
        {
            get
            {
                return selectedSortedType;
            }
            set
            {
                selectedSortedType = value;
                OnPropertyChanged("SelectedSortedType");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public EliminationEnum SelectedElimination
        {
            get
            {
                return selectedElimination;
            }
            set
            {
                selectedElimination = value;
                OnPropertyChanged("SelectedElimination");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<SectionVertex> Vertex
        {
            get
            {
                return vertex;
            }
            set
            {
                vertex = value;
                OnPropertyChanged("Vertex");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RectangleInfoViewModel SelectedRectangle
        {
            get
            {
                return selectedRectangle;
            }
            set
            {
                selectedRectangle = value;
                OnPropertyChanged("SelectedRectangle");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<ConnectingLine> ConnectingLines
        {
            get
            {
                return connectingLines;
            }
            set
            {
                connectingLines = value;
                OnPropertyChanged("ConnectingLines");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<DiaElementViewModel> DiaElements
        {
            get
            {
                return diaElements;
            }
            set
            {
                diaElements = value;
                OnPropertyChanged("DiaElements");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double PanelHeight
        {
            get
            {
                return panelHeight;
            }
            set
            {
                panelHeight = value;
                OnPropertyChanged("PanelHeight");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double OriginePanelHeight
        {
            get
            {
                return originePanelHeight;
            }
            set
            {

                double newvalue = value;
                if (newvalue > 0.001)
                {
                    //if (Math.Abs(MultiplicationHeightDiaPanel - 1.0) > 0.001)
                    //    MultiplicationHeightDiaPanel *= newvalue / originePanelHeight;
                    originePanelHeight = newvalue;
                    OnPropertyChanged("OriginePanelHeight");
                    OnPropertyChanged("ActualPanelHeight");
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double ActualPanelHeight
        {
            get
            {
                return originePanelHeight * multiplicationHeightDiaPanel;
            }
            set
            {
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double PanelWidth
        {
            get
            {
                return panelWidth;
            }
            set
            {
                panelWidth = value;
                OnPropertyChanged("PanelWidth");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double OriginePanelWidth
        {
            get
            {
                return originePanelWidth;
            }
            set
            {
                double newvalue = value;
                if (newvalue > 0.001)
                {
                    //if (Math.Abs(MultiplicationWidthDiaPanel - 1.0) > 0.001)
                    //    MultiplicationWidthDiaPanel *= newvalue / originePanelWidth;
                    originePanelWidth = newvalue;
                    OnPropertyChanged("OriginePanelWidth");
                    OnPropertyChanged("ActualPanelWidth");
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double ActualPanelWidth
        {
            get
            {
                return originePanelWidth * multiplicationWidthDiaPanel;
            }
            set
            {
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double MultiplicationHeightDiaPanel
        {
            get
            {
                return multiplicationHeightDiaPanel;
            }
            set
            {
                multiplicationHeightDiaPanel = value;
                OnPropertyChanged("MultiplicationHeightDiaPanel");
                OnPropertyChanged("ActualPanelHeight");
                MultiplicationElementHeightDiaPanel = ActualPanelHeight/startOriginePanelHeight;
                RectangleInfoViewModel.MultiplicationElementHeightDiaPanel = MultiplicationElementHeightDiaPanel;
                ConnectLineInfoViewModel.MultiplicationElementHeightDiaPanel = MultiplicationElementHeightDiaPanel;
                ArrowInfoViewModel.MultiplicationElementHeightDiaPanel = MultiplicationElementHeightDiaPanel;
                if (DiaElements != null)
                    for (int i = 0; i < DiaElements.Count; i++)
                        DiaElements[i].NotifyUI();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double MultiplicationWidthDiaPanel
        {
            get
            {
                return multiplicationWidthDiaPanel;
            }
            set
            {
                multiplicationWidthDiaPanel = value;
                OnPropertyChanged("MultiplicationWidthDiaPanel");
                OnPropertyChanged("ActualPanelWidth");
                MultiplicationElementWidthDiaPanel = ActualPanelWidth / startOriginePanelWidth;
                RectangleInfoViewModel.MultiplicationElementWidthDiaPanel = MultiplicationElementWidthDiaPanel;
                ConnectLineInfoViewModel.MultiplicationElementWidthDiaPanel = MultiplicationElementWidthDiaPanel;
                ArrowInfoViewModel.MultiplicationElementWidthDiaPanel = MultiplicationElementWidthDiaPanel;
                if (DiaElements != null)
                    for (int i = 0; i < DiaElements.Count; i++)
                        DiaElements[i].NotifyUI();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double MultiplicationElementHeightDiaPanel
        {
            get
            {
                return multiplicationElementHeightDiaPanel;
            }
            set
            {
                multiplicationElementHeightDiaPanel = value;
                OnPropertyChanged("MultiplicationElementHeightDiaPanel");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double MultiplicationElementWidthDiaPanel
        {
            get
            {
                return multiplicationElementWidthDiaPanel;
            }
            set
            {
                multiplicationElementWidthDiaPanel = value;
                OnPropertyChanged("MultiplicationElementWidthDiaPanel");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<DiaMakerModel.DataBase> DataBases
        {
            get
            {
                return dataBases;
            }
            set
            {
                dataBases = value;
                OnPropertyChanged("DataBases");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<Table> Tables
        {
            get
            {
                return tables;
            }
            set
            {
                tables = value;
                OnPropertyChanged("Tables");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public Table SelectedTable
        {
            get
            {
                return selectedTable;
            }
            set
            {
                selectedTable = value;
                OnPropertyChanged("SelectedTable");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
                OnPropertyChanged("ConnectionString");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public string SearchTablePattern
        {
            get
            {
                return searchTablePattern;
            }
            set
            {
                searchTablePattern = value;
                OnPropertyChanged("SearchTablePattern");
            }
        }
        private int selectedDataBaseIndex = 0;
        //----------------------------------------------------------------------------------------------------------------------
        public int SelectedDataBaseIndex
        {
            get
            {
                return selectedDataBaseIndex;
            }
            set
            {
                selectedDataBaseIndex = value;
                OnPropertyChanged("SelectedDataBaseIndex");
            }
        }
        //--------------------------------------------------------------------------------------
        public int ChangeOptimalNumber
        {
            get
            {
                return changeOptimalNumber;
            }
            set
            {
                changeOptimalNumber = value;
                OnPropertyChanged("ChangeOptimalNumber");
            }
        }
        //--------------------------------------------------------------------------------------
        public int WeightOfCurrentRoute
        {
            get
            {
                return weightOfCurrentRoute;
            }
            set
            {
                weightOfCurrentRoute = value;
                OnPropertyChanged("WeightOfCurrentRoute");
            }
        }
        //--------------------------------------------------------------------------------------
        public int PositionInBestRoutes
        {
            get
            {
                return positionInBestRoutes;
            }
            set
            {
                positionInBestRoutes = value;
                OnPropertyChanged("PositionInBestRoutes");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public ObservableCollection<AlgorithmInfo> VerteciesInTableAlgorithms
        {
            get
            {
                return verteciesInTableAlgorithms;
            }
            set
            {
                verteciesInTableAlgorithms = value;
                OnPropertyChanged("VerteciesInTableAlgorithms");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public AlgorithmInfo SelectedVerteciesInTableAlgorithm
        {
            get
            {
                return selectedVerteciesInTableAlgorithm;
            }
            set
            {
                selectedVerteciesInTableAlgorithm = value;
                OnPropertyChanged("SelectedVerteciesInTableAlgorithm");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public bool IsColored
        {
            get
            {
                return isColored;
            }
            set
            {
                isColored = value;
                OnPropertyChanged("IsColored");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public int MaxDurability
        {
            get
            {
                return maxDurability;
            }
            set
            {
                maxDurability = value;
                OnPropertyChanged("MaxDurability");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public int BinSubsetPresentation
        {
            get
            {
                return binSubsetPresentation;
            }
            set
            {
                binSubsetPresentation = value;
                OnPropertyChanged("BinSubsetPresentation");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public int CountOfSelected
        {
            get
            {
                return countOfSelected;
            }
            set
            {
                countOfSelected = value;
                OnPropertyChanged("CountOfSelected");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public string ManualName
        {
            get
            {
                return manualName;
            }
            set
            {
                manualName = value;
                OnPropertyChanged("ManualName");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand GetDataBasesCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand GetTablesCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand CreateDiaCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand CreateDiaCustomCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand CreateDiaManualStringCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand CreateDiaUnionCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand PreviousOptCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand NextOptCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand IncreaseDiaAreaCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand DecreaseDiaAreaCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SelectAllCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SelectCurrentAndAdjacentsCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SelectCurrentAndAdjacentsTo2Command
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SelectAdjacentsForSelectedCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SelectGroupOfSelectedCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand CancalColoringCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SetFilterForSelectedCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand ClearFilteringCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand StartSearchTableCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand ContinueSearchTableCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand UnSelectAllCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SetSelectedCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public RelayCommand SelectTablesBaseOnBinPresentationCommand
        {
            get;
            private set;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public DiaViewModel()
        {
            multiplicationHeightDiaPanel = 1;
            multiplicationWidthDiaPanel = 1;
            multiplicationElementHeightDiaPanel = 1;
            multiplicationElementWidthDiaPanel = 1;
            originePanelWidth = 1;
            originePanelHeight = 1;
            startOriginePanelHeight = 1;
            startOriginePanelWidth = 1;
            InitializeCommands();
            InitializeData();
        }

        //----------------------------------------------------------------------------------------------------------------------
        private void InitializeData()
        {
            dataBases = new ObservableCollection<DiaMakerModel.DataBase>();
            dataBases = new ObservableCollection<DiaMakerModel.DataBase>();
            Tables = new ObservableCollection<Table>();
            ConnectionString = DiaMakerViewModel.Properties.Settings.Default.ConnectionString;
            if (Environment.MachineName.ToUpper() != "OKOLOMIYETS-N")
                ConnectionString = DiaMakerViewModel.Properties.Settings.Default.ConnectionString.Replace("CRM", "BOTANIK2008");
 
            PropertyChanged += Dictionary_PropertyChanged;
            CreateDataBaseList();
            if (DataBases.Count > 0)
                SelectedDataBaseIndex = 0;
            VerteciesInTableAlgorithms = new ObservableCollection<AlgorithmInfo>();
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "Enumerate"});
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "EnumerateAdvanced" });
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "ByVertexEnumerate" });
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "ByVertexEnumerateAdvanced" });
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "EnumerateWithLowEstimate"});
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "EnumerateAdvancedWithLowEstimate" });
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "ByVertexEnumerateWithLowEstimate" });
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "ByVertexEnumerateAdvancedWithLowEstimate" });
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "EnumerateSorted" });
            VerteciesInTableAlgorithms.Add(new AlgorithmInfo() { Name = "Approximate" });
            SelectedVerteciesInTableAlgorithm = VerteciesInTableAlgorithms[0];
            PropertyChanged += DiaViewModel_PropertyChanged;

            emptyOrderList = AlgorithmConfiguration.CreateObservableCollection<EmptyOrderEnum>();  // new ObservableCollection<EmptyOrderEnum>() { EmptyOrderEnum.EmptyFirst, EmptyOrderEnum.EmptyLast };
            baseEnumerationList = AlgorithmConfiguration.CreateObservableCollection<BaseEnumerationEnum>();  //new ObservableCollection<BaseEnumerationEnum>() { BaseEnumerationEnum.ByPlace, BaseEnumerationEnum.ByVertex }; ;
            usingLowestimationList = AlgorithmConfiguration.CreateObservableCollection<UsingLowestimationEnum>();  //new ObservableCollection<UsingLowestimationEnum>() { UsingLowestimationEnum.NotUseLowEstimation, UsingLowestimationEnum.WithUseLowEstation }; ;
            eliminationList = AlgorithmConfiguration.CreateObservableCollection<EliminationEnum>();  //new ObservableCollection<EliminationEnum>() { EliminationEnum.FullEnum, EliminationEnum.StrictElim, EliminationEnum.RughElim };
            isSortedList = AlgorithmConfiguration.CreateObservableCollection<IsSortedEnum>();
            SortedTypeList = AlgorithmConfiguration.CreateObservableCollection<SortedTypeEnum>();
            SelectedEmptyOrder = emptyOrderList[0];
            //selectedBaseEnumeration = baseEnumerationList[0];
            //selectedUsingLowestimation = usingLowestimationList[0];
            //selectedElimination = eliminationList[0];

        }
        //----------------------------------------------------------------------------------------------------------------------
        private void DiaViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedTable" && SelectedTable != null && DiaElements != null)
            {
                var rl = DiaElements.OfType<RectangleInfoViewModel>().ToList();
                var vv = rl.FirstOrDefault(v => v.Name == SelectedTable.Name);
                if (vv != null)
                {
                    if (SelectedRectangle != null)
                        SelectedRectangle.IsSelected = false;
                    SelectedRectangle = vv;
                    SelectedRectangle.IsSelected = true;
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void InitializeCommands()
        {
            GetDataBasesCommand =
                new RelayCommand(o =>
                {
                    CreateDataBaseList();
                },
                    o => true);
            GetTablesCommand =
                new RelayCommand(o =>
                {
                    CreateTablesList();
                },
                    o => true);
            CreateDiaCommand =
                new RelayCommand(o =>
                {
                    CreateDia();
                },
                    o => true);
            CreateDiaCustomCommand =
                new RelayCommand(o =>
                {
                    CreateDiaCustom();
                },
                    o => true);
            CreateDiaManualStringCommand =
                new RelayCommand(o =>
                {
                    CreateDiaManualString();
                },
                    o => true);
            CreateDiaUnionCommand =
                new RelayCommand(o =>
                {
                    CreateDiaUnion();
                },
                    o => true);
            PreviousOptCommand =
                new RelayCommand(o =>
                {
                    PreviousOptAction();
                },
                    o => CanPreviousOptAction());
            NextOptCommand =
                new RelayCommand(o =>
                {
                    NextOptAction();
                },
                    o => CanNextOptAction());
            IncreaseDiaAreaCommand =
                new RelayCommand(o =>
                {
                    IncreaseDiaAreaAction();
                },
                    o => CanIncreaseDiaAreaAction());
            DecreaseDiaAreaCommand =
                new RelayCommand(o =>
                {
                    DecreaseDiaAreaAction();
                },
                    o => CanDecreaseDiaAreaAction());
            SelectAllCommand =
                new RelayCommand(o =>
                {
                    SelectAll();
                },
                    o => true);
            SelectCurrentAndAdjacentsCommand =
                new RelayCommand(o =>
                {
                    SelectCurrentAndAdjacents();
                },
                    o => true);
            SelectCurrentAndAdjacentsTo2Command =
                new RelayCommand(o =>
                {
                    SelectCurrentAndAdjacentsTo2();
                },
                    o => true);
            SelectAdjacentsForSelectedCommand =
                new RelayCommand(o =>
                {
                    SelectAdjacentsForSelected();
                },
                    o => true);
            SelectGroupOfSelectedCommand =
                new RelayCommand(o =>
                {
                    SelectGroupOfSelected();
                },
                    o => true);
            CancalColoringCommand =
                new RelayCommand(o =>
                {
                    CancalColoringAction();
                },
                    o => true);
            SetFilterForSelectedCommand =
                new RelayCommand(o =>
                {
                    SetFilterForSelectedAction();
                },
                    o => true);
            ClearFilteringCommand =
                new RelayCommand(o =>
                {
                    ClearFilteringAction();
                },
                    o => true);
            StartSearchTableCommand =
                new RelayCommand(o =>
                {
                    StartSearchTableAction();
                },
                    o => true);
            ContinueSearchTableCommand =
                new RelayCommand(o =>
                {
                    ContinueSearchTableAction();
                },
                    o => true);
            UnSelectAllCommand =
                new RelayCommand(o =>
                {
                    UnSelectAll();
                },
                    o => true);
            SetSelectedCommand =
                new RelayCommand(o =>
                {
                    SetSelected(o);
                },
                    o => true);
            SelectTablesBaseOnBinPresentationCommand =

                new RelayCommand(o =>
                {
                    SelectTablesBaseOnBinPresentationAction();
                },
                    o => true);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void NextOptAction()
        {
            tracing.ToNextRoute();
            CreateDiaElements();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanNextOptAction()
        {
            return true; // tracing == null ? false : tracing.CanNextRoute();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void IncreaseDiaAreaAction()
        {
            MultiplicationHeightDiaPanel *= 1.1;
            MultiplicationWidthDiaPanel *= 1.1;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanIncreaseDiaAreaAction()
        {
            return true; // tracing == null ? false : tracing.CanNextRoute();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void DecreaseDiaAreaAction()
        {
            if ( minimalMultiplicationHeightDiaPanel + 0.0001 < MultiplicationHeightDiaPanel / 1.1)
                MultiplicationHeightDiaPanel /= 1.1;
            if (minimalMultiplicationWidthDiaPanel + 0.0001 < MultiplicationWidthDiaPanel / 1.1)
                MultiplicationWidthDiaPanel /= 1.1;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanDecreaseDiaAreaAction()
        {
            return true; // tracing == null ? false : tracing.CanNextRoute();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void PreviousOptAction()
        {
            tracing.ToPreviousRoute();
            CreateDiaElements();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private bool CanPreviousOptAction()
        {
            return true; // tracing == null ? false : tracing.CanPreviousRoute();
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void UnSelectAll()
        {
            for (int i = 0; i < Tables.Count; i++)
                Tables[i].IsSelected = false;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SelectAll()
        {
            for (int i = 0; i < Tables.Count; i++)
                Tables[i].IsSelected = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SelectCurrentAndAdjacents()
        {
            if (SelectedTable != null)
            {
                IVertex curVertex = fullGraph.Vertices.Where(v => v.Name == selectedTable.FullName).FirstOrDefault();
                var list = Tables.AsParallel().Where(t => curVertex.AdjacentVertices.Any(i => fullGraph.Vertices[i].Name == t.FullName)).ToList();

                //for (int i = 0; i < list.Count; i++)
                //    list[i].IsSelected = true;

                list.FindAll(t => t.IsSelected = true);

                SelectedTable.IsSelected = true;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SelectCurrentAndAdjacentsTo2()
        {
            if (SelectedTable != null)
            {
                IVertex curVertex = fullGraph.Vertices.Where(v => v.Name == selectedTable.Name).FirstOrDefault();
                var level1 = Tables.Where(t => curVertex.AdjacentVertices.Any(i => fullGraph.Vertices[i].Name == t.Name)).ToList();
                for (int i = 0; i < level1.Count; i++)
                    level1[i].IsSelected = true;

                if (level1.Count > 0)
                {
                    IEnumerable<IVertex> selVertices = fullGraph.Vertices.Where(v => level1.Any(m => v.Name == m.Name)).ToList();
                    var list = Tables.Where(t => selVertices.SelectMany(vv => vv.AdjacentVertices).Any(i => fullGraph.Vertices[i].Name == t.Name)).ToList();
                    for (int i = 0; i < list.Count; i++)
                        list[i].IsSelected = true;
                    SelectedTable.IsSelected = true;
                }


                SelectedTable.IsSelected = true;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SelectAdjacentsForSelected()
        {
            var markedTables = Tables.Where(t => t.IsSelected).ToList();
            if (markedTables.Count > 0)
            {
                IEnumerable<IVertex> selVertices = fullGraph.Vertices.Where(v => Tables.Any(m => v.Name == m.Name)).ToList();
                var list = Tables.Where(t => selVertices.SelectMany(vv => vv.AdjacentVertices).Any(i => fullGraph.Vertices[i].Name == t.Name)).ToList();
                for (int i = 0; i < list.Count; i++)
                    list[i].IsSelected = true;
                SelectedTable.IsSelected = true;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SelectGroupOfSelected()
        {
            var list = Tables.Where(t => t.Group == SelectedTable.Group).ToList();
            for (int i = 0; i < list.Count; i++)
                list[i].IsSelected = true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CancalColoringAction()
        {
            for (int i = 0; i < Tables.Count; i++)
                Tables[i].IsColored = false;
            ListCollectionView lcv = CollectionViewSource.GetDefaultView(Tables) as ListCollectionView;
            if (lcv != null && lcv.CanSort == true)
            {
                lcv.SortDescriptions.Clear();
                lcv.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
            IsColored = false;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SetFilterForSelectedAction()
        {
            OnDataGridCommitEdit();
            ListCollectionView lcv = CollectionViewSource.GetDefaultView(Tables) as ListCollectionView;
            if (lcv != null && lcv.CanFilter)
            {
                lcv.Filter = o =>
                {
                    Table t = o as Table;
                    if (t == null)
                        return false;
                    return t.IsSelected;
                };
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ClearFilteringAction()
        {
            ListCollectionView lcv = CollectionViewSource.GetDefaultView(Tables) as ListCollectionView;
            if (lcv != null && lcv.CanFilter)
            {
                OnDataGridCommitEdit();
                lcv.Filter = null;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void StartSearchTableAction()
        {
            if ( !string.IsNullOrEmpty(SearchTablePattern))
            {
                Table table = Tables.FirstOrDefault(t => t.Name.ToLower().Contains(SearchTablePattern.ToLower()));
                if (table != null)
                    SelectedTable = table;
                OnMainGridScrollIntoView(SelectedTable);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void ContinueSearchTableAction()
        {
            Table table = Tables.SkipWhile(t => !object.ReferenceEquals(t, SelectedTable)).Skip(1).FirstOrDefault(t => t.Name.ToLower().Contains(SearchTablePattern.ToLower()));
            if (table != null)
                SelectedTable = table;
            OnMainGridScrollIntoView(SelectedTable);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SetSelected(object pItem)
        {
            RectangleInfoViewModel newSelectedTable = pItem as RectangleInfoViewModel;
            if (newSelectedTable != null)
            {
                if (SelectedRectangle != null)
                    SelectedRectangle.IsSelected = false;
                SelectedRectangle = newSelectedTable;
                SelectedRectangle.IsSelected = true;
                var tt = Tables.FirstOrDefault(t => t.Name == SelectedRectangle.Name);
                if (tt != null)
                    SelectedTable = tt;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SelectTablesBaseOnBinPresentationAction()
        {
            int bnp = BinSubsetPresentation;
            for (int i = 0; i < Tables.Count; i++)
            {
                if ((bnp & (1 << i)) > 0)
                    Tables[i].IsSelected = true;
                else
                    Tables[i].IsSelected = false;
            }
        }
        ////----------------------------------------------------------------------------------------------------------------------
        //private void CreateDia()
        //{
        //    //PanelWidth *= 2;
        //    //PanelHeight *= 2;
        //    IGraph<IVertex> lGraph = tableKeys.CreateGraph();
        //    /*
        //    Dictionary<string, int> tableNumbers = new Dictionary<string, int>();
        //    IGraph<IVertex> lGraph = new Graph<IVertex>();
        //    int i = 0;
        //    foreach (var pair in tableKeys.Tables)
        //    {
        //        if (pair.Value.IsSelected)
        //        {
        //            IVertex v = new CVertex(pair.Key);
        //            lGraph.Vertices.Add(v);
        //            tableNumbers.Add(pair.Key, i);
        //            i++;
        //        }
        //    }
        //    foreach (var r in tableKeys.ForeignKeys)
        //    {
        //        if (tableNumbers.ContainsKey(r.TableFrom.Name) && tableNumbers.ContainsKey(r.TableTo.Name))
        //            lGraph.AddEdge(tableNumbers[r.TableFrom.Name], tableNumbers[r.TableTo.Name]);
                    
        //    }
        //    */

        //    PresentationData presentationData = new PresentationData(PanelWidth, PanelHeight, lGraph);
        //    tracing = new Tracing(lGraph, presentationData, SelectedVerteciesInTableAlgorithm.Name);
        //    tracing.Execute(MaxDurability * 1000);
        //    ChangeOptimalNumber = tracing.ChangeOptimalNumber; 

        //    tracing.CreateRectangleAndLine();

        //    CreateDiaElements();

        //}
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateDia()
        {
            CreateDiaCommon(GetTracingByName);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateDiaCustom()
        {
            CreateDiaCommon(GetTracingComposite);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateDiaManualString()
        {
            CreateDiaCommon(GetTracingCreateDiaManualString);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateDiaUnion()
        {
            CreateDiaCommon(GetTracingUnion);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateDiaCommon(Func<IGraph<IVertex>, PresentationData, Tracing> GetTracing)
        {
            //PanelWidth *= 2;
            //PanelHeight *= 2;
            IGraph<IVertex> lGraph = tableKeys.CreateGraph();

            startOriginePanelWidth = OriginePanelWidth;
            startOriginePanelHeight = OriginePanelHeight;

            PresentationData presentationData = new PresentationData(OriginePanelWidth, OriginePanelHeight, lGraph);
            minimalMultiplicationHeightDiaPanel = presentationData.MinimalMultiplicationHeightDiaPanel;
            minimalMultiplicationWidthDiaPanel = presentationData.MinimalMultiplicationWidthDiaPanel;
            if (MultiplicationHeightDiaPanel - 0.00001 < minimalMultiplicationHeightDiaPanel)
                MultiplicationHeightDiaPanel = presentationData.MinimalMultiplicationHeightDiaPanel;
            if (MultiplicationWidthDiaPanel  - 0.00001 < minimalMultiplicationWidthDiaPanel)
                MultiplicationWidthDiaPanel = presentationData.MinimalMultiplicationWidthDiaPanel;
            tracing = GetTracing(lGraph, presentationData);
            tracing.Execute(maxDurability * 1000);
            ChangeOptimalNumber = tracing.ChangeOptimalNumber;

            tracing.CreateRectangleAndLine();

            CreateDiaElements();

        }
        //----------------------------------------------------------------------------------------------------------------------
        private Tracing GetTracingByName(IGraph<IVertex> pGraph, PresentationData pPresentationData)
        {
            Tracing tracing = new Tracing(pGraph, pPresentationData, SelectedVerteciesInTableAlgorithm.Name);
            return tracing;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private Tracing GetTracingComposite(IGraph<IVertex> pGraph, PresentationData pPresentationData)
        {
            AlgorithmConfiguration lAlgorithmConfiguration = new AlgorithmConfiguration(SelectedEmptyOrder, SelectedBaseEnumeration,
                    SelectedUsingLowestimation, SelectedElimination, SelectedIsSorted, SelectedSortedType);
            Tracing tracing = new Tracing(pGraph, pPresentationData, lAlgorithmConfiguration);
            return tracing;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private Tracing GetTracingCreateDiaManualString(IGraph<IVertex> pGraph, PresentationData pPresentationData)
        {
            Tracing tracing = new Tracing(pGraph, pPresentationData, ManualName);
            return tracing;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private Tracing GetTracingUnion(IGraph<IVertex> pGraph, PresentationData pPresentationData)
        {
            Tracing tracing = new Tracing(pGraph, pPresentationData, "Union");
            return tracing;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateDiaElements()
        {
            if (tracing.SectionMatrix != null)
            {
                RectangleInfoViewModel.MultiplicationElementHeightDiaPanel = MultiplicationElementHeightDiaPanel;
                RectangleInfoViewModel.MultiplicationElementWidthDiaPanel = MultiplicationElementWidthDiaPanel;
                DiaElements = new ObservableCollection<DiaElementViewModel>();
                foreach (var v in tracing.SectionMatrix.OfType<SectionVertex>().Where(v => !v.IsEmptyPlace))
                    DiaElements.Add(new RectangleInfoViewModel(v));
                foreach (var l in tracing.ConnectingLines)
                    DiaElements.Add(new ConnectLineInfoViewModel(l));
                foreach (var a in tracing.ArrowDatas)
                    DiaElements.Add(new ArrowInfoViewModel(a));
                WeightOfCurrentRoute = tracing.WeightOfCurrentRoute;
                PositionInBestRoutes = tracing.PositionInBestRoutes;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateDataBaseList()
        {
            DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(ConnectionString);
            DataBases.Clear();
            foreach (var d in dataBaseRepository.DataBases)
                DataBases.Add(d);
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void CreateTablesList()
        {
            if (selectedDataBaseIndex >= 0)
            {
                DataBaseRepository dataBaseRepository = DataBaseRepository.CreateInstance(ConnectionString);
                string dataBasesName = dataBases[selectedDataBaseIndex].Name;
                tableKeys = dataBaseRepository.GetTables(dataBasesName);
                Tables.Clear();
                foreach (var d in tableKeys.Tables.Values)
                {
                    d.PropertyChanged += table_PropertyChanged;
                    Tables.Add(d);
                }
                fullGraph = tableKeys.CreateGraph(true);
                GraphLib.GraphSplitter graphSplitter = new GraphLib.GraphSplitter(fullGraph);
                subGraphList = graphSplitter.CreateConnectedSubGraphes();
                Table.CreateTableColorSet(subGraphList.Count);
                int lGroup = 0;
                foreach (IGraph<IVertex> sg in subGraphList.OrderByDescending(g => g.Vertices.Count))
                {
                    List<Table> foundTables = Tables.Where(t => sg.Vertices.Any(v => v.Name == t.Name)).ToList();
                    for (int i = 0; i < foundTables.Count; i++)
                        foundTables[i].Group = lGroup;
                    lGroup++;
                }
                ListCollectionView lcv = CollectionViewSource.GetDefaultView(Tables) as ListCollectionView;
                if (lcv != null && lcv.CanSort == true)
                {
                    lcv.SortDescriptions.Clear();
                    lcv.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
                    lcv.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        void table_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                int binPresentation = 0;
                for (int i = 0; i < tables.Count; i++)
                {
                    if (tables[i].IsSelected)
                        binPresentation |= 1 << i;
                }
                BinSubsetPresentation = binPresentation;
                CountOfSelected = tables.Count(t => t.IsSelected);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void Dictionary_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDataBaseIndex")
            {
                CreateTablesList();
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        protected virtual void OnMainGridScrollIntoView(object item)
        {
            Action<object> handler = MainGridScrollIntoView;
            if (handler != null)
                handler(item);
        }
        //-------------------------------------------------------------------------------------------------------------------
        protected virtual void OnDataGridCommitEdit()
        {
            Action handler = DataGridCommitEdit;
            if (handler != null)
                handler();
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
 