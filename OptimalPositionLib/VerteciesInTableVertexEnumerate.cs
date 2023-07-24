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
    // class VerteciesInTableVertexEnumerate
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumerate : VerteciesInTableVertex 
    {
 //       protected int[] fOptimalPlaces;
        ////--------------------------------------------------------------------------------------
        //public override int[] OptimalRoute
        //{
        //    get
        //    {
        //        return fOptimalPlaces;
        //    }
        //}
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumerate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
            : base(pGraph, pMatrixofDistance, pAlgorithmName, pMaxDurability)
        {
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            return IsEliminableByVertex();
            /*
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeightByVertex();
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++ )
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
            */
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableVertexEnumeratevWithLowEstimate
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumeratevWithLowEstimate : VerteciesInTableVertexEnumerate
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumeratevWithLowEstimate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
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
    // class VerteciesInTableVertexEnumerateSorted
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumerateSorted : VerteciesInTableVertexEnumerate
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumerateSorted(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
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
            return IsEliminableByVertexSorted();
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    // class VerteciesInTableVertexEnumerateSortedVertex
    //--------------------------------------------------------------------------------------
    public class VerteciesInTableVertexEnumerateSortedVertex : VerteciesInTableVertexEnumerate
    {
        //--------------------------------------------------------------------------------------
        public VerteciesInTableVertexEnumerateSortedVertex(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorithmName, int pMaxDurability)
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
            return IsEliminableByVertexSortedVertex();
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------------
}
