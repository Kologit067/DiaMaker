#define TESTCURRENTWEIGHT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;
using CommonLib;
using ArrayExtensions;
using CommonLib.Tools;

namespace OptimalPositionLib
{
    // EmptyFirst/EmptyLast
    // ByVertex/ByPlace
    // WithUseLowEst/NotUseLowEst
    // FullEnum/StructElim/RughElim
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableDefine
    //--------------------------------------------------------------------------------------
    public partial class VerteciesInTableDefine : PermitationBase
    {
        protected IGraph<IVertex> fGraph;
        protected IMatrixOfDistance fMatrixofDistance;
        protected int[] fOptimalRoute = null;				// маршрут обхода
        protected Queue<Tuple<int[], int>> optimalStorage;
        protected int changeOptimalNumber;
        protected int fOptimalWeight = 0;
        protected int lowEstimate = 0;
        protected string algorithmName;
        protected int[] fOptimalPlaces;
        protected int currentWeightOfRoute;
        protected int[] weightOnStep;
        protected int currentCountOfEdges;
        protected int currentCountOfVertices;
        protected int[] countOfEdgesOnStep;
        protected Int32[] positionOfVertex;
        protected Int32[] positionOfPlace;
        protected Int32[] sortedVertices;
        protected Int32[] sortedPlaces;
        protected Int32[] sortedVerticesInverse;
        protected Int32[] sortedPlacesInverse;
//        protected BaseEnumerationEnum baseEnumeration;
        protected AlgorithmConfiguration algorithmConfiguration;
        private Func<bool> IsEliminableMethod;
        private Action ChangeOptimalWeightActionMethod;
        private Action<int> BackActionMethod;
        private List<AlgorithmOptimalValueChange> algorithmOptimalValueChanges = new List<AlgorithmOptimalValueChange>();
//        private bool isComplete;
        //--------------------------------------------------------------------------------------
        public virtual int[] OptimalRoute
        {
            get
            {
                if (algorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByPlace)
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
        public string AlgorithmName
        {
            get
            {
                return algorithmName;
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
        public List<AlgorithmOptimalValueChange> AlgorithmOptimalValueChanges
        {
            get
            {
                return algorithmOptimalValueChanges;
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

            sortedVertices = Enumerable.Range(0, pGraph.Vertices.Count).ToArray();
            sortedPlaces = Enumerable.Range(0, pMatrixofDistance.Length).ToArray();

            sortedVerticesInverse = sortedVertices.InversePermitation();
            sortedPlacesInverse = sortedPlaces.InversePermitation();

            algorithmConfiguration = new AlgorithmConfiguration();

        }

        //--------------------------------------------------------------------------------------
        public VerteciesInTableDefine(string pAlgorithmName)
        {
            algorithmName = pAlgorithmName;
        }
        //--------------------------------------------------------------------------------------
        public VerteciesInTableDefine(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, 
            int pSize, int pNumberOfPlace, string pAlgorithmName, int pMaxDurability)
            : this(pGraph, pMatrixofDistance, pSize, pNumberOfPlace, pMaxDurability)
        {
            //fGraph = pGraph;
            //fMatrixofDistance = pMatrixofDistance;
            //fOptimalRoute = new int[fNumberOfPlace];
            //optimalStorage = new Queue<Tuple<int[], int>>(15);
            //fOptimalWeight = int.MaxValue;
            //fOptimalPlaces = new int[fSize];

            algorithmName = pAlgorithmName;
            algorithmConfiguration.BaseEnumeration = BaseEnumerationEnum.ByPlace;
        }
        //--------------------------------------------------------------------------------------
        public VerteciesInTableDefine(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance,
            int pSize, int pNumberOfPlace, AlgorithmConfiguration pAlgorithmConfiguration, int pMaxDurability)
            : this(pGraph, pMatrixofDistance, pSize, pNumberOfPlace, pMaxDurability)
        {
            //fGraph = pGraph;
            //fMatrixofDistance = pMatrixofDistance;
            //fOptimalRoute = new int[fNumberOfPlace];
            //optimalStorage = new Queue<Tuple<int[], int>>(15);
            //fOptimalWeight = int.MaxValue;
            //fOptimalPlaces = new int[fSize];

            algorithmConfiguration = pAlgorithmConfiguration;
            algorithmName = algorithmConfiguration.AlgorithmName;
                //(pEmptyOrder == EmptyOrderEnum.EmptyFirst ? "EOF_" : "EOL_") +
                //            (pBaseEnumeration == BaseEnumerationEnum.ByPlace ? "BEP_" : "BEV_") +
                //            (pUsingLowestimation == UsingLowestimationEnum.WithUseLowEstation ? "LEY_" : "LEN_") +
                //            (pElimination == EliminationEnum.FullEnum ? "ETF" : (pElimination == EliminationEnum.StrictElim ? "ETS" : "ETR")) +
                //            (pIsSorted == IsSortedEnum.IsSortedNo ? "ISN" : "ISY") +
                //            (pSortedType == SortedTypeEnum.SortedNone ? "STN" : (pSortedType == SortedTypeEnum.SortedBoth ? "STB" : (pSortedType == SortedTypeEnum.SortedPlace ? "STP" : "STV")));

//            baseEnumeration = pBaseEnumeration;

            if (algorithmConfiguration.SortedType == SortedTypeEnum.SortedBoth || algorithmConfiguration.SortedType == SortedTypeEnum.SortedVertex)
            {
                sortedVertices = sortedVertices.OrderByDescending(v => pGraph.Vertices[v].Weight).ToArray();
                sortedVerticesInverse = sortedVertices.InversePermitation();
            }

            if (algorithmConfiguration.SortedType == SortedTypeEnum.SortedBoth || algorithmConfiguration.SortedType == SortedTypeEnum.SortedPlace)
            {
                sortedPlaces = sortedPlaces.OrderByDescending(v => VerticesInTableApproximate.WeightOfPlacePosition(v, fMatrixofDistance)).ToArray();
                sortedPlacesInverse = sortedPlaces.InversePermitation();
            }

            if (algorithmConfiguration.EmptyOrder == EmptyOrderEnum.EmptyFirst)
                NextPointMethod = NextPointEmptyFirst;
            else
            {
                NextPointMethod = NextPointEmptyLast;
                startNumber = -1;
            }

            ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionCommon;
            BackActionMethod = BackActionByCommon;

            if (algorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByPlace)
            {
                fOptimalWeight = int.MaxValue;
                if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo)
                    IsEliminableMethod = IsEliminableByPlace;
                else
                    IsEliminableMethod = IsEliminableByPlaceSorted;

                if (algorithmConfiguration.Elimination == EliminationEnum.RughElim || algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                {
                    if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo && algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        IsEliminableMethod = IsEliminableByPlaceAdv;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedYes && algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        IsEliminableMethod = IsEliminableByPlaceAdvSorted;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo && algorithmConfiguration.Elimination == EliminationEnum.RughElim)
                        IsEliminableMethod = IsEliminableByPlaceForced;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedYes && algorithmConfiguration.Elimination == EliminationEnum.RughElim)
                        IsEliminableMethod = IsEliminableByPlaceForcedSorted;
                    else
                        throw new ArgumentException("incorrect configuration input parameters. can not define IsEliminableMethod method.");

                    if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo || algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        BackActionMethod = BackActionByPlaceAdv;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedYes || algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        BackActionMethod = BackActionByPlaceAdvSorted;
                    if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo || algorithmConfiguration.Elimination == EliminationEnum.RughElim)
                        BackActionMethod = BackActionByPlaceForced;
                    else 
                        BackActionMethod = BackActionByPlaceForcedSorted;

                    currentWeightOfRoute = 0;
                    weightOnStep = new int[fNumberOfPlace];
                    countOfEdgesOnStep = new int[fNumberOfPlace];
                    positionOfVertex = new Int32[fSize];

                    for (int i = 0; i < fSize; i++)
                        positionOfVertex[i] = -1;
                }
            }
            else if (algorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByVertex)
            {
                if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo)
                    IsEliminableMethod = IsEliminableByVertex;
                else
                    IsEliminableMethod = IsEliminableByVertexSorted;
                if (algorithmConfiguration.Elimination == EliminationEnum.RughElim || algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                {
                    if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo && algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        IsEliminableMethod = IsEliminableByVertexAdv;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedYes && algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        IsEliminableMethod = IsEliminableByVertexAdvSorted;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo && algorithmConfiguration.Elimination == EliminationEnum.RughElim)
                        IsEliminableMethod = IsEliminableByVertexForced;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedYes && algorithmConfiguration.Elimination == EliminationEnum.RughElim)
                        IsEliminableMethod = IsEliminableByVertexForcedSorted;
                    else
                        throw new ArgumentException("incorrect configuration input parameters. can not define IsEliminableMethod method.");

                    if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo || algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        BackActionMethod = BackActionByVertexAdv;
                    else if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedYes || algorithmConfiguration.Elimination == EliminationEnum.StrictElim)
                        BackActionMethod = BackActionByVertexForcedSorted;
                    if (algorithmConfiguration.IsSorted == IsSortedEnum.IsSortedNo || algorithmConfiguration.Elimination == EliminationEnum.RughElim)
                        BackActionMethod = BackActionByVertexAdv;
                    else
                        BackActionMethod = BackActionByVertexForcedSorted;

                    currentWeightOfRoute = 0;
                    weightOnStep = new int[fNumberOfPlace];
                    countOfEdgesOnStep = new int[fNumberOfPlace];
                    positionOfPlace = new Int32[fSize];

                    for (int i = 0; i < fSize; i++)
                        positionOfPlace[i] = -1;
                }
            }
            if (algorithmConfiguration.UsingLowestimation == UsingLowestimationEnum.WithUseLowEstation)
            {
                if (algorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByPlace && algorithmConfiguration.Elimination == EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByPlaceLowEst;
                }
                else if (algorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByPlace && algorithmConfiguration.Elimination != EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByPlaceAdvLowEst;
                }
                else if (algorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByVertex && algorithmConfiguration.Elimination == EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByVertexLowEst;
                }
                else if (algorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByVertex && algorithmConfiguration.Elimination != EliminationEnum.FullEnum)
                {
                    ChangeOptimalWeightActionMethod = ChangeOptimalWeightActionByVertexAdvLowEst;
                }
            }
            if (IsEliminableMethod == null)
                throw new Exception("IsEliminableMethod was not defined");
        }
        //--------------------------------------------------------------------------------------
        public static VerteciesInTableDefine Create(string pAlgorithm, IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, int pMaxDurability)
        {
            if (pAlgorithm == "Union")
                return new VerteciesUnited(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "UnionParallel")
                return new VerteciesUnitedParallel(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);

            if (pAlgorithm == "Enumerate")
                return new VerteciesInTablePlaceEnumerate(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "EnumerateSorted")
                return new CVerteciesInTableEnumerateWithLowSorted(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "EnumerateSortedPlace")
                return new CVerteciesInTableEnumerateWithLowSortedPlace(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "EnumerateWithLowEstimate")
                return new CVerteciesInTableEnumerateWithLowEstimate(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "EnumerateAdvanced")
                return new VerteciesInTablePlaceEnumerateAdv(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "EnumerateAdvancedSorted")
                return new VerteciesInTablePlaceEnumerateAdvSorted(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "EnumerateAdvancedSortedPlace")
                return new VerteciesInTablePlaceEnumerateAdvSortedPlace(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "EnumerateAdvancedWithLowEstimate")
                return new CVerteciesInTableEnumerateAdvWithLowEstimate(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerate")
                return new VerteciesInTableVertexEnumerate(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerateSorted")
                return new VerteciesInTableVertexEnumerateSorted(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerateSortedVertex")
                return new VerteciesInTableVertexEnumerateSortedVertex(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerateWithLowEstimate")
                return new VerteciesInTableVertexEnumeratevWithLowEstimate(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerateAdvanced")
                return new VerteciesInTableVertexEnumerateAdv(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerateAdvancedSorted")
                return new VerteciesInTableVertexEnumerateAdvSorted(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerateAdvancedSortedVertex")
                return new VerteciesInTableVertexEnumerateAdvSortedVertex(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "ByVertexEnumerateAdvancedWithLowEstimate")
                return new VerteciesInTableVertexEnumerateAdvWithLowEstimate(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            if (pAlgorithm == "Approximate")
                return new VerticesInTableApproximate(pGraph, pMatrixofDistance, pAlgorithm, pMaxDurability);
            else
            {
                //EmptyOrderEnum lEmptyOrder;
                //BaseEnumerationEnum lBaseEnumeration;
                //UsingLowestimationEnum lUsingLowestimation;
                //EliminationEnum lElimination;
                //if (pAlgorithm.Contains("EOF_"))
                //    lEmptyOrder = EmptyOrderEnum.EmptyFirst;
                //else if (pAlgorithm.Contains("EOL_"))
                //    lEmptyOrder = EmptyOrderEnum.EmptyLast;
                //else
                //    throw new ArgumentException("can not define EmptyOrder.", "pAlgorithm");
                //if (pAlgorithm.Contains("BEP_"))
                //    lBaseEnumeration = BaseEnumerationEnum.ByPlace;
                //else if (pAlgorithm.Contains("BEV_"))
                //    lBaseEnumeration = BaseEnumerationEnum.ByVertex;
                //else
                //    throw new ArgumentException("can not define BaseEnumeration", "pAlgorithm");
                //if (pAlgorithm.Contains("LEY_"))
                //    lUsingLowestimation = UsingLowestimationEnum.WithUseLowEstation;
                //else if (pAlgorithm.Contains("LEN_"))
                //    lUsingLowestimation = UsingLowestimationEnum.NotUseLowEstimation;
                //else
                //    throw new ArgumentException("can not define BaseEnumeration", "pAlgorithm");
                //if (pAlgorithm.Contains("ETF"))
                //    lElimination = EliminationEnum.FullEnum;
                //else if (pAlgorithm.Contains("ETS"))
                //    lElimination = EliminationEnum.StrictElim;
                //else if (pAlgorithm.Contains("ETR"))
                //    lElimination = EliminationEnum.RughElim;
                //else
                //    throw new ArgumentException("can not define BaseEnumeration", "pAlgorithm");

                AlgorithmConfiguration config = AlgorithmConfiguration.CreateFromName(pAlgorithm);
                return Create(pGraph, pMatrixofDistance, config, pMaxDurability);
            }
        }
        //--------------------------------------------------------------------------------------
        public static VerteciesInTableDefine Create(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance,
            AlgorithmConfiguration pAlgorithmConfiguration, int pMaxDurability)
        {
            if (pAlgorithmConfiguration.BaseEnumeration == BaseEnumerationEnum.ByPlace)
                return new VerteciesInTablePlace(pGraph, pMatrixofDistance, pAlgorithmConfiguration, pMaxDurability);
            else
                return new VerteciesInTableVertex(pGraph, pMatrixofDistance, pAlgorithmConfiguration, pMaxDurability);
            
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

                    AddOptimalStorage();
                    AddOptimalValueChangeData();

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
        public bool IsEliminableByPlaceSortedPlace()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByPlaceSortedPlace(fRoute);
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++)
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;

                    fOptimalRoute = sortedPlacesInverse.Select(i => fOptimalRoute[i]).ToArray();

                    AddOptimalStorage();
                    AddOptimalValueChangeData();

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
        public bool IsEliminableByPlaceSorted()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByPlaceSorted(fRoute);
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++)
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;
                    fOptimalRoute = sortedPlacesInverse.Select(i => fOptimalRoute[i] == 0 ? fOptimalRoute[i] : (sortedVertices[fOptimalRoute[i]-1] + 1)).ToArray();
#if TESTCURRENTWEIGHT
                    int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalRoute);
                    if (lWeightOfRoute != lNSWeightOfRoute)
                        throw new Exception("Logical error in IsEliminableByPlaceSorted. Incorrect Optimal Route.");
#endif                    
                    AddOptimalStorage();
                    AddOptimalValueChangeData();

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
#if INCLUDEELIMINATION
            if (currentWeightOfRoute >= fOptimalWeight)
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
#endif
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByPlace(fRoute);
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fRoute.Length; i++)
                    fOptimalRoute[i] = fRoute[i];
                fOptimalWeight = currentWeightOfRoute;

                AddOptimalStorage();
                AddOptimalValueChangeData();

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
        public bool IsEliminableByPlaceAdvSortedPlace()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfVertex[fRoute[lPosition] - 1] = lPosition;

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepVertexSortedPlace(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
            }
#if INCLUDEELIMINATION
            if (currentWeightOfRoute >= fOptimalWeight)
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
#endif
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByPlaceSortedPlace(fRoute);
#if TESTCURRENTWEIGHT
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fRoute.Length; i++)
                    fOptimalRoute[i] = fRoute[i];
                fOptimalWeight = currentWeightOfRoute;
                fOptimalRoute = sortedPlacesInverse.Select(i => fOptimalRoute[i]).ToArray();
                int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalRoute);
#if TESTCURRENTWEIGHT
                if (lWeightOfRoute != lNSWeightOfRoute)
                    throw new Exception("Logical error in IsEliminableByPlaceSorted. Incorrect Optimal Route.");
#endif
                AddOptimalStorage();
                AddOptimalValueChangeData();
                
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
        public bool IsEliminableByPlaceAdvSorted()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfVertex[sortedVertices[fRoute[lPosition] - 1]] = lPosition;

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepVertexSorted(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
            }
#if INCLUDEELIMINATION
            if (currentWeightOfRoute >= fOptimalWeight)
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
#endif
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByPlaceSorted(fRoute);
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fRoute.Length; i++)
                    fOptimalRoute[i] = fRoute[i];
                fOptimalWeight = currentWeightOfRoute;
                fOptimalRoute = sortedPlacesInverse.Select(i => fOptimalRoute[i] == 0 ? fOptimalRoute[i] : (sortedVertices[fOptimalRoute[i] - 1] + 1)).ToArray();
#if TESTCURRENTWEIGHT
                int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalRoute);
                if (lWeightOfRoute != lNSWeightOfRoute)
                    throw new Exception("Logical error in IsEliminableByPlaceSorted. Incorrect Optimal Route.");
#endif
                AddOptimalStorage();
                AddOptimalValueChangeData();
                
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
        public bool IsEliminableByPlaceForced()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfVertex[fRoute[lPosition] - 1] = lPosition;

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepVertex(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
                countOfEdgesOnStep[lPosition] = CountOfEdgesOfStepVertex(lPosition);
                currentCountOfEdges += countOfEdgesOnStep[lPosition];
                currentCountOfVertices += (fRoute[lPosition] > 0) ? 1 : 0;
            }
            if (currentWeightOfRoute >= fOptimalWeight ||
                Limiter(lPosition, fSize, currentCountOfEdges, countOfEdgesOnStep[lPosition],
                        currentWeightOfRoute, weightOnStep[lPosition])
                //                (double)currentWeightOfRoute / (double)currentCountOfEdges > Limiter(currentCountOfVertices, currentCountOfEdges, fSize)
                )
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByPlace(fRoute);
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fRoute.Length; i++)
                    fOptimalRoute[i] = fRoute[i];
                fOptimalWeight = currentWeightOfRoute;

