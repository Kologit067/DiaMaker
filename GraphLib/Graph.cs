using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;

namespace Caculation.GraphLib
{
    //-------------------------------------------------------------------------------------------------------
    // class Graph
    //-------------------------------------------------------------------------------------------------------
    public class Graph<T> : IGraph<T> where T : IVertex
    {
        private List<T> _fVertices = new List<T>();
        //-------------------------------------------------------------------------------------------------------
        public List<T> Vertices 
        {
            get
            {
                return _fVertices;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void AddEdge(int pVertexFrom, int pVertexto)
        {
            if (_fVertices[pVertexFrom].EndPoints.Contains(pVertexto))
                return;
            _fVertices[pVertexFrom].EndPoints.Add(pVertexto);
            _fVertices[pVertexto].StartPoints.Add(pVertexFrom);
        }
        //-------------------------------------------------------------------------------------------------------
        public int GetVertexWeight(int pVertexNumber)
        {
            return _fVertices[pVertexNumber].Weight;
        }
        //-------------------------------------------------------------------------------------------------------
        public int GetEdgeCount()
        {
            return _fVertices.Sum(v => v.StartPoints.Count);
        }
        //-------------------------------------------------------------------------------------------------------
        public bool IsContainEdge(string pNameVertexStart, string pNameVertexEnd)
        {
            return _fVertices.Any(v => v.Name == pNameVertexStart && v.EndPoints.Any(e => _fVertices[e].Name == pNameVertexEnd));
        }
        //-------------------------------------------------------------------------------------------------------
        public int DefineInfimum(int pWidth, int pHeight, IMatrixOfDistance pMatrixofDistance)
        {
            int lMaxEstimate = 0;
            foreach (IVertex v in Vertices)
            {
                int lEstimate = 0;
                int lRestCount = v.EndPoints.Count;
                if (lRestCount > 4)
                {
                    lRestCount -= 4;
                    if (lRestCount > 4)
                    {
                        lEstimate += 4 * pMatrixofDistance[0, pHeight + 1];
                        lRestCount -= 4;
                        if (lRestCount > 8)
                        {
                            lEstimate += 8 * pMatrixofDistance[0, pHeight + 2];
                            lRestCount -= 8;
                            lEstimate += lRestCount * pMatrixofDistance[0, 2 * pHeight + 2];
                        }
                        else
                            lEstimate += lRestCount * pMatrixofDistance[0, pHeight + 2];
                    }
                    else
                        lEstimate += lRestCount * pMatrixofDistance[0, pHeight + 1];
                }
                if (lEstimate > lMaxEstimate)
                    lMaxEstimate = lEstimate;
            }
            return lMaxEstimate;
        }
        ////-------------------------------------------------------------------------------------------------------
        //public List<IGraph<IVertex>> CreateConnectedSubGraphes()
        //{
        //    List<IGraph<IVertex>> graphs = new List<IGraph<IVertex>>();
        //    Queue<int> unprocessed = new Queue<int>();
        //    List<int> unexamined = new List<int>(Vertices.Count);
        //    List<int> currentGroup = new List<int>(Vertices.Count);
        //    for (int i = 0; i < Vertices.Count; i++)
        //        unexamined.Add(i);

        //    while(unexamined.Count > 0)
        //    {
        //        int cv = unexamined[0];
        //        unprocessed.Enqueue(cv);
        //        unexamined.RemoveAt(0);
        //        currentGroup.Clear();
        //        currentGroup.Add(cv);
        //        while (unprocessed.Count > 0)
        //        {
        //            int curvertex = unprocessed.Dequeue();
        //            foreach (int svertex in _fVertices[curvertex].EndPoints.Union(_fVertices[curvertex].StartPoints))
        //            {
        //                if (!currentGroup.Contains(svertex))
        //                {
        //                    unexamined.Remove(svertex);
        //                    unprocessed.Enqueue(svertex);
        //                    currentGroup.Add(svertex);
        //                }
        //            }
        //        }
        //        Graph<IVertex> newGraph = new Graph<IVertex>();
        //        for (int i = 0; i < currentGroup.Count; i++)
        //            newGraph.Vertices.Add(new CVertex(_fVertices[currentGroup[i]].Name));
        //        for (int i = 0; i < currentGroup.Count; i++)
        //        {
        //            foreach (int v in _fVertices[currentGroup[i]].EndPoints)
        //            {
        //                int posInGroup = currentGroup.IndexOf(v);
        //                if (posInGroup >= 0)
        //                {
        //                    newGraph.AddEdge(i, posInGroup);
        //                }
        //            }
        //        }
        //        graphs.Add(newGraph);
        //    }

        //    return graphs;
        //}

        //public List<HashSet<int>> CreateStrongConnectedParts()
        //{
        //    // create  queu linkeditem unprocessed element
        //    // create list of set (cicles)
        //    // process all unproces
        //    // all adjaced vertex
        //    // create new linkeditem for adjaced (parent - current item)
        //    // check all item in chain
        //    // if new linkeditem already exist crete new cicle
        //    // else add item into queu
        //    // process cicles
        //    // eximen all pair of cicle
        //    // if cicles intersects join cicles
        //}

        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
