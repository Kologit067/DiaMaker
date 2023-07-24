using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimalPositionLib.Matrix;
using Common.CommonLib.Interfaces;
using OptimalPositionLib;
using CommonLib;
using CommonLib.Tools;

namespace DiaMakerModel.Tracing
{
    /// <summary>
    /// source data 
    /// list of Nodes and list of Edges
    /// 1 define optimal size of matrix X,Y
    /// 1.1 define HorisontalStripWidth, HorisontalStripIndent, VerticalStripWidth, VerticalStripIndent
    /// 2 define optimal position in matrix
    /// 3 create list of DataRectangles and 2-dimension array of DataRectangles
    /// 2.1 define coordinate of DataRectangles
    /// 4 Create list of empty ConnectingLines (only first and last points)
    /// 5 Create lists of HorisontalStrips and VerticalStrips
    /// 6 create 1-2 variants of section sequance for each ConnectingLine
    /// 6.1 select optimal variant
    /// 7 define order of ConectingLine  for each Horisontal and Vertical Strings
    /// 8 define position of ConectingLine  for each Horisontal and Vertical Strings
    /// 9 define position of ConnectingLine in DataRectangle
    /// 10 define sequance of point
    /// goal: get 
    /// </summary>
    //--------------------------------------------------------------------------------------
    // class Tracing
    //--------------------------------------------------------------------------------------
    public partial class Tracing
    {
        private List<ConnectingLine> allPossibleConnectingLines;
        private List<ConnectingLine> connectingLines;
        private List<ArrowData> arrowDatas = new List<ArrowData>();
        private int[,] connectingLineRelation;
        private int[,] connectingLineIntersection;
//        private Tuple<int, int, int>[,] connectingLineRelation_;
        private List<Strip> strips = new List<Strip>();
        private List<HorisontalStrip> horisontalStrips = new List<HorisontalStrip>();
        private List<VerticalStrip> verticalStrips = new List<VerticalStrip>();
        private Section[,] sectionMatrix;
        private IGraph<IVertex> graph;
        private int[] optimalRoute;
        private Tuple<int, int> sizes;
        private PresentationData presentationData;
        private List<Tuple<int[],int>> bestRoutes;
        private int positionInBestRoutes;
        private int changeOptimalNumber;
        private IMatrixOfDistance matrixofDistance;
        private string algorithmName;
        private AlgorithmConfiguration algorithmConfiguration;
        //--------------------------------------------------------------------------------------
        public double HorisontalStripWidth
        {
            get
            {
                return presentationData.HeightStrip;
            }
        }
        //--------------------------------------------------------------------------------------
        public double HorisontalStripIndent
        {
            get
            {
                return presentationData.HorisontalStripIndent;
            }
        }

        //--------------------------------------------------------------------------------------
        public double VerticalStripWidth
        {
            get
            {
                return presentationData.WidthStrip;
            }
        }
        //--------------------------------------------------------------------------------------
        public double VerticalStripIndent
        {
            get
            {
                return presentationData.VerticalStripIndent;
            }
        }

        //--------------------------------------------------------------------------------------
        public List<ConnectingLine> ConnectingLines
        {
            get
            {
                return connectingLines;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<ArrowData> ArrowDatas
        {
            get
            {
                return arrowDatas;
            }
        }
        //--------------------------------------------------------------------------------------
        public Section[,] SectionMatrix
        {
            get
            {
                return sectionMatrix;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<HorisontalStrip> HorisontalStrips
        {
            get
            {
                return horisontalStrips;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<VerticalStrip> VerticalStrips
        {
            get
            {
                return verticalStrips;
            }
        }
        //--------------------------------------------------------------------------------------
        public int ChangeOptimalNumber
        {
            get
            {
                return changeOptimalNumber;
            }
        }
        //--------------------------------------------------------------------------------------
        public List<Tuple<int[], int>> BestRoutes
        {
            get
            {
                return bestRoutes;
            }
        }
        //--------------------------------------------------------------------------------------
        public int WeightOfCurrentRoute
        {
            get
            {
                return BestRoutes[PositionInBestRoutes].Item2;
            }
        }
        //--------------------------------------------------------------------------------------
        public int PositionInBestRoutes
        {
            get
            {
                return positionInBestRoutes;
            }
        }
        //--------------------------------------------------------------------------------------
        public Tracing(IGraph<IVertex> pGraph, PresentationData pPresentationData)
        {
            presentationData = pPresentationData;
            graph = pGraph;
            // определить размер матрицы секций
            sizes = presentationData.Sizes;
            if (sizes.Item1 == 0 || sizes.Item2 == 0)
                return;
            // создать матрицу расстояниц
            matrixofDistance = new CMatrixOfDistance(sizes.Item1, sizes.Item2);
            matrixofDistance.Generate();

        }
        //--------------------------------------------------------------------------------------
        public Tracing(IGraph<IVertex> pGraph, PresentationData pPresentationData, string pAlgorithmName = "Enumerate")
            : this(pGraph, pPresentationData)
        {
            algorithmName = pAlgorithmName;
        }
        //--------------------------------------------------------------------------------------
        public Tracing(IGraph<IVertex> pGraph, PresentationData pPresentationData, AlgorithmConfiguration pAlgorithmConfiguration)
            : this(pGraph, pPresentationData)
        {
            algorithmConfiguration = pAlgorithmConfiguration;
        }
        //--------------------------------------------------------------------------------------
        public void Execute(int pMaxDurability)
        {
            // определить оптимальное размещение вершин в матрице секций
            //            VerteciesInTablePlaceEnumerate lVerteciesInTableEnumerate = new VerteciesInTablePlaceEnumerate(graph, matrixofDistance);
            VerteciesInTableDefine lVerteciesInTableEnumerate;
            if (!string.IsNullOrWhiteSpace(algorithmName))
                lVerteciesInTableEnumerate = VerteciesInTableDefine.Create(algorithmName, graph, matrixofDistance, pMaxDurability);
            else
                lVerteciesInTableEnumerate = VerteciesInTableDefine.Create(graph, matrixofDistance, algorithmConfiguration, pMaxDurability);

            lVerteciesInTableEnumerate.Execute();
            optimalRoute = lVerteciesInTableEnumerate.OptimalRoute;
            bestRoutes = lVerteciesInTableEnumerate.OptimalStorage.ToList();
            positionInBestRoutes = bestRoutes.Count - 1;
            changeOptimalNumber = lVerteciesInTableEnumerate.ChangeOptimalNumber;
        }
        //--------------------------------------------------------------------------------------
        private void SetOptimalRoute(int[] pNewroute)
        {
            optimalRoute = pNewroute;
        }
        //--------------------------------------------------------------------------------------
        public bool CanPreviousRoute()
        {
            if (bestRoutes != null && positionInBestRoutes > 0)
            {
                return true;
            }
            return false;
        }
        //--------------------------------------------------------------------------------------
        public void ToPreviousRoute()
        {
            if (positionInBestRoutes > 0)
            {
                positionInBestRoutes--;
                CreateRectangleAndLineForCurrentPosition();
            }
        }
        //--------------------------------------------------------------------------------------
        public bool CanNextRoute()
        {
            if (bestRoutes  != null && positionInBestRoutes < bestRoutes.Count - 1)
            {
                return true;
            }
            return false;
        }
        //--------------------------------------------------------------------------------------
        public void ToNextRoute()
        {
            if (positionInBestRoutes < bestRoutes.Count - 1)
            {
                positionInBestRoutes++;
                CreateRectangleAndLineForCurrentPosition();
            }
        }
        //--------------------------------------------------------------------------------------
        public void CreateRectangleAndLineForCurrentPosition()
        {
            SetOptimalRoute(bestRoutes[positionInBestRoutes].Item1);
            CreateRectangleAndLine();
        }
        //--------------------------------------------------------------------------------------
        public void CreateRectangleAndLine()
        {
            if (optimalRoute == null)
                return;
            // заполнить матрицу секций
            sectionMatrix = CreateSectionMatrix(sizes, optimalRoute);
            DefineNeighborForSectionInStrip();
            // создать массив ребер
            CreateEdgesList(graph, optimalRoute);
            // соэдание полос
            CreateStrips();
            // создать полный список соединительных линий  и заполнить соединительные линии для ребер
            CreateConnectingLine();
            CreateStripLists();
            // создать матрице отношений соединительных линий  
            CreateConnectingLineRelation();
            // перебор всез комбинаций соединительных линий для ребер и выбор оптимальной
            TraverseLineCombination();
            // определить порядок линий средиполос
            DefineLineOrder(); // 
            // определть положение линий внутри полос и в секциях
            DefineLinePosition();
            // определить последовательность точек в соединительных линиях
            DefineSequanceOfPoint();
            CreateArrow();
        }
        //--------------------------------------------------------------------------------------
        private void CreateStripLists()
        {
            for (int i = 0; i < allPossibleConnectingLines.Count; i++)
            {
                allPossibleConnectingLines[i].CreateStripList(horisontalStrips, verticalStrips);
            }
        }
        //--------------------------------------------------------------------------------------
        private void CreateConnectingLine()
        {
            allPossibleConnectingLines = new List<ConnectingLine>();
            foreach (Edge e in edges)
            {
                e.CreateConnectingLine(sectionMatrix,this);
                foreach (var l in e.ConnectingLines)
                {
                    allPossibleConnectingLines.Add(l);
                    l.Index = allPossibleConnectingLines.Count - 1;
                }
            }
        }

        //--------------------------------------------------------------------------------------
        private void CreateEdgesList(IGraph<IVertex> pGraph, int[] pRoute)
        {
            edges = new List<Edge>();
            int lSize = pRoute.Length;
            Int32[] lPositionOfVertex = new Int32[pGraph.Vertices.Count];
            for (int i = 0; i < lSize; i++)
                if (pRoute[i] > 0)
                    lPositionOfVertex[pRoute[i] - 1] = i;


            for (int i = 0; i < lSize; i++)
            {
                if (pRoute[i] > 0)
                {
                    int lX1 = (i / sizes.Item2) * 2;
                    int lY1 = (i % sizes.Item2) * 2;
                    foreach (int indVertex in pGraph.Vertices[pRoute[i] - 1].EndPoints)
                    {
                        int lX2 = (lPositionOfVertex[indVertex] / sizes.Item2) * 2;
                        int lY2 = (lPositionOfVertex[indVertex] % sizes.Item2) * 2;
                        SectionVertex section1 = sectionMatrix[lX1, lY1] as SectionVertex;
                        SectionVertex section2 = sectionMatrix[lX2, lY2] as SectionVertex;
                        if (section1 == null || section2 == null)
                            throw new Exception("logical error in CreateEdgesList - incorect section type");
                        edges.Add(new Edge(section1, section2));
                    }
                }
            }
        }


        //--------------------------------------------------------------------------------------
        private void CreateStrips()
        {
            horisontalStrips = new List<HorisontalStrip>();
            verticalStrips = new List<VerticalStrip>();
            strips = new List<Strip>();
            for (int i = 1; i < sectionMatrix.GetLength(0); i += 2)
            {
                Section[] lSections = new Section[sectionMatrix.GetLength(1)];
                for (int j = 0; j < sectionMatrix.GetLength(1); j++)
                {
                    lSections[j] = sectionMatrix[i, j];
                }
                VerticalStrip lStrip = new VerticalStrip(this, lSections, lSections[0].PositionX);
                strips.Add(lStrip);
                verticalStrips.Add(lStrip);
            }
            for (int j = 1; j < sectionMatrix.GetLength(1); j += 2)
            {
                Section[] lSections = new Section[sectionMatrix.GetLength(0)];
                for (int i = 0; i < sectionMatrix.GetLength(0); i++)
                {
                    lSections[i] = sectionMatrix[i, j];
                }
                HorisontalStrip lStrip = new HorisontalStrip(this, lSections, lSections[0].PositionY);
                strips.Add(lStrip);
                horisontalStrips.Add(lStrip);
            }
        }
        //--------------------------------------------------------------------------------------
        private Section[,] CreateSectionMatrix(Tuple<int, int> sizes, int[] pOptimalRoute)
        {
            if (pOptimalRoute == null)
                return null;
            int lHorisontalSize = sizes.Item1 * 2 - 1;
            int lVerticalSize = sizes.Item2 * 2 - 1;
            Section[,] lSectionMatrix = new Section[lHorisontalSize, lVerticalSize];

            double lLeft = 0;
            int lPositionInVertices = 0;
            for (int i = 0; i < lHorisontalSize; i++)
            {
                double lTop = 0;
                bool isIEven = i % 2 == 0;
                for (int j = 0; j < lVerticalSize; j++)
                {
                    bool isjEven = j % 2 == 0;
                    if (isIEven && isjEven)
                    {
//                        int lPositionInVertices = i * sizes.Item1 + j;
                        if (pOptimalRoute[lPositionInVertices] > 0)
                        {
                            lSectionMatrix[i, j] = new SectionVertex(graph.Vertices[pOptimalRoute[lPositionInVertices] - 1],
                                lLeft, lTop, i, j, presentationData);
                        }
                        else
                        {
                            lSectionMatrix[i, j] = new SectionVertex(null,
                                lLeft, lTop, i, j, presentationData);
                        }
                        lPositionInVertices++;
                    }
                    else if (!isIEven && isjEven)
                    {
                        lSectionMatrix[i, j] = new SectionInVerticalStrip(lLeft, lTop, presentationData);
                    }
                    else if (isIEven && !isjEven)
                    {
                        lSectionMatrix[i, j] = new SectionInHorisontalStrip(lLeft, lTop, presentationData);
                    }
                    else if (!isIEven && !isjEven)
                    {
                        lSectionMatrix[i, j] = new SectionIntersection(lLeft, lTop, presentationData);
                    }

                    if (isjEven)
                        lTop += presentationData.HeightVertex;
                    else
                        lTop += presentationData.HeightStrip;
                }
                if (isIEven)
                    lLeft += presentationData.WidthVertex;
                else
                    lLeft += presentationData.WidthStrip;
            }

            return lSectionMatrix;
        }
        //--------------------------------------------------------------------------------------
        private void DefineNeighborForSectionInStrip()
        {
            // Section In Horisontal Strip
            for (int i = 0; i < SectionMatrix.GetLength(0); i += 2)
            {
                for (int j = 1; j < SectionMatrix.GetLength(1); j += 2)
                {
                    SectionInHorisontalStrip section = SectionMatrix[i, j] as SectionInHorisontalStrip;
                    if (section == null)
                        throw new Exception("Logical error in DefineNeighborForSectionInStrip. Wrong section type");
                    SectionVertex lNextTop = SectionMatrix[i, j - 1] as SectionVertex;
                    SectionVertex lNextBottom = SectionMatrix[i, j + 1] as SectionVertex;
                    if (lNextTop == null || lNextBottom == null)
                        throw new Exception("Logical error in DefineNeighborForSectionInStrip. Wrong Neighbor type");
                    section.SetNeighbor(lNextTop, lNextBottom);
                }
            }
            // Section In Vertical Strip
            for (int i = 1; i < SectionMatrix.GetLength(0); i += 2)
            {
                for (int j = 0; j < SectionMatrix.GetLength(1); j += 2)
                {
                    SectionInVerticalStrip section = SectionMatrix[i, j] as SectionInVerticalStrip;
                    if (section == null)
                        throw new Exception("Logical error in DefineNeighborForSectionInStrip. Wrong section type");
                    SectionVertex lNextLeft = SectionMatrix[i - 1, j] as SectionVertex;
                    SectionVertex lNextRight = SectionMatrix[i + 1, j] as SectionVertex;
                    if (lNextLeft == null || lNextRight == null)
                        throw new Exception("Logical error in DefineNeighborForSectionInStrip. Wrong Neighbor type");
                    section.SetNeighbor(lNextLeft, lNextRight);
                }
            }
        }
        //--------------------------------------------------------------------------------------
        private void CreateConnectingLineRelation()
        {
            connectingLineRelation = new int[allPossibleConnectingLines.Count, allPossibleConnectingLines.Count];
            connectingLineIntersection = new int[allPossibleConnectingLines.Count, allPossibleConnectingLines.Count];
//            for (int i = 0; i < allPossibleConnectingLines.Count; i++)
            Parallel.For(0, allPossibleConnectingLines.Count, i =>
            {
                for (int j = i + 1; j < allPossibleConnectingLines.Count; j++)
                {
                    Tuple<int, int> r = allPossibleConnectingLines[i].DefineRelation(allPossibleConnectingLines[j]);
                    connectingLineRelation[i, j] = r.Item1;
                    connectingLineRelation[j, i] = r.Item1 * (-1);
                    if (r.Item2 > 0)
                    {
                        connectingLineIntersection[i, j] = r.Item2;
                        connectingLineIntersection[j, i] = r.Item2;
                    }
                }
            });
        }
        //--------------------------------------------------------------------------------------
        private void DefineLineOrder()
        {
            // parallel?
            foreach (SectionVertex s in sectionMatrix.OfType<SectionVertex>())
            {
                //SectionVertex sv = s as SectionVertex;
                //if (sv != null)
                    s.DefineLineOrder(connectingLineRelation);
            }
            foreach (Strip strip in strips)
            {
                strip.DefineLineOrder(connectingLineRelation);
            }
            foreach (SectionInStrip s in sectionMatrix.OfType<SectionInStrip>())
            {
                s.DefineLineOrder();
            }
        }
        //--------------------------------------------------------------------------------------
        private void DefineLinePosition()
        {
            // parallel?
            foreach (Section s in sectionMatrix)
            {
                SectionVertex sv = s as SectionVertex;
                if (sv != null)
                    sv.DefineLinePosition();
            }
            foreach (Strip strip in strips)
            {
                strip.DefineLinePosition();
            }
            foreach (SectionInStrip s in sectionMatrix.OfType<SectionInStrip>())
            {
                s.DefineLinePosition();
            }
        }
        //--------------------------------------------------------------------------------------
        private void DefineSequanceOfPoint()
        {
            foreach (ConnectingLine line in connectingLines)
            {
                line.DefineSequanceOfPoint();
            }
        }
        //--------------------------------------------------------------------------------------
        private void CreateArrow()
        {
            arrowDatas = new List<ArrowData>();
            foreach (ConnectingLine line in connectingLines)
            {
                arrowDatas.Add(line.CreateArrow());
            }
        }
        //--------------------------------------------------------------------------------------
        internal int CompareLine(ConnectingLine connectingLine1, ConnectingLine connectingLine2)
        {
            return connectingLine1.IndexInOptimalList < connectingLine2.IndexInOptimalList ? -1 : connectingLine1.IndexInOptimalList == connectingLine2.IndexInOptimalList ? 0 : 1;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