                AddOptimalStorage();
                AddOptimalValueChangeData();

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
        public bool IsEliminableByPlaceForcedSorted()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfVertex[sortedVertices[fRoute[lPosition] - 1]] = lPosition;

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepVertexSorted(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
                countOfEdgesOnStep[lPosition] = CountOfEdgesOfStepVertexSorted(lPosition);
                currentCountOfEdges += countOfEdgesOnStep[lPosition];
                currentCountOfVertices += (fRoute[lPosition] > 0) ? 1 : 0;
            }
            if (currentWeightOfRoute >= fOptimalWeight ||
                Limiter(lPosition, fSize, currentCountOfEdges, countOfEdgesOnStep[lPosition],
                        currentWeightOfRoute, weightOnStep[lPosition])
//                (double)currentWeightOfRoute / (double)currentCountOfEdges > Limiter(currentCountOfVertices, currentCountOfEdges, fSize)
                )
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByPlaceSorted(fRoute);
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;

                for (int i = 0; i < fRoute.Length; i++)
                    fOptimalRoute[i] = fRoute[i];
                fOptimalWeight = currentWeightOfRoute;
                fOptimalRoute = sortedPlacesInverse.Select(i => fOptimalRoute[i] == 0 ? fOptimalRoute[i] : (sortedVertices[fOptimalRoute[i] - 1] + 1)).ToArray();
#if TESTCURRENTWEIGHT
                int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalRoute);
                if (lWeightOfRoute != lNSWeightOfRoute)
                    throw new Exception("Logical error in IsEliminableByPlaceSorted. Incorrect Optimal Route.");
