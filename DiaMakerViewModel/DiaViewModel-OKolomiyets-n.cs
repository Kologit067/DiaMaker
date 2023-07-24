using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiaMakerModel;
using System.Collections.ObjectModel;
using DiaMakerModel.Tracing;
using Common.CommonLib.Interfaces;
using Caculation.GraphLib;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using CommonLib;

namespace DiaMakerViewModel
{
    /*
     1  Scrolling
     2  Issue: equal count of itteration of ALL algorythm
     7  Option to tune maximum of execution time, appropriate change in UI and VM (ui control, property, command and etc) 
     6  united algorythm (better exact and approximate)
     8  Selection vertices (path length < 2 from current) 
     9  Test method with parameter defining set of subgraph for testing for example (1,5,7)
     10 Test for asinchronous algorythm
     -------------------------------
     * 
     * 
     * 
     * 
     3  algorythm with previous sorting of vertices (starting from vertix with more weight)
     4  algorythm with previous sorting of place (starting with central place)
     5  edge enumeration
     11 Handling exception in new asinchronous algorythm
     12 Possibility to change size of working panel
     13 calculation of minimal size of working panel
     14 add checking fo optimal route into general test method
     15 store procedure for saving statistics to archive
     16 usage parallel lynq
     17 details of algorythm performance (option fo saving)
     18 show on UI binary presentation of selected subset of tables set 
      
    */
    //----------------------------------------------------------------------------------------------------------------------
// class DiaViewModel
//----------------------------------------------------------------------------------------------------------------------
    public class DiaViewModel : ViewModelBase
    {
        private ObservableCollection<EmptyOrderEnum> emptyOrderList;
        private ObservableCollection<BaseEnumerationEnum> baseEnumerationList;
        private ObservableCollection<UsingLowestimationEnum> usingLowestimationList;
        private ObservableCollection<EliminationEnum> eliminationList;

        private EmptyOrderEnum selectedEmptyOrder;
        private BaseEnumerationEnum selectedBaseEnumeration;
        private UsingLowestimationEnum selectedUsingLowestimation;
        private EliminationEnum selectedElimination;
        
        private ObservableCollection<DiaMakerModel.DataBase> dataBases;
        private ObservableCollection<DiaElementViewModel> diaElements;
        private ObservableCollection<Table> tables;
        private ObservableCollection<SectionVertex> vertex;
        private ObservableCollection<ConnectingLine> connectingLines;
        private ObservableCollection<AlgorythmInfo> verteciesInTableAlgorythms;

