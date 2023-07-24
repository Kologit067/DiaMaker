using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArrayExtensions;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;
using CommonLib;

namespace OptimalPositionLib
{
    //--------------------------------------------------------------------------------------
    // class VerteciesInTablePlaceEnumerate
    //--------------------------------------------------------------------------------------
    public class VerteciesInTablePlaceEnumerate : VerteciesInTablePlace 
    {
//        private List<string> fResultStringSet = new List<string>();
//        protected int[] fOptimalRoute = null;				// маршрут обхода
        ////--------------------------------------------------------------------------------------
        //public int[] OptimalRoute
        //{
        //    get
        //    {
        //        return fOptimalRoute;
        //    }
        //}
        //--------------------------------------------------------------------------------------
        public VerteciesInTablePlaceEnumerate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
            //fGraph = pGraph;
            //fMatrixofDistance = pMatrixofDistance;
            //fOptimalRoute = new int[fNumberOfPlace];
            fOptimalWeight = int.MaxValue;
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            return IsEliminableByPlace();
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByPlace(fRoute);
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++ )
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;

                    AddOptimalStorage();
                    AddOptimalValueChangeData();

                    changeOptimalNumber++;
                    fUpdateOptcount++;
                    ChangeOptimalWeightAction();
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
    }
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerateWithLowEstimate
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerateWithLowEstimate : VerteciesInTablePlaceEnumerate
    {
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerateWithLowEstimate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
        protected override void ChangeOptimalWeightAction()
        {
            if (LowEstimate >= fOptimalWeight)
                fIsStop = true;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerateWithLowSorted
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerateWithLowSorted : VerteciesInTablePlaceEnumerate
    {
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerateWithLowSorted(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
            sortedVertices = sortedVertices.OrderByDescending(v => pGraph.Vertices[v].Weight).ToArray();
            sortedPlaces = sortedPlaces.OrderByDescending(v => VerticesInTableApproximate.WeightOfPlacePosition(v, fMatrixofDistance)).ToArray();
            sortedVerticesInverse = sortedVertices.InversePermitation();
            sortedPlacesInverse = sortedPlaces.InversePermitation();
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            return IsEliminableByPlaceSorted();
        }
        //--------------------------------------------------------------------------------------
        protected override void PostExecuteAction()
        {

        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerateWithLowSortedPlace
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerateWithLowSortedPlace : VerteciesInTablePlaceEnumerate
    {
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerateWithLowSortedPlace(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
            sortedVertices = sortedVertices.OrderByDescending(v => pGraph.Vertices[v].Weight).ToArray();
            sortedPlaces = sortedPlaces.OrderByDescending(v => VerticesInTableApproximate.WeightOfPlacePosition(v, fMatrixofDistance)).ToArray();
            sortedVerticesInverse = sortedVertices.InversePermitation();
            sortedPlacesInverse = sortedPlaces.InversePermitation();
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            return IsEliminableByPlaceSortedPlace();
        }
        //--------------------------------------------------------------------------------------
        protected override void PostExecuteAction()
        {

        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------
}