#endif
                AddOptimalStorage();
                AddOptimalValueChangeData();

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
                    CreateOptimalPlaces();
                    Parallel.For(0, fOptimalPlaces.Length, i => { if (fOptimalPlaces[i] == -1) fOptimalPlaces[i] = 0; });

                    AddOptimalStorage(fOptimalPlaces.ToArray());
                    AddOptimalValueChangeData();

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
        public bool IsEliminableByVertexSortedVertex()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByVertexSortedVertex();
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++)
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;
                    CreateOptimalPlacesSortedVertex();
#if TESTCURRENTWEIGHT
                    int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalPlaces, fSize, fNumberOfPlace);
                    if (lWeightOfRoute != lNSWeightOfRoute)
                    {
                        lNSWeightOfRoute = ComputeWeightByPlace(fOptimalPlaces, fSize, fNumberOfPlace);
                        throw new Exception("Logical error in IsEliminableByVertexSortedVertex. Incorrect Optimal Route.");
                    }
#endif

                    AddOptimalStorage(fOptimalPlaces.ToArray());
                    AddOptimalValueChangeData();

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
        public bool IsEliminableByVertexSorted()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByVertexSorted();
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++)
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;
                    CreateOptimalPlacesSorted();
#if TESTCURRENTWEIGHT
                    int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalPlaces, fSize, fNumberOfPlace);
                    if (lWeightOfRoute != lNSWeightOfRoute)
                        throw new Exception("Logical error in IsEliminableByVertexSorted. Incorrect Optimal Route.");
