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
    // class VerteciesInTablePlaceEnumerate
    //--------------------------------------------------------------------------------------
    public class OptimalPlacingPartEnumerate : PermitationBase
    {
        private IGraph<IVertex> fGraph;
        private IMatrixOfDistance fMatrixofDistance;
        protected int[] fOptimalRoute = null;				// маршрут обхода
        protected int fOptimalWeight = 0;
        protected int changeOptimalNumber;
//        private int partElementNumber;
        private List<int> partVertecies;
        private List<int> partPlace;
        private int[] positionVertexInOptimal;
        private int[] positionVertexInMatrix;
        //--------------------------------------------------------------------------------------
        public int[] OptimalRoute
        {
            get
            {
                return fOptimalRoute;
            }
        }
        //--------------------------------------------------------------------------------------
        public OptimalPlacingPartEnumerate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance,
            List<int> pVertecies, List<int> pPlace, int[] pPositionVertexInMatrix, int pMaxDurability)
            : base(pVertecies.Count, pPlace.Count, pMaxDurability)
        {
            fGraph = pGraph;
            fMatrixofDistance = pMatrixofDistance;
            fOptimalRoute = new int[fNumberOfPlace];
            fOptimalWeight = int.MaxValue;
            partVertecies = pVertecies;
            partPlace = pPlace;
            positionVertexInMatrix = pPositionVertexInMatrix;
            positionVertexInOptimal = new int[fGraph.Vertices.Count];
            for (int i = 0; i < positionVertexInOptimal.Length; i++)
                positionVertexInOptimal[i] = -1;
        }
        //--------------------------------------------------------------------------------------
        public override bool IsEliminable()
        {
            fIterationCount++;
            if (fCurrentPosition == fNumberOfPlace)
            {
                int lWeightOfRoute = ComputeWeight();
                if (lWeightOfRoute < fOptimalWeight)
                {
                    for (int i = 0; i < fRoute.Length; i++ )
                        fOptimalRoute[i] = fRoute[i];
                    fOptimalWeight = lWeightOfRoute;
                    fUpdateOptcount++;
                }
                fCountTerminal++;
                
                return true;
            }
            else
            {
                return false;
            }
        }
        //--------------------------------------------------------------------------------------
        public int ComputeWeight()
        {
            int lWeight = 0;
            for (int i = 0; i < fRoute.Length; i++)
                if (fRoute[i] > 0)
                    positionVertexInOptimal[partVertecies[fRoute[i] - 1]] = i;
            foreach (int numberOfVertex in partVertecies)
            {
                foreach (int vi in fGraph.Vertices[numberOfVertex].AdjacentVertices)
                {
                    lWeight += DefineEdgeWeight(numberOfVertex, vi);
                }
                //foreach (int vi in fGraph.Vertices[numberOfVertex].EndPoints)
                //{
                //    lWeight += DefineEdgeWeight(numberOfVertex, vi);
                //}
            }

            return lWeight;
        }

        //--------------------------------------------------------------------------------------
        private int DefineEdgeWeight(int numberOfVertex, int vi)
        {
            if (fRoute[positionVertexInOptimal[numberOfVertex]] > 0)
            {
//                int positionFirst = partPlace[fRoute[positionVertexInOptimal[numberOfVertex]] - 1];
                int positionFirst = partPlace[positionVertexInOptimal[numberOfVertex]];

                if (positionVertexInMatrix[vi] >= 0)        // part 1 - from previous vertices
                {
                    return fMatrixofDistance[positionFirst, positionVertexInMatrix[vi]];
                }
                else if (positionVertexInOptimal[vi] >= 0)  // part 2 - from current vertices
                {
//                    return fMatrixofDistance[positionFirst, partPlace[fRoute[positionVertexInOptimal[vi]]] - 1];
                    return fMatrixofDistance[positionFirst, partPlace[positionVertexInOptimal[vi]]];
                }
            }
            return 0;            
        }
        //--------------------------------------------------------------------------------------
        public string CurrentRouteAsString
        {
            get
            {
                if (fRoute != null && fRoute.Length > 0)
                    return string.Join(",", fRoute.Select(i => i.ToString()));
                return "Empty";
            }
        }
        //--------------------------------------------------------------------------------------
        public string OptimalRouteAsString 
        {
            get
            {
                if (fOptimalRoute != null && fOptimalRoute.Length > 0)
                    return string.Join(",", fOptimalRoute.Select(i => i.ToString()));
                return "Empty";
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
    }
}
