using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaMakerModel.Tracing
{
    //--------------------------------------------------------------------------------------
    // class Tracing
    //--------------------------------------------------------------------------------------
    public partial class Tracing
    {
        private List<Edge> edges;
        //--------------------------------------------------------------------------------------
        private void TraverseLineCombination()
        {
            List<Edge> lAlternateEdges = edges.Where(e => e.IsAlternate).ToList();
            int[] lCurrentSelectedIndecies = new int[edges.Count];
            int ina = 0;
            foreach (var pos in edges.Where(e => !e.IsAlternate).Select(e => e.ConnectingLines[0].Index))
                lCurrentSelectedIndecies[ina++] = pos;
            int lAlternateStart = ina;
            int lVectorLenth = lAlternateEdges.Count;
            int lMaxVector = 1 << lVectorLenth;
            ConnectingLine[] lSelectedLines = new ConnectingLine[lVectorLenth];
            ConnectingLine[] lBestOptimal = new ConnectingLine[lVectorLenth];
            int lBestOptimalValue = int.MaxValue;
            for (int lVerctorAsNumber = 0; lVerctorAsNumber < lMaxVector; lVerctorAsNumber++)
            {
                for (int i = 0; i < lVectorLenth; i++)
                    lSelectedLines[i] = lAlternateEdges[i].GetConnectingLine(lVerctorAsNumber, i);
                // Item1 - number of intersection
                int lCurrentOptimalValue = DefineOptimalOrder(lSelectedLines, lCurrentSelectedIndecies, lAlternateStart);
                if (lCurrentOptimalValue < lBestOptimalValue)
                {
                    lBestOptimalValue = lCurrentOptimalValue;
                    for (int i = 0; i < lVectorLenth; i++)
                        lBestOptimal[i] = lSelectedLines[i];
                }
            }
            connectingLines = lBestOptimal.ToList();
            foreach (var cl in edges.Where(e => !e.IsAlternate).SelectMany(e => e.ConnectingLines))
                connectingLines.Add(cl);
            for (int i = 0; i < connectingLines.Count; i++)
                connectingLines[i].IndexInOptimalList = i;
        }
        //--------------------------------------------------------------------------------------
        private int DefineOptimalOrder(ConnectingLine[] pSelectedLines, int[] pCurrentSelectedIndecies, int pAlternateStart)
        {
            for (int i = 0; i < pSelectedLines.Length; i++)
                pCurrentSelectedIndecies[pAlternateStart + i] = pSelectedLines[i].Index;
            int lIntersectionNumber = 0;
            for (int i = pAlternateStart; i < pCurrentSelectedIndecies.Length; i++)
                for (int j = 0; j < i; j++)
                    lIntersectionNumber += connectingLineIntersection[pCurrentSelectedIndecies[i], pCurrentSelectedIndecies[j]];

            return lIntersectionNumber;
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
