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
    // class VerticesInTableApproximate
    //--------------------------------------------------------------------------------------
    public class VerticesInTableApproximate : VerteciesInTableDefine
    {
        //private IGraph<IVertex> fGraph;
        //private IMatrixOfDistance fMatrixofDistance;
        //protected int[] fOptimalRoute = null;				// маршрут обхода
        private int[] positionVertexInMatrix;
        private bool[] sectionSelected;
        private List<int> notSetVertices;
        private Queue<int> notProcessVertices;
        private List<int> processVertices;
        //private int[] firstLevelPlace = new int[4];
        //private int[] secondLevelPlace = new int[4];
        //private int[] thirdLevelPlace = new int[8];
        //private int[] firstLevelVertices = new int[4];
        //private int[] secondLevelVertices = new int[4];
        //private int[] thirdLevelVertices = new int[8];
        //private int[] optimalPlacingVector = new int[8];
        private List<int> firstLevelPlace = new List<int>(4);
        private List<int> secondLevelPlace = new List<int>(4);
        private List<int> thirdLevelPlace = new List<int>(8);
        private List<int> firstLevelVertices = new List<int>(4);
        private List<int> secondLevelVertices = new List<int>(4);
        private List<int> thirdLevelVertices = new List<int>(8);
        private List<int> optimalPlacingVector = new List<int>(8);
        private int positionInNotSetVertices;
        ////--------------------------------------------------------------------------------------
        //public int[] OptimalRoute
        //{
        //    get
        //    {
        //        return fOptimalRoute;
        //    }
        //}
        //--------------------------------------------------------------------------------------
        public VerticesInTableApproximate(IGraph<IVertex> pGraph, IMatrixOfDistance pMatrixofDistance, string pAlgorythmName)
            : base(pGraph, pMatrixofDistance, pGraph.Vertices.Count, pMatrixofDistance.Length, pAlgorythmName)
        {
            fGraph = pGraph;
            fMatrixofDistance = pMatrixofDistance;
            fNumberOfPlace = pMatrixofDistance.Length;
            fOptimalRoute = new int[fNumberOfPlace];
            sectionSelected = new bool[fNumberOfPlace];
            notSetVertices = new List<int>(fGraph.Vertices.Count);
            notProcessVertices = new Queue<int>(fGraph.Vertices.Count);
            processVertices = new List<int>(fGraph.Vertices.Count);
            for (int i = 0; i < fGraph.Vertices.Count; i++)
                notSetVertices.Add(i);
            positionVertexInMatrix = new int[fGraph.Vertices.Count];
            for (int i = 0; i < positionVertexInMatrix.Length; i++)
                positionVertexInMatrix[i] = -1;
            for (int i = 0; i < fOptimalRoute.Length; i++)
                fOptimalRoute[i] = -1;
            notSetVertices.Sort(new VertexComparison(fGraph));
        }
        //--------------------------------------------------------------------------------------
        public override void Execute()
        {
            while (notSetVertices.Count > 0 )
            {
                int nextv = notSetVertices[0];      // DefineHeavestVertex();
                int bestpos = DefineBestPositionInTable();
                MoveToSetState(nextv, bestpos);
                while (notSetVertices.Count > 0 && notProcessVertices.Count > 0)
                {
                    positionInNotSetVertices = 0;
                    if (positionInNotSetVertices < notSetVertices.Count)
                        Step1(nextv, bestpos);
                    if (positionInNotSetVertices < notSetVertices.Count)
                        Step2(nextv, bestpos);
                    if (positionInNotSetVertices < notSetVertices.Count)
                        Step3(nextv, bestpos);
                    notProcessVertices.Dequeue();
                    processVertices.Add(nextv);
                    if (notProcessVertices.Count != 0)
                    {
                        nextv = notProcessVertices.Peek();
                        bestpos = positionVertexInMatrix[nextv];
                    }
                }
            }
            for (int i = 0; i < fOptimalRoute.Length; i++)
                fOptimalRoute[i] += 1;
            fOptimalWeight = ComputeWeight(OptimalRoute);
            optimalStorage.Enqueue(Tuple.Create(OptimalRoute.ToArray(), fOptimalWeight));
        }
        //--------------------------------------------------------------------------------------
        private void Step3(int pCurrentVertex, int pCurrentPosition)
        {
            int thirdLevelNumber = DefineThirdLevelPlace(pCurrentPosition);
            DefineThirdLevelVertices(pCurrentVertex, thirdLevelNumber);
            if (thirdLevelVertices.Count > 0)
            {
                //while (thirdLevelPlace.Count > thirdLevelVertices.Count)
                //{
                //    thirdLevelPlace.RemoveAt(thirdLevelVertices.Count);
                //    thirdLevelNumber--;
                //}
                OptimalPlacing( thirdLevelVertices, thirdLevelPlace);
            }
        }
        //--------------------------------------------------------------------------------------
        private void DefineThirdLevelVertices(int pCurrentVertex, int pThirdLevelNumber)
        {
            int j = pThirdLevelNumber;
            thirdLevelVertices.Clear();
            while (positionInNotSetVertices < notSetVertices.Count && j > 0)
            {
                if (fGraph.Vertices[notSetVertices[positionInNotSetVertices]].ContaintVertex(pCurrentVertex))
                {
                    thirdLevelVertices.Add(notSetVertices[positionInNotSetVertices]);
                    j--;
                }
                positionInNotSetVertices++;
            }
        }
        //--------------------------------------------------------------------------------------
        private int DefineThirdLevelPlace(int pCurrentPosition)
        {
            thirdLevelPlace.Clear();
            if (pCurrentPosition % fMatrixofDistance.Height > 1 && pCurrentPosition / fMatrixofDistance.Height > 0 && !sectionSelected[pCurrentPosition - 2 - fMatrixofDistance.Height])
                thirdLevelPlace.Add(pCurrentPosition - 2 - fMatrixofDistance.Height);
            if (pCurrentPosition % fMatrixofDistance.Height != 0 && pCurrentPosition / fMatrixofDistance.Height > 1 && !sectionSelected[pCurrentPosition - 1 - 2*fMatrixofDistance.Height])
                thirdLevelPlace.Add(pCurrentPosition - 1 - 2*fMatrixofDistance.Height);
            if (fMatrixofDistance.Height - (pCurrentPosition % fMatrixofDistance.Height) < 2 && pCurrentPosition / fMatrixofDistance.Height < fMatrixofDistance.Width - 1 && !sectionSelected[pCurrentPosition + 2 + fMatrixofDistance.Height])
                thirdLevelPlace.Add(pCurrentPosition + 2 + fMatrixofDistance.Height);
            if ((pCurrentPosition + 1) % fMatrixofDistance.Height != 0 && pCurrentPosition / fMatrixofDistance.Height < fMatrixofDistance.Width - 2 && !sectionSelected[pCurrentPosition + 1 + 2*fMatrixofDistance.Height])
                thirdLevelPlace.Add(pCurrentPosition + 1 + 2*fMatrixofDistance.Height);
            if (pCurrentPosition / fMatrixofDistance.Height > 0 && (pCurrentPosition + 1) % fMatrixofDistance.Height - (pCurrentPosition % fMatrixofDistance.Height) < 2 && !sectionSelected[pCurrentPosition - fMatrixofDistance.Height + 2])
                thirdLevelPlace.Add(pCurrentPosition - fMatrixofDistance.Height + 2);
            if (pCurrentPosition / fMatrixofDistance.Height > 1 && (pCurrentPosition + 1) % fMatrixofDistance.Height != 0 && !sectionSelected[pCurrentPosition - 2*fMatrixofDistance.Height + 1])
                thirdLevelPlace.Add(pCurrentPosition - 2*fMatrixofDistance.Height + 1);
            if (pCurrentPosition / fMatrixofDistance.Height < fMatrixofDistance.Width - 1 && pCurrentPosition % fMatrixofDistance.Height > 1 && !sectionSelected[pCurrentPosition + fMatrixofDistance.Height - 2])
                thirdLevelPlace.Add(pCurrentPosition + fMatrixofDistance.Height - 2);
            if (pCurrentPosition / fMatrixofDistance.Height < fMatrixofDistance.Width - 2 && pCurrentPosition % fMatrixofDistance.Height != 0 && !sectionSelected[pCurrentPosition + 2*fMatrixofDistance.Height - 1])
                thirdLevelPlace.Add(pCurrentPosition + 2*fMatrixofDistance.Height - 1);
            return thirdLevelPlace.Count;
        }
        //--------------------------------------------------------------------------------------
        private void Step2(int pCurrentVertex, int pCurrentPosition)
        {
            int secondLevelNumber = DefineSecondLevelPlace(pCurrentPosition);
            DefineSecondLevelVertices(pCurrentVertex, secondLevelNumber);
            if (secondLevelVertices.Count > 0)
            {
                //while (secondLevelPlace.Count > secondLevelVertices.Count)
                //{
                //    secondLevelPlace.RemoveAt(secondLevelVertices.Count);
                //    secondLevelNumber--;
                //}
                OptimalPlacing( secondLevelVertices, secondLevelPlace);
            }
        }
        //--------------------------------------------------------------------------------------
        private void DefineSecondLevelVertices(int pCurrentVertex, int pSecondLevelNumber)
        {
            int j = pSecondLevelNumber;
            secondLevelVertices.Clear();
            while (positionInNotSetVertices < notSetVertices.Count && j > 0)
            {
                if (fGraph.Vertices[notSetVertices[positionInNotSetVertices]].ContaintVertex(pCurrentVertex))
                {
                    secondLevelVertices.Add(notSetVertices[positionInNotSetVertices]);
                    j--;
                }
                positionInNotSetVertices++;
            }
        }
        //--------------------------------------------------------------------------------------
        private int DefineSecondLevelPlace(int pCurrentPosition)
        {
            secondLevelPlace.Clear();
            if (pCurrentPosition % fMatrixofDistance.Height != 0 && pCurrentPosition / fMatrixofDistance.Height > 0 && !sectionSelected[pCurrentPosition - 1 - fMatrixofDistance.Height])
                secondLevelPlace.Add(pCurrentPosition - 1 - fMatrixofDistance.Height);
            if ((pCurrentPosition + 1) % fMatrixofDistance.Height != 0 && pCurrentPosition / fMatrixofDistance.Height < fMatrixofDistance.Width - 1 && !sectionSelected[pCurrentPosition + 1 + fMatrixofDistance.Height])
                secondLevelPlace.Add(pCurrentPosition + 1 + fMatrixofDistance.Height);
            if (pCurrentPosition / fMatrixofDistance.Height > 0 && (pCurrentPosition + 1) % fMatrixofDistance.Height != 0 && !sectionSelected[pCurrentPosition - fMatrixofDistance.Height + 1])
                secondLevelPlace.Add(pCurrentPosition - fMatrixofDistance.Height + 1);
            if (pCurrentPosition / fMatrixofDistance.Height < fMatrixofDistance.Width - 1 && pCurrentPosition % fMatrixofDistance.Height != 0 && !sectionSelected[pCurrentPosition + fMatrixofDistance.Height - 1])
                secondLevelPlace.Add(pCurrentPosition + fMatrixofDistance.Height - 1);
            return secondLevelPlace.Count;
        }
        //--------------------------------------------------------------------------------------
        private void Step1(int pCurrentVertex, int pCurrentPosition)
        {
            int firstLevelNumber = DefineFirstLevelPlace(pCurrentPosition);
            DefineFirstLevelVertices(pCurrentVertex, firstLevelNumber);
            if (firstLevelVertices.Count > 0)
            {
                //while (firstLevelPlace.Count > firstLevelVertices.Count)
                //{
                //    firstLevelPlace.RemoveAt(firstLevelVertices.Count);
                //    firstLevelNumber--;
                //}
                OptimalPlacing(firstLevelVertices, firstLevelPlace);
            }
        }
        //--------------------------------------------------------------------------------------
        private void OptimalPlacing( List<int> pVertices, List<int> pPlace)
        {
            OptimalPlacingPartEnumerate enumEngine = new OptimalPlacingPartEnumerate(fGraph, fMatrixofDistance, 
                pVertices, pPlace, positionVertexInMatrix);
            enumEngine.Execute();
            for (int i = 0; i < pPlace.Count; i++)
            {
                if ( enumEngine.OptimalRoute[i] > 0 )
                    MoveToSetState(pVertices[enumEngine.OptimalRoute[i]-1], pPlace[i]);
            }
        }
        //--------------------------------------------------------------------------------------
        private void MoveToSetState(int pNextVertex, int pNextBestPosition)
        {

            OptimalRoute[pNextBestPosition] = pNextVertex;
            positionVertexInMatrix[pNextVertex] = pNextBestPosition;
            sectionSelected[pNextBestPosition] = true;
            notProcessVertices.Enqueue(pNextVertex);
            notSetVertices.Remove(pNextVertex);
            positionInNotSetVertices--;
        }
        //--------------------------------------------------------------------------------------
        private void DefineFirstLevelVertices(int pCurrentVertex, int pFirstLevelNumber)
        {
            int j = pFirstLevelNumber;
            firstLevelVertices.Clear();
            while (positionInNotSetVertices < notSetVertices.Count && j > 0)
            {
                if (fGraph.Vertices[notSetVertices[positionInNotSetVertices]].ContaintVertex(pCurrentVertex))
                {
                    firstLevelVertices.Add(notSetVertices[positionInNotSetVertices]);
                    j--;
                }
                positionInNotSetVertices++;
            }
        }
        //--------------------------------------------------------------------------------------
        private int DefineFirstLevelPlace(int pCurrentPosition)
        {
            firstLevelPlace.Clear();
            if (pCurrentPosition % fMatrixofDistance.Height != 0 && !sectionSelected[pCurrentPosition - 1])
                firstLevelPlace.Add( pCurrentPosition - 1);
            if ((pCurrentPosition + 1) % fMatrixofDistance.Height != 0 && !sectionSelected[pCurrentPosition + 1])
                firstLevelPlace.Add( pCurrentPosition + 1);
            if (pCurrentPosition / fMatrixofDistance.Height > 0 && !sectionSelected[pCurrentPosition - fMatrixofDistance.Height])
                firstLevelPlace.Add( pCurrentPosition - fMatrixofDistance.Height);
            if (pCurrentPosition / fMatrixofDistance.Height < fMatrixofDistance.Width - 1 && !sectionSelected[pCurrentPosition + fMatrixofDistance.Height])
                firstLevelPlace.Add( pCurrentPosition + fMatrixofDistance.Height);
            return firstLevelPlace.Count;
        }
        //--------------------------------------------------------------------------------------
        private int DefineHeavestVertex()
        {
            int heavesti = 0;
            int weight = -1;
            for (int i = 1; i < notSetVertices.Count; i++)
            {
                int cweight = fGraph.GetVertexWeight(notSetVertices[i]);
                if (cweight > weight)
                {
                    weight = fGraph.Vertices[notSetVertices[i]].EndPoints.Count;
                    heavesti = i;
                }
            }
            return notSetVertices[heavesti];
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
        private int DefineBestPositionInTable()
        {
            int lWeight = 0;
            int iMax = 0;
            for (int i = 0; i < fNumberOfPlace; i++)
            {
                if (!sectionSelected[i])
                {
                    int lNewWeight = 0;

                    for (int j = 0; j < fNumberOfPlace; j++)
                    {
                        if (!sectionSelected[j])
                            lNewWeight += Math.Max(0, 5 - fMatrixofDistance[i, j]);
                    }
                    if (lNewWeight > lWeight)
                    {
                        lWeight = lNewWeight;
                        iMax = i;
                    }
                }
            }
            return iMax;
        }
        //--------------------------------------------------------------------------------------
        private class VertexComparison : IComparer<int>
        {
            private IGraph<IVertex> fGraph;
            //--------------------------------------------------------------------------------------
            public VertexComparison(IGraph<IVertex> pGraph)
            {
                fGraph = pGraph;
            }
            //--------------------------------------------------------------------------------------
            public int Compare(int x, int y)
            {
                if (fGraph.Vertices[x].Weight > fGraph.Vertices[y].Weight)
                    return -1;
                if (fGraph.Vertices[x].Weight < fGraph.Vertices[y].Weight)
                    return 1;
                return 0;
            }
            //--------------------------------------------------------------------------------------
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
