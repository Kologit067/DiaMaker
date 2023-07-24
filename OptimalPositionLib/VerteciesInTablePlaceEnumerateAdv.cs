using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArrayExtensions;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;

namespace OptimalPositionLib
{
    //--------------------------------------------------------------------------------------
    // class VerteciesInTablePlaceEnumerateAdv
    //--------------------------------------------------------------------------------------
    public class VerteciesInTablePlaceEnumerateAdv : VerteciesInTablePlaceEnumerate
    {
        //private int currentWeightOfRoute;
        //private int[] weightOnStep;				
        //private Int32[] positionOfVertex;
        //--------------------------------------------------------------------------------------
        public VerteciesInTablePlaceEnumerateAdv(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
            currentWeightOfRoute = 0;
            weightOnStep = new int[fNumberOfPlace];
            positionOfVertex = new Int32[fSize];

            for (int i = 0; i < fSize; i++)
                positionOfVertex[i] = -1;
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            return IsEliminableByPlaceAdv();
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfVertex[fRoute[lPosition] - 1] = lPosition;

            if ( lPosition >= 0)
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
        protected override void BackAction(int pCurrentPosition)
        {
            BackActionByPlaceAdv(pCurrentPosition);
            //if ( fRoute[fCurrentPosition] > 0 )
            //    positionOfVertex[fRoute[fCurrentPosition]-1] = -1;
            //currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            //weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerateAdvWithLowEstimate
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerateAdvWithLowEstimate : VerteciesInTablePlaceEnumerateAdv
    {
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerateAdvWithLowEstimate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
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
    // class VerteciesInTablePlaceEnumerateAdvSortedPlace
    //--------------------------------------------------------------------------------------
    public class VerteciesInTablePlaceEnumerateAdvSortedPlace : VerteciesInTablePlaceEnumerateAdv
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTablePlaceEnumerateAdvSortedPlace(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance,
            string pAlgorithmName, int pMaxDurability)
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
            return IsEliminableByPlaceAdvSortedPlace();
        }
        //--------------------------------------------------------------------------------------
        protected override void BackAction(int pCurrentPosition)
        {
            BackActionByPlaceAdvSortedPlace(pCurrentPosition);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class VerteciesInTablePlaceEnumerateAdvSorted
    //--------------------------------------------------------------------------------------
    public class VerteciesInTablePlaceEnumerateAdvSorted : VerteciesInTablePlaceEnumerateAdv
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTablePlaceEnumerateAdvSorted(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance,
            string pAlgorithmName, int pMaxDurability)
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
            return IsEliminableByPlaceAdvSorted();
        }
        //--------------------------------------------------------------------------------------
        protected override void BackAction(int pCurrentPosition)
        {
            BackActionByPlaceAdvSorted(pCurrentPosition);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
