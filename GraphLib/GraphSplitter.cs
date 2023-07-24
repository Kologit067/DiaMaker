using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.CommonLib.Interfaces;

namespace GraphLib
{
    //-------------------------------------------------------------------------------------------------------
    // class GraphSplitter
    //-------------------------------------------------------------------------------------------------------
    public class GraphSplitter
    {
        private IGraph<IVertex> graph;
        private Queue<LinkedItem<int>> unprocessedElements;
        private List<HashSet<int>> cycles;
        //-------------------------------------------------------------------------------------------------------
        public GraphSplitter(IGraph<IVertex> pGraph)
        {
            graph = pGraph;
        }
        //-------------------------------------------------------------------------------------------------------
        public List<IGraph<IVertex>> CreateConnectedSubGraphes()
        {
            List<IGraph<IVertex>> graphs = new List<IGraph<IVertex>>();
            Queue<int> unprocessed = new Queue<int>();
            List<int> unexamined = new List<int>(graph.Vertices.Count);
            List<int> currentGroup = new List<int>(graph.Vertices.Count);
            for (int i = 0; i < graph.Vertices.Count; i++)
                unexamined.Add(i);

            while (unexamined.Count > 0)
            {
                int cv = unexamined[0];
                unprocessed.Enqueue(cv);
                unexamined.RemoveAt(0);
                currentGroup.Clear();
                currentGroup.Add(cv);
                while (unprocessed.Count > 0)
                {
                    int curvertex = unprocessed.Dequeue();
                    foreach (int svertex in graph.Vertices[curvertex].EndPoints.Union(graph.Vertices[curvertex].StartPoints))
                    {
                        if (!currentGroup.Contains(svertex))
                        {
                            unexamined.Remove(svertex);
                            unprocessed.Enqueue(svertex);
                            currentGroup.Add(svertex);
                        }
                    }
                }
                IGraph<IVertex> newGraph = new Caculation.GraphLib.Graph<IVertex>();
                for (int i = 0; i < currentGroup.Count; i++)
                    newGraph.Vertices.Add(new Caculation.GraphLib.CVertex(graph.Vertices[currentGroup[i]].Name));
                for (int i = 0; i < currentGroup.Count; i++)
                {
                    foreach (int v in graph.Vertices[currentGroup[i]].EndPoints)
                    {
                        int posInGroup = currentGroup.IndexOf(v);
                        if (posInGroup >= 0)
                        {
                            newGraph.AddEdge(i, posInGroup);
                        }
                    }
                }
                graphs.Add(newGraph);
            }

            return graphs;
        }
        //-------------------------------------------------------------------------------------------------------
        public List<HashSet<int>> CreateStrongConnectedParts()
        {
            List<Tuple<int, int>> processedEdges = new List<Tuple<int, int>>();
            Dictionary<int, LinkedItem<int>> passedVertices = new Dictionary<int, LinkedItem<int>>();
            // create queue of linkeditem - unprocessed element
            unprocessedElements = new Queue<LinkedItem<int>>();
            // create list of cycles
            cycles = new List<HashSet<int>>();
            var top = new LinkedItem<int>(0, null);
            unprocessedElements.Enqueue(new LinkedItem<int>(0, null));
            passedVertices.Add(top.Item, top);
            // process all unproces
            while (unprocessedElements.Count > 0)
            {
                LinkedItem<int> currentItem = unprocessedElements.Dequeue();
                // process all adjacent vertices
                foreach (int adjVertex in graph.Vertices[currentItem.Item].AdjacentVertices)
                {
                    if (currentItem.Parent != null && currentItem.Parent.Item == adjVertex)
                        continue;
                    //if (processedEdges.Any(e => e.Item1 == adjVertex && e.Item2 == currentItem.Item ||
                    //    e.Item2 == adjVertex && e.Item1 == currentItem.Item))
                    //    continue;
                    // create new linkeditem for aadjVertex
                    // check all item in chain
                    //LinkedItem<int> v = currentItem.Parent;
                    //while (v != null && v.Item != adjVertex)
                    //    v = v.Parent;
                    // if new vertex already exist in chain crete new cicle
                    if (passedVertices.ContainsKey(adjVertex))
                    {
                        var alterItem = passedVertices[adjVertex];
                        List<int> firstPath = new List<int>();

                        LinkedItem<int> v = currentItem;
                        while (v != null)
                        {
                            firstPath.Add(v.Item);
                            v = v.Parent;
                        }

                        List<int> secondPath = new List<int>();
                        v = alterItem;
                        while (v != null)
                        {
                            secondPath.Add(v.Item);
                            v = v.Parent;
                        }

                        firstPath.Reverse();
                        secondPath.Reverse();

                        HashSet<int> newCycle = new HashSet<int>();
                        int i = 0;
                        while (!(firstPath[i] == secondPath[i] && firstPath[i + 1] != secondPath[i + 1]))
                            i++;
                        newCycle.Add(firstPath[i]);
                        while (i < firstPath.Count || i < secondPath.Count)
                        {
                            if (i < firstPath.Count)
                                newCycle.Add(firstPath[i]);
                            if (i < secondPath.Count)
                                newCycle.Add(secondPath[i]);
                            i++;
                        }
                        cycles.Add(newCycle);
                    }
                    else // else add item into queue
                    {
                        LinkedItem<int> nextItem = new LinkedItem<int>(adjVertex, currentItem);
                        unprocessedElements.Enqueue(nextItem);
                        passedVertices.Add(nextItem.Item, nextItem);
                    }
//                    processedEdges.Add(Tuple.Create(adjVertex, currentItem.Item));
                }
            }
            // process cicles
            bool isUnionAction = true;
            while (isUnionAction)
            {
                isUnionAction = false;
                // eximen all pair of cicle
                for (int i = 0; i < cycles.Count; i++)
                    for (int j = i + 1; j < cycles.Count; )
                    {
                        // if cicles intersects unite cicles
                        if (cycles[i].Intersect(cycles[j]).Count() > 1)
                        {
                            cycles[i].UnionWith(cycles[j]);
                            cycles.RemoveAt(j);
                            isUnionAction = true;
                        }
                        else
                            j++;
                    }
            }
            return cycles;
        }
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
