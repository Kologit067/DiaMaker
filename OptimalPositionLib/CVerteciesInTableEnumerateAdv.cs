using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;

namespace OptimalPositionLib
{
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerateAdv
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerateAdv : CVerteciesInTableEnumerate
    {
        private int currentWeightOfRoute;
        private int[] weightOnStep;				
        private Int32[] positionOfVertex;
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerateAdv(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorythmName)
            : base(pGraph, pMatrixofDistance, pAlgorythmName)
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
            fIterationCount++;
            int lPosition = fCurrentPosition - 1;
            if (lPosition >= 0 && fRoute[lPosition] > 0)
                positionOfVertex[fRoute[lPosition] - 1] = lPosition;

            if ( lPosition >= 0)
            {
                weightOnStep[lPosition] = WeightOfStep(lPosition);
                currentWeightOfRoute += weightOnStep[lPosition];
            }
            //if (currentWeightOfRoute >= fOptimalWeight)
            //    return true;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeight(fRoute);
                if (lWeightOfRoute != currentWeightOfRoute)
                    throw new Exception("Logical Error: incorrect computation of Current Weight Of Route.");
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
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        private int WeightOfStep(int position)
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
        protected override void BackAction(int pCurrentPosition)
        {
            if ( fRoute[fCurrentPosition] > 0 )
                positionOfVertex[fRoute[fCurrentPosition]-1] = -1;
            currentWeightOfRoute -= weightOnStep[pCurrentPosition];
            weightOnStep[pCurrentPosition] = 0;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class CVerteciesInTableEnumerateAdvWithLowEstimate
    //--------------------------------------------------------------------------------------
    public class CVerteciesInTableEnumerateAdvWithLowEstimate : CVerteciesInTableEnumerateAdv
    {
        //--------------------------------------------------------------------------------------
        public CVerteciesInTableEnumerateAdvWithLowEstimate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorythmName)
            : base(pGraph, pMatrixofDistance, pAlgorythmName)
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
    //--------------------------------------------------------------------------------------
}
