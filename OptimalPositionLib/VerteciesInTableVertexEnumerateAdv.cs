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
    // class VerteciesInTableVertexEnumerateAdv
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumerateAdv : VerteciesInTableVertexEnumerate
    {
        //private int currentWeightOfRoute;
        //private int[] weightOnStep;
        //private Int32[] positionOfPlace;
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumerateAdv(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
            currentWeightOfRoute = 0;
            weightOnStep = new int[fNumberOfPlace];
            positionOfPlace = new Int32[fSize];

            for (int i = 0; i < fSize; i++)
                positionOfPlace[i] = -1;
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            return IsEliminableByVertexAdv();
            /*
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfPlace[fRoute[lPosition] - 1] = lPosition;

            if ( lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStepPlace(lPosition);
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
             * */
        }
        //--------------------------------------------------------------------------------------
        protected override void BackAction(int pCurrentPosition)
        {
            BackActionByVertexAdv(pCurrentPosition);
            //positionOfPlace[fRoute[fCurrentPosition] - 1] = -1;
            //currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            //weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableVertexEnumerateAdvWithLowEstimate
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumerateAdvWithLowEstimate : VerteciesInTableVertexEnumerateAdv
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumerateAdvWithLowEstimate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
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
    // class VerteciesInTableVertexEnumerateAdvSorted
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumerateAdvSorted : VerteciesInTableVertexEnumerateAdv
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumerateAdvSorted(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
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
            return IsEliminableByVertexAdvSorted();
        }
        //--------------------------------------------------------------------------------------
        protected override void BackAction(int pCurrentPosition)
        {
            BackActionByVertexAdvSorted(pCurrentPosition);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableVertexEnumerateAdvSortedVertex
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumerateAdvSortedVertex : VerteciesInTableVertexEnumerateAdv
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumerateAdvSortedVertex(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
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
            return IsEliminableByVertexAdvSortedVertex();
        }
        //--------------------------------------------------------------------------------------
        protected override void BackAction(int pCurrentPosition)
        {
            BackActionByVertexAdvSortedVertex(pCurrentPosition);
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