#endif
                    AddOptimalStorage(fOptimalPlaces.ToArray());
                    AddOptimalValueChangeData();

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
#if INCLUDEELIMINATION
            if (currentWeightOfRoute >= fOptimalWeight)
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
#endif
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByVertex();
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fOptimalPlaces.Length; i++)
                    fOptimalPlaces[i] = positionOfPlace[i] + 1;
                fOptimalWeight = currentWeightOfRoute;
                
                AddOptimalStorage(fOptimalPlaces.ToArray());
                AddOptimalValueChangeData();

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
        public bool IsEliminableByVertexAdvSortedVertex()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfPlace[fRoute[lPosition] - 1] = sortedVertices[lPosition];

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepPlaceSortedVertex(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
            }
#if INCLUDEELIMINATION
            if (currentWeightOfRoute >= fOptimalWeight)
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
#endif
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByVertexSortedVertex();
                if (lWeightOfRoute != currentWeightOfRoute)
                {
                    lWeightOfRoute = ComputeWeightByVertexSortedVertex();
                    weightOnStep[lPosition] = WeightOfStepPlaceSortedVertex(lPosition);
                    weightOnStep[lPosition] = WeightOfStepPlaceSortedVertex(lPosition); throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
                }
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fOptimalPlaces.Length; i++)
                    fOptimalPlaces[i] = positionOfPlace[i] + 1;
                fOptimalWeight = currentWeightOfRoute;
                int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalPlaces, fSize, fNumberOfPlace);
#if TESTCURRENTWEIGHT
                if (lWeightOfRoute != lNSWeightOfRoute)
                    throw new Exception("Logical error in IsEliminableByVertexAdvSortedVertex. Incorrect Optimal Route.");
#endif
                AddOptimalStorage(fOptimalPlaces.ToArray());
                AddOptimalValueChangeData();

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
        public bool IsEliminableByVertexAdvSorted()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfPlace[sortedPlaces[fRoute[lPosition] - 1]] = sortedVertices[lPosition];

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepPlaceSorted(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
            }
#if INCLUDEELIMINATION
            if (currentWeightOfRoute >= fOptimalWeight)
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
#endif
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByVertexSorted();
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fOptimalPlaces.Length; i++)
                    fOptimalPlaces[i] = positionOfPlace[i] + 1;
                fOptimalWeight = currentWeightOfRoute;
#if TESTCURRENTWEIGHT
                int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalPlaces, fSize, fNumberOfPlace);
                if (lWeightOfRoute != lNSWeightOfRoute)
                    throw new Exception("Logical error in IsEliminableByVertexAdvSorted. Incorrect Optimal Route.");
#endif
                AddOptimalStorage(fOptimalPlaces.ToArray());
                AddOptimalValueChangeData();
                
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
        public bool IsEliminableByVertexForced()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfPlace[fRoute[lPosition] - 1] = lPosition;

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepPlace(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
                countOfEdgesOnStep[lPosition] = CountOfEdgesOfStepPlace(lPosition);
                currentCountOfEdges += countOfEdgesOnStep[lPosition];
            }
            if (currentWeightOfRoute >= fOptimalWeight ||
                Limiter( lPosition,  fNumberOfPlace, currentCountOfEdges,  countOfEdgesOnStep[lPosition], 
                        currentWeightOfRoute,  weightOnStep[lPosition])
//                (double)currentWeightOfRoute / (double)currentCountOfEdges > Limiter(lPosition, currentCountOfEdges, fNumberOfPlace)
            )
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByVertex();
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fOptimalPlaces.Length; i++)
                    fOptimalPlaces[i] = positionOfPlace[i] + 1;
                fOptimalWeight = currentWeightOfRoute;

                AddOptimalStorage(fOptimalPlaces.ToArray());
                AddOptimalValueChangeData();

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
        public bool IsEliminableByVertexForcedSorted()
        {
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfPlace[sortedPlaces[fRoute[lPosition] - 1]] = sortedVertices[lPosition];

            if (lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepPlaceSorted(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
                countOfEdgesOnStep[lPosition] = CountOfEdgesOfStepPlaceSorted(lPosition);
                currentCountOfEdges += countOfEdgesOnStep[lPosition];
            }
            if (currentWeightOfRoute >= fOptimalWeight ||
                Limiter( lPosition,  fNumberOfPlace, currentCountOfEdges,  countOfEdgesOnStep[lPosition], 
                        currentWeightOfRoute,  weightOnStep[lPosition])
//                (double)currentWeightOfRoute / (double)currentCountOfEdges > Limiter(lPosition, currentCountOfEdges, fNumberOfPlace)
                )
            {
                fCountTerminal++;
                fElemenationCount++;
                return true;
            }
            if (fCurrentPosition == fNumberOfPlace)
            {
#if TESTCURRENTWEIGHT
                int lWeightOfRoute = ComputeWeightByVertexSorted();
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
#endif
                fCountTerminal++;
                if (currentWeightOfRoute >= fOptimalWeight)
                    return true;
                for (int i = 0; i < fOptimalPlaces.Length; i++)
                    fOptimalPlaces[i] = positionOfPlace[i] + 1;
                fOptimalWeight = currentWeightOfRoute;
#if TESTCURRENTWEIGHT
                int lNSWeightOfRoute = ComputeWeightByPlace(fOptimalPlaces, fSize, fNumberOfPlace);
                if (lWeightOfRoute != lNSWeightOfRoute)
                    throw new Exception("Logical error in IsEliminableByVertexAdvSorted. Incorrect Optimal Route.");
#endif
                AddOptimalStorage(fOptimalPlaces.ToArray());
                AddOptimalValueChangeData();

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
            return ComputeWeightByPlace(pRoute, fNumberOfPlace, fSize);
        }
        //--------------------------------------------------------------------------------------
        public int ComputeWeightByPlace(int[] pRoute, int pNumberOfPlace, int pSize)
        {
            int lWeight = 0;
            Int32[] lPositionOfVertex = new Int32[pSize];

            for (int i = 0; i < pNumberOfPlace; i++)
                if (pRoute[i] > 0)
                    lPositionOfVertex[pRoute[i] - 1] = i;

            for (int i = 0; i < pNumberOfPlace; i++)
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
        public int ComputeWeightByPlaceSortedPlace(int[] pRoute)
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
                        lWeight += fMatrixofDistance[sortedPlaces[i], sortedPlaces[lPositionOfVertex[indVertex]]];
                    }
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        public int ComputeWeightByPlaceSorted(int[] pRoute)
        {
            int lWeight = 0;
            Int32[] lPositionOfVertex = new Int32[fSize];

            for (int i = 0; i < fNumberOfPlace; i++)
                if (pRoute[i] > 0)
                    lPositionOfVertex[sortedVertices[pRoute[i] - 1]] = i;

            for (int i = 0; i < fNumberOfPlace; i++)
            {
                if (pRoute[i] > 0)
                {
                    foreach (int indVertex in fGraph.Vertices[sortedVertices[pRoute[i] - 1]].EndPoints)
                    {
                        lWeight += fMatrixofDistance[sortedPlaces[i], sortedPlaces[lPositionOfVertex[indVertex]]];
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
        protected int ComputeWeightByVertexSortedVertex()
        {
            int lWeight = 0;

            for (int i = 0; i < fGraph.Vertices.Count; i++)
            {
                IVertex vertex = fGraph.Vertices[sortedVertices[i]];
                foreach (int indVertex in vertex.EndPoints)
                {
                    lWeight += fMatrixofDistance[fRoute[i] - 1, fRoute[sortedVerticesInverse[indVertex]] - 1];
                }
            }


            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int ComputeWeightByVertexSorted()
        {
            int lWeight = 0;

            for (int i = 0; i < fGraph.Vertices.Count; i++)
            {
                IVertex vertex = fGraph.Vertices[sortedVertices[i]];
                foreach (int indVertex in vertex.EndPoints)
                {
                    lWeight += fMatrixofDistance[sortedPlaces[fRoute[i] - 1], sortedPlaces[fRoute[sortedVerticesInverse[indVertex]] - 1]];
                }
            }


            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected void CreateOptimalPlaces()
        {
            for (int i = 0; i < fOptimalPlaces.Length; i++)
                fOptimalPlaces[i] = 0;
            for (int i = 0; i < fCurrentPosition; i++)
                fOptimalPlaces[fRoute[i] - 1] = i + 1;
        }
        //--------------------------------------------------------------------------------------
        protected void CreateOptimalPlacesSortedVertex()
        {
            for (int i = 0; i < fOptimalPlaces.Length; i++)
                fOptimalPlaces[i] = 0;
            for (int i = 0; i < fCurrentPosition; i++)
                fOptimalPlaces[fRoute[i] - 1] = sortedVertices[i] + 1;
        }
        //--------------------------------------------------------------------------------------
        protected void CreateOptimalPlacesSorted()
        {
            for (int i = 0; i < fOptimalPlaces.Length; i++)
                fOptimalPlaces[i] = 0;
            for (int i = 0; i < fCurrentPosition; i++)
                fOptimalPlaces[sortedPlaces[fRoute[i] - 1]] = sortedVertices[i] + 1;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepPlace(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                foreach (int indVertex in fGraph.Vertices[position].AdjacentVertices)
                {
                    if (indVertex < position)
                        lWeight += fMatrixofDistance[fRoute[position] - 1, fRoute[indVertex] - 1];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepPlaceSortedVertex(int position)
        {
            int lWeight = 0;
            if (position >= 0 && fRoute[position] > 0)
            {
                IVertex vertex = fGraph.Vertices[sortedVertices[position]];
                foreach (int indVertex in vertex.AdjacentVertices)
                {
                    if (sortedVerticesInverse[indVertex] < position)
                        lWeight += fMatrixofDistance[fRoute[position] - 1, fRoute[sortedVerticesInverse[indVertex]] - 1];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepPlaceSorted(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                IVertex vertex = fGraph.Vertices[sortedVertices[position]];
                foreach (int indVertex in vertex.AdjacentVertices)
                {
                    if (sortedVerticesInverse[indVertex] < position)
                        lWeight += fMatrixofDistance[sortedPlaces[fRoute[position] - 1], sortedPlaces[fRoute[sortedVerticesInverse[indVertex]] - 1]];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int CountOfEdgesOfStepPlace(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                lWeight = fGraph.Vertices[position].AdjacentVertices.Where(indVertex => indVertex < position).Count();
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int CountOfEdgesOfStepPlaceSorted(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                lWeight = fGraph.Vertices[sortedVertices[position]].AdjacentVertices.Where(indVertex => sortedVerticesInverse[indVertex] < position).Count();
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepVertex(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                foreach (int indVertex in fGraph.Vertices[fRoute[position] - 1].AdjacentVertices)
                {
                    if (positionOfVertex[indVertex] >= 0)
                        lWeight += fMatrixofDistance[position, positionOfVertex[indVertex]];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepVertexSortedPlace(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                foreach (int indVertex in fGraph.Vertices[fRoute[position] - 1].AdjacentVertices)
                {
                    if (positionOfVertex[indVertex] >= 0)
                        lWeight += fMatrixofDistance[sortedPlaces[position], sortedPlaces[positionOfVertex[indVertex]]];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int WeightOfStepVertexSorted(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                foreach (int indVertex in fGraph.Vertices[sortedVertices[fRoute[position] - 1]].AdjacentVertices)
                {
                    if (positionOfVertex[indVertex] >= 0)
                        lWeight += fMatrixofDistance[sortedPlaces[position], sortedPlaces[positionOfVertex[indVertex]]];
                }
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int CountOfEdgesOfStepVertex(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                lWeight = fGraph.Vertices[fRoute[position] - 1].AdjacentVertices.Where(indVertex => positionOfVertex[indVertex] >= 0).Count();
            }
            return lWeight;
        }
        //--------------------------------------------------------------------------------------
        protected int CountOfEdgesOfStepVertexSorted(int position)
        {
            int lWeight = 0;

            if (position >= 0 && fRoute[position] > 0)
            {
                lWeight = fGraph.Vertices[sortedVertices[fRoute[position] - 1]].AdjacentVertices.Where(indVertex => positionOfVertex[indVertex] >= 0).Count();
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
        protected void BackActionByVertexAdvSortedVertex(int pCurrentPosition)
        {
            positionOfPlace[fRoute[fCurrentPosition] - 1] = -1;
            currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByVertexAdvSorted(int pCurrentPosition)
        {
            positionOfPlace[sortedPlaces[fRoute[fCurrentPosition] - 1]] = -1;
            currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByVertexForsed(int pCurrentPosition)
        {
            BackActionByVertexAdv(pCurrentPosition);
            currentCountOfEdges -= countOfEdgesOnStep[pCurrentPosition];
            countOfEdgesOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByVertexForcedSorted(int pCurrentPosition)
        {
            BackActionByVertexAdvSorted(pCurrentPosition);
            currentCountOfEdges -= countOfEdgesOnStep[pCurrentPosition];
            countOfEdgesOnStep[pCurrentPosition] = 0;
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
        protected void BackActionByPlaceAdvSortedPlace(int pCurrentPosition)
        {
            if (fRoute[fCurrentPosition] > 0)
                positionOfVertex[fRoute[fCurrentPosition] - 1] = -1;
            currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByPlaceAdvSorted(int pCurrentPosition)
        {
            if (fRoute[fCurrentPosition] > 0)
                positionOfVertex[sortedVertices[fRoute[fCurrentPosition] - 1]] = -1;
            currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByPlaceForced(int pCurrentPosition)
        {
            currentCountOfVertices -= (fRoute[fCurrentPosition] > 0) ? 1 : 0;
            BackActionByPlaceAdv(pCurrentPosition);
            currentCountOfEdges -= countOfEdgesOnStep[pCurrentPosition];
            countOfEdgesOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void BackActionByPlaceForcedSorted(int pCurrentPosition)
        {
            currentCountOfVertices -= (fRoute[fCurrentPosition] > 0) ? 1 : 0;
            BackActionByPlaceAdvSorted(pCurrentPosition);
            currentCountOfEdges -= countOfEdgesOnStep[pCurrentPosition];
            countOfEdgesOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
        protected void AddOptimalStorage()
        {
            AddOptimalStorage(fOptimalRoute.ToArray());
        }
        //--------------------------------------------------------------------------------------
        protected void AddOptimalStorage(int[] pOptimalRoute)
        {
            if (optimalStorage.Count >= 10)
                optimalStorage.Dequeue();
            optimalStorage.Enqueue(Tuple.Create(pOptimalRoute, fOptimalWeight));
        }
        //--------------------------------------------------------------------------------------
        protected void AddOptimalValueChangeData()
        {
            AddOptimalValueChangeDataBody();
        }
        //--------------------------------------------------------------------------------------
        partial void AddOptimalValueChangeDataBody();
        //--------------------------------------------------------------------------------------
        private bool Limiter(int pCurrentPosition, int pNumberOfVertex, 
            int pCurrentCountOfEdges, int pCountOfEdgesOnStep, 
            int pCurrentWeightOfRoute, int pWeightOnStep)
        {
            if (pNumberOfVertex - pCurrentPosition < 5)
                return false;

            if (pCurrentCountOfEdges <= 2 || pCurrentPosition <= 2 )
                return (double)pCurrentWeightOfRoute > 0.00001;
            if (pCurrentCountOfEdges <= 3 || pCurrentPosition <= 3 )
                return (double)pCurrentWeightOfRoute > 1.00001;
            if (pCurrentCountOfEdges <= 6 || pCurrentPosition <= 4 )
                return (double)pCurrentWeightOfRoute > 2.00001;

            if (pCountOfEdgesOnStep == 0)
                return (double)pCurrentWeightOfRoute > 0.0001;
            else if (pCountOfEdgesOnStep == 1)
                return (double)pCurrentWeightOfRoute > 2.0001;
            else if (pCountOfEdgesOnStep == 2)
                return (double)pCurrentWeightOfRoute > 6.0001;
            else if (pCountOfEdgesOnStep == 3)
                return (double)pCurrentWeightOfRoute > 10.0001;
            else if (pCountOfEdgesOnStep == 4)
                return (double)pCurrentWeightOfRoute > 16.0001;

            return false;
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