        private AlgorythmInfo selectedVerteciesInTableAlgorythm;
        private Table selectedTable;
        private TableKeys tableKeys;
        private string connectionString;
        private double panelHeight;
        private double panelWidth;
        private int changeOptimalNumber;
        private int weightOfCurrentRoute;
        private int positionInBestRoutes;
        private int maxDurability = 120;
        private IGraph<IVertex> fullGraph;
        private List<IGraph<IVertex>> subGraphList;
        private Tracing tracing;
        private RectangleInfoViewModel selectedRectangle;
        private bool isColored = true;
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
        public ObservableCollection<AlgorythmInfo> VerteciesInTableAlgorythms
        {
            get
            {
                return verteciesInTableAlgorythms;
            }
            set
            {
                verteciesInTableAlgorythms = value;
                OnPropertyChanged("VerteciesInTableAlgorythms");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public AlgorythmInfo SelectedVerteciesInTableAlgorythm
        {
            get
            {
                return selectedVerteciesInTableAlgorythm;
            }
            set
            {
                selectedVerteciesInTableAlgorythm = value;
                OnPropertyChanged("SelectedVerteciesInTableAlgorythm");
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
        public DiaViewModel()
        {
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
            VerteciesInTableAlgorythms = new ObservableCollection<AlgorythmInfo>();
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "Enumerate"});
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "EnumerateAdvanced" });
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "ByVertexEnumerate" });
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "ByVertexEnumerateAdvanced" });
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "EnumerateWithLowEstimate"});
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "EnumerateAdvancedWithLowEstimate" });
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "ByVertexEnumerateWithLowEstimate" });
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "ByVertexEnumerateAdvancedWithLowEstimate" });
            VerteciesInTableAlgorythms.Add(new AlgorythmInfo() { Name = "Approximate"});
            SelectedVerteciesInTableAlgorythm = VerteciesInTableAlgorythms[0];
            PropertyChanged += DiaViewModel_PropertyChanged;

            emptyOrderList = new ObservableCollection<EmptyOrderEnum>() { EmptyOrderEnum.EmptyFirst, EmptyOrderEnum.EmptyLast };
            baseEnumerationList = new ObservableCollection<BaseEnumerationEnum>() { BaseEnumerationEnum.ByPlace, BaseEnumerationEnum.ByVertex }; ;
            usingLowestimationList = new ObservableCollection<UsingLowestimationEnum>() { UsingLowestimationEnum.NotUseLowEstimation, UsingLowestimationEnum.WithUseLowEstation }; ;
            eliminationList = new ObservableCollection<EliminationEnum>() { EliminationEnum.FullEnum, EliminationEnum.StrictElim, EliminationEnum.RughElim };
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
                IVertex curVertex = fullGraph.Vertices.Where(v => v.Name == selectedTable.Name).FirstOrDefault();
                var list = tables.AsParallel().Where(t => curVertex.AdjacentVertices.Any(i => fullGraph.Vertices[i].Name == t.Name)).ToList();

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
                var level1 = tables.Where(t => curVertex.AdjacentVertices.Any(i => fullGraph.Vertices[i].Name == t.Name)).ToList();
                for (int i = 0; i < level1.Count; i++)
                    level1[i].IsSelected = true;

                if (level1.Count > 0)
                {
                    IEnumerable<IVertex> selVertices = fullGraph.Vertices.Where(v => level1.Any(m => v.Name == m.Name)).ToList();
                    var list = tables.Where(t => selVertices.SelectMany(vv => vv.AdjacentVertices).Any(i => fullGraph.Vertices[i].Name == t.Name)).ToList();
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
            var markedTables = tables.Where(t => t.IsSelected).ToList();
            if (markedTables.Count > 0)
            {
                IEnumerable<IVertex> selVertices = fullGraph.Vertices.Where(v => markedTables.Any( m => v.Name == m.Name)).ToList();
                var list = tables.Where(t => selVertices.SelectMany(vv => vv.AdjacentVertices).Any(i => fullGraph.Vertices[i].Name == t.Name)).ToList();
                for (int i = 0; i < list.Count; i++)
                    list[i].IsSelected = true;
                SelectedTable.IsSelected = true;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void SelectGroupOfSelected()
        {
            var list = tables.Where(t => t.Group == SelectedTable.Group).ToList();
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
        //    tracing = new Tracing(lGraph, presentationData, SelectedVerteciesInTableAlgorythm.Name);
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

            PresentationData presentationData = new PresentationData(PanelWidth, PanelHeight, lGraph);
            tracing =  GetTracing(lGraph, presentationData);
            tracing.Execute(maxDurability * 1000);
            ChangeOptimalNumber = tracing.ChangeOptimalNumber;

            tracing.CreateRectangleAndLine();

            CreateDiaElements();

        }
        //----------------------------------------------------------------------------------------------------------------------
        private Tracing GetTracingByName(IGraph<IVertex> pGraph, PresentationData pPresentationData)
        {
            Tracing tracing = new Tracing(pGraph, pPresentationData, SelectedVerteciesInTableAlgorythm.Name);
            return tracing;
        }
        //----------------------------------------------------------------------------------------------------------------------
        private Tracing GetTracingComposite(IGraph<IVertex> pGraph, PresentationData pPresentationData)
        {
            Tracing tracing = new Tracing(pGraph, pPresentationData, SelectedEmptyOrder, SelectedBaseEnumeration,
                    SelectedUsingLowestimation, SelectedElimination);
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
                    Tables.Add(d);
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
        private void Dictionary_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDataBaseIndex")
            {
                CreateTablesList();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
 