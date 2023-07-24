using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;
using CommonLib;

namespace OptimalPositionLib
{
    // EmptyFirst/EmptyLast
    // ByVertex/ByPlace
    // WithUseLowEst/NotUseLowEst
    // FullEnum/StructElim/RughElim
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableDefine
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableDefine : PermitationBase
    {
        protected IGraph<IVertex> fGraph;
        protected IMatrixOfDistance fMatrixofDistance;
        protected int[] fOptimalRoute = null;				// маршрут обхода
        protected Queue<Tuple<int[], int>> optimalStorage;
        protected int changeOptimalNumber;
        protected int fOptimalWeight = 0;
        protected int lowEstimate = 0;
        protected string algorythmName;
        protected int[] fOptimalPlaces;
        protected int currentWeightOfRoute;
        protected int[] weightOnStep;
        protected Int32[] positionOfVertex;
        protected Int32[] positionOfPlace;
        protected BaseEnumerationEnum baseEnumeration;
        private Func<bool> IsEliminableMethod;
        private Action ChangeOptimalWeightActionMethod;
        private Action<int> BackActionMethod;
//        private bool isComplete;
        //--------------------------------------------------------------------------------------
        public virtual int[] OptimalRoute
        {
            get
            {
                if (baseEnumeration == BaseEnumerationEnum.ByPlace)
                    return fOptimalRoute;
                else
                    return fOptimalPlaces;
            }
        }
        //--------------------------------------------------------------------------------------
        public Queue<Tuple<int[], int>> OptimalStorage
        {
            get
            {
                return optimalStorage;
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
        public int OptimalWeight
        {
            get
            {
                return fOptimalWeight;
            }
        }
        //--------------------------------------------------------------------------------------
        public int LowEstimate
        {
            get
            {
                return lowEstimate;
            }
        }
        //--------------------------------------------------------------------------------------
        public string AlgorythmName
        {
            get
            {
                return algorythmName;
            }
        }
        //--------------------------------------------------------------------------------------
        public IGraph<IVertex> Graph
        {
            get
            {
                return fGraph;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsComplete
        {
            get
            {
                return !fOutQueryStop;
            }
        }
        //--------------------------------------------------------------------------------------
        public string OptimalRouteAsString
        {
            get
            {
                if (OptimalRoute != null && OptimalRoute.Length > 0)
                    return string.Join(",", OptimalRoute.Select(i => i.ToString()));
                return "Empty";
            }
        }
        //--------------------------------------------------------------------------------------
        private VerteciesInTableDefine(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance,
            int pSize, int pNumberOfPlace, int pMaxDurability)
            : base(pSize, pNumberOfPlace, pMaxDurability)
        {
            fGraph = pGraph;
            fMatrixofDistance = pMatrixofDistance;
            fOptimalRoute = new int[fNumberOfPlace];
            optimalStorage = new Queue<Tuple<int[], int>>(15);
            fOptimalWeight = int.MaxValue;
            fOptimalPlaces = new int[fSize];
            ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionCommon;
            BackActionMethod = BackActionByCommon;
        }
        //--------------------------------------------------------------------------------------
        public VerteciesInTableDefine(string pAlgorythmName)
        {
            algorythmName = pAlgorythmName;
        }
        //--------------------------------------------------------------------------------------
        public VerteciesInTableDefine(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, 
            int pSize, int pNumberOfPlace, string pAlgorythmName, int pMaxDurability)
            : this(pGraph, pMatrixofDistance, pSize, pNumberOfPlace, pMaxDurability)
        {
            //fGraph = pGraph;
            //fMatrixofDistance = pMatrixofDistance;
            //fOptimalRoute = new int[fNumberOfPlace];
            //optimalStorage = new Queue<Tuple<int[], int>>(15);
            //fOptimalWeight = int.MaxValue;
            //fOptimalPlaces = new int[fSize];

            algorythmName = pAlgorythmName;
            baseEnumeration = BaseEnumerationEnum.ByPlace;
        }
        //--------------------------------------------------------------------------------------
        public VerteciesInTableDefine(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance,
            int pSize, int pNumberOfPlace, 
            EmptyOrderEnum pEmptyOrder, BaseEnumerationEnum pBaseEnumeration,
            UsingLowestimationEnum pUsingLowestimation, EliminationEnum pElimination, int pMaxDurability)
            : this(pGraph, pMatrixofDistance, pSize, pNumberOfPlace, pMaxDurability)
        {
            //fGraph = pGraph;
            //fMatrixofDistance = pMatrixofDistance;
            //fOptimalRoute = new int[fNumberOfPlace];
            //optimalStorage = new Queue<Tuple<int[], int>>(15);
            //fOptimalWeight = int.MaxValue;
            //fOptimalPlaces = new int[fSize];
            
            algorythmName = (pEmptyOrder == EmptyOrderEnum.EmptyFirst ? "EOF_" : "EOL_") +
                            (pBaseEnumeration == BaseEnumerationEnum.ByPlace ? "BEP_" : "BEV_") +
                            (pUsingLowestimation == UsingLowestimationEnum.WithUseLowEstation ? "LEY_" : "LEN_") +
                            (pElimination == EliminationEnum.FullEnum ? "ETF" : (pElimination == EliminationEnum.StrictElim ? "ETS" : "ETR"));

            baseEnumeration = pBaseEnumeration;

            if (pEmptyOrder == EmptyOrderEnum.EmptyFirst)
                NextPointMethod = NextPointEmptyFirst;
            else
            {
                NextPointMethod = NextPointEmptyLast;
                startNumber = -1;
            }

            ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionCommon;
            BackActionMethod = BackActionByCommon;

            if (pBaseEnumeration == BaseEnumerationEnum.ByPlace)
            {
                fOptimalWeight = int.MaxValue;
                IsEliminableMethod = IsEliminableByPlace;
                if (pElimination == EliminationEnum.RughElim || pElimination == EliminationEnum.StrictElim)
                {
                    IsEliminableMethod = IsEliminableByPlaceAdv;
                    BackActionMethod = BackActionByPlaceAdv;
                    currentWeightOfRoute = 0;
                    weightOnStep = new int[fNumberOfPlace];
                    positionOfVertex = new Int32[fSize];

                    for (int i = 0; i < fSize; i++)
                        positionOfVertex[i] = -1;
                }
            }
            else if (pBaseEnumeration == BaseEnumerationEnum.ByVertex)
            {
                IsEliminableMethod = IsEliminableByVertex;
                if (pElimination == EliminationEnum.RughElim || pElimination == EliminationEnum.StrictElim)
                {
                    IsEliminableMethod = IsEliminableByVertexAdv;
                    BackActionMethod = BackActionByVertexAdv;
                    currentWeightOfRoute = 0;
                    weightOnStep = new int[fNumberOfPlace];
                    positionOfPlace = new Int32[fSize];

                    for (int i = 0; i < fSize; i++)
                        positionOfPlace[i] = -1;
                }
            }
            if (pUsingLowestimation == UsingLowestimationEnum.WithUseLowEstation)
            {
                if (pBaseEnumeration == BaseEnumerationEnum.ByPlace && pElimination == EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByPlaceLowEst;
                }
                else if (pBaseEnumeration == BaseEnumerationEnum.ByPlace && pElimination != EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByPlaceAdvLowEst;
                }
                else if (pBaseEnumeration == BaseEnumerationEnum.ByVertex && pElimination == EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByVertexLowEst;
                }
                else if (pBaseEnumeration == BaseEnumerationEnum.ByVertex && pElimination != EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByVertexAdvLowEst;
                }
            }
            if (IsEliminableMethod == null)
                throw new Exception("IsEliminableMethod was not defined");
        }
        //--------------------------------------------------------------------------------------
        public static VerteciesInTableDefine Create(string pAlgorythm, IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, int pMaxDurability)
        {
            if (pAlgorythm == "Union")
                return new VerteciesUnited(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "UnionParallel")
                return new VerteciesUnitedParallel(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            
            if (pAlgorythm == "Enumerate")
                return new VerteciesInTablePlaceEnumerate(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "EnumerateWithLowEstimate")
                return new CVerteciesInTableEnumerateWithLowEstimate(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "EnumerateAdvanced")
                return new VerteciesInTablePlaceEnumerateAdv(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "EnumerateAdvancedWithLowEstimate")
                return new CVerteciesInTableEnumerateAdvWithLowEstimate(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "ByVertexEnumerate")
                return new VerteciesInTableVertexEnumerate(pGraph, pMatrixofDistance, pAlgorythm,pMaxDurability);
            if (pAlgorythm == "ByVertexEnumerateWithLowEstimate")
                return new VerteciesInTableVertexEnumeratevWithLowEstimate(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "ByVertexEnumerateAdvanced")
                return new VerteciesInTableVertexEnumerate(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "ByVertexEnumerateAdvancedWithLowEstimate")
                return new VerteciesInTableVertexEnumerateAdvWithLowEstimate(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            if (pAlgorythm == "Approximate")
                return new VerticesInTableApproximate(pGraph, pMatrixofDistance, pAlgorythm, pMaxDurability);
            else
            {
                EmptyOrderEnum lEmptyOrder;
                BaseEnumerationEnum lBaseEnumeration;
                UsingLowestimationEnum lUsingLowestimation;
                EliminationEnum lElimination;
                if (pAlgorythm.Contains("EOF_"))
                    lEmptyOrder = EmptyOrderEnum.EmptyFirst;
                else if (pAlgorythm.Contains("EOL_"))
                    lEmptyOrder = EmptyOrderEnum.EmptyLast;
                else
                    throw new ArgumentException("can not define EmptyOrder.", "pAlgorythm");
                if (pAlgorythm.Contains("BEP_"))
                    lBaseEnumeration = BaseEnumerationEnum.ByPlace;
                else if (pAlgorythm.Contains("BEV_"))
                    lBaseEnumeration = BaseEnumerationEnum.ByVertex;
                else
                    throw new ArgumentException("can not define BaseEnumeration", "pAlgorythm");
                if (pAlgorythm.Contains("LEY_"))
                    lUsingLowestimation = UsingLowestimationEnum.WithUseLowEstation;
                else if (pAlgorythm.Contains("LEN_"))
                    lUsingLowestimation = UsingLowestimationEnum.NotUseLowEstimation;
                else
                    throw new ArgumentException("can not define BaseEnumeration", "pAlgorythm");
                if (pAlgorythm.Contains("ETF"))
                    lElimination = EliminationEnum.FullEnum;
                else if (pAlgorythm.Contains("ETS"))
                    lElimination = EliminationEnum.StrictElim;
                else if (pAlgorythm.Contains("ETR"))
                    lElimination = EliminationEnum.RughElim;
                else
                    throw new ArgumentException("can not define BaseEnumeration", "pAlgorythm");
                return Create(pGraph, pMatrixofDistance, lEmptyOrder, lBaseEnumeration, lUsingLowestimation, lElimination, pMaxDurability);
            }
        }
        //--------------------------------------------------------------------------------------
        public static VerteciesInTableDefine Create(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, 
            EmptyOrderEnum pEmptyOrder, BaseEnumerationEnum pBaseEnumeration,
            UsingLowestimationEnum pUsingLowestimation, EliminationEnum pElimination, int pMaxDurability)
        {
            if (pBaseEnumeration == BaseEnumerationEnum.ByPlace)
                return new VerteciesInTablePlace(pGraph, pMatrixofDistance, pEmptyOrder, pBaseEnumeration,
                    pUsingLowestimation, pElimination, pMaxDurability);
            else
                return new VerteciesInTableVertex(pGraph, pMatrixofDistance, pEmptyOrder, pBaseEnumeration,
                    pUsingLowestimation, pElimination, pMaxDurability);
            
        }
        //--------------------------------------------------------------------------------------
        public void DefineLowEstimate()
        {
            lowEstimate = 0;
            List<Tuple<int, int>> arrangeVertices = new List<Tuple<int, int>>();
            for(int i= 0; i < fGraph.Vertices.Count; i++)
                arrangeVertices.Add(Tuple.Create(i, fGraph.Vertices[i].Weight));
            arrangeVertices.Sort(new PairComparison());
            while (arrangeVertices.Count > 0 && arrangeVertices[0].Item2 > 4)
            {
                lowEstimate += arrangeVertices[0].Item2 - 4;
                for (int i = 1; i < arrangeVertices.Count; i++)
                    if (fGraph.Vertices[arrangeVertices[i].Item1].ContaintVertex(arrangeVertices[0].Item1))
                        arrangeVertices[i] = Tuple.Create(arrangeVertices[i].Item1, arrangeVertices[i].Item2 - 1);
                arrangeVertices.RemoveAt(0);
            }
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            return IsEliminableMethod();
        }
        //--------------------------------------------------------------------------------------
        public bool IsEliminableByPlace()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByPlace(fRoute);
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++)
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;
                    if (optimalStorage.Count >= 10)
                        optimalStorage.Dequeue();
                    optimalStorage.Enqueue(Tuple.Create(fOptimalRoute.ToArray(), fOptimalWeight));
                    changeOptimalNumber++;
                    ChangeOptimalWeightAction();
                    fUpdateOptcount++;
                }
                //                fResultStringSet.Add(RouteAsString);
                fCountTerminal++;

                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsEliminableByPlaceAdv()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfVertex[fRoute[lPosition] - 1] = lPosition;

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepVertex(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
            }
            //if (currentWeightOfRoute >= fOptimalWeight)
            //{
            //    fCountTerminal++;
            //    fElemenationCount++;
            //    return true;
            //}
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByPlace(fRoute);
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fRoute.Length; i++)
                    fOptimalRoute[i] = fRoute[i];
                fOptimalWeight = currentWeightOfRoute;
                if (optimalStorage.Count >= 10)
                    optimalStorage.Dequeue();
                optimalStorage.Enqueue(Tuple.Create(fOptimalRoute.ToArray(), fOptimalWeight));
                changeOptimalNumber++;
                ChangeOptimalWeightAction();
                fUpdateOptcount++;
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsEliminableByVertex()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByVertex();
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++)
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;
                    if (optimalStorage.Count >= 10)
                        optimalStorage.Dequeue();
                    CreateOptimalPlaces();
                    optimalStorage.Enqueue(Tuple.Create(fOptimalPlaces.ToArray(), fOptimalWeight));
                    changeOptimalNumber++;
                    ChangeOptimalWeightAction();
                    fUpdateOptcount++;
                }
                //                fResultStringSet.Add(RouteAsString);
                fCountTerminal++;
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public bool IsEliminableByVertexAdv()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfPlace[fRoute[lPosition] - 1] = lPosition;

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepPlace(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
            }
            //if (currentWeightOfRoute >= fOptimalWeight)
            //{
            //    fCountTerminal++;
            //fElemenationCount++;
            //    return true;
            //}
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByVertex();
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fOptimalPlaces.Length; i++)
                    fOptimalPlaces[i] = positionOfPlace[i] + 1;
                fOptimalWeight = currentWeightOfRoute;
                if (optimalStorage.Count >= 10)
                    optimalStorage.Dequeue();
                optimalStorage.Enqueue(Tuple.Create(fOptimalPlaces.ToArray(), fOptimalWeight));
                changeOptimalNumber++;
                ChangeOptimalWeightAction();
                fUpdateOptcount++;
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public int ComputeWeightByPlace(int[] pRoute)
        {
            int lWeight = 0;
            Int32[] lPositionOfVertex = new Int32[fSize];

            for (int i = 0; i < fNumberOfPlace; i++)
                if (pRoute[i] > 0)
                    lPositionOfVertex[pRoute[i] - 1] = i;

            for (int i = 0; i < fNumberOfPlace; i++)
            {
                if (pRoute[i] > 0)
                {
                    foreach (int indVertex in fGraph.Vertices[pRoute[i] - 1].EndPoints)
                    {
                        lWeight += fMatrixofDistance[i, lPositionOfVertex[indVertex]];
                    }
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int ComputeWeightByVertex()
        {
            int lWeight = 0;

            for (int i = 0; i < fGraph.Vertices.Count; i++)
            {
                IVertex vertex = fGraph.Vertices[i];
                foreach (int indVertex in vertex.EndPoints)
                {
                    lWeight += fMatrixofDistance[fRoute[i] - 1, fRoute[indVertex] - 1];
                }
            }


            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected void CreateOptimalPlaces()
        {
            for (int i = 0; i < fOptimalPlaces.Length; i++)
                fOptimalPlaces[i] = -1;
            for (int i = 0; i < fCurrentPosition; i++)
                fOptimalPlaces[fRoute[i] - 1] = i + 1;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepPlace(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                foreach (int indVertex in fGraph.Vertices[position].EndPoints)
                {
                    if (indVertex < position)
                        lWeight += fMatrixofDistance[fRoute[position] - 1, fRoute[indVertex] - 1];
                }
                foreach (int indVertex in fGraph.Vertices[position].StartPoints)
                {
                    if (indVertex < position)
                        lWeight += fMatrixofDistance[fRoute[position] - 1, fRoute[indVertex] - 1];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepVertex(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                foreach (int indVertex in fGraph.Vertices[fRoute[position] - 1].EndPoints)
                {
                    if (positionOfVertex[indVertex] >= 0)
                        lWeight += fMatrixofDistance[position, positionOfVertex[indVertex]];
                }
                foreach (int indVertex in fGraph.Vertices[fRoute[position] - 1].StartPoints)
                {
                    if (positionOfVertex[indVertex] >= 0)
                        lWeight += fMatrixofDistance[position, positionOfVertex[indVertex]];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected virtual void ChangeOptimalWeightAction()
        {
            ChangeOptimalWeightActionMethod();
        }
        //--------------------------------------------------------------------------------------
        protected void ChangeOptimalWeightActionCommon()
        {
        }
        //--------------------------------------------------------------------------------------
        protected void ChangeOptimalWeightActionByPlaceLowEst()
        {
            if (LowEstimate >= fOptimalWeight)
                fIsStop = true;
        }
        //--------------------------------------------------------------------------------------
        protected void ChangeOptimalWeightActionByPlaceAdvLowEst()
        {
            if (LowEstimate >= fOptimalWeight)
                fIsStop = true;
        }
        //--------------------------------------------------------------------------------------
        protected void ChangeOptimalWeightActionByVertexLowEst()
        {
            if (LowEstimate >= fOptimalWeight)
                fIsStop = true;
        }
        //--------------------------------------------------------------------------------------
        protected void ChangeOptimalWeightActionByVertexAdvLowEst()
        {
            if (LowEstimate >= fOptimalWeight)
                fIsStop = true;
        }
        //--------------------------------------------------------------------------------------
        protected override void BackAction(int pCurrentPosition)
        {
            BackActionMethod(pCurrentPosition);
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByCommon(int pCurrentPosition)
        {
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByVertexAdv(int pCurrentPosition)
        {
            positionOfPlace[fRoute[fCurrentPosition] - 1] = -1;
            currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByPlaceAdv(int pCurrentPosition)
        {
            if (fRoute[fCurrentPosition] > 0)
                positionOfVertex[fRoute[fCurrentPosition] - 1] = -1;
            currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        private class PairComparison : IComparer<Tuple<int, int>>
        {
            //--------------------------------------------------------------------------------------
            //public PairComparison()
            //{
            //}
            //--------------------------------------------------------------------------------------
            public int Compare(Tuple<int, int> x, Tuple<int, int> y)
            {
                if (x.Item2 > y.Item2)
                    return -1;
                if (y.Item2 > x.Item2)
                    return 1;
                return 0;
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
