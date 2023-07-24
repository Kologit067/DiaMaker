using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Caculation.GraphLib;
using Common.CommonLib.Interfaces;

namespace GraphTest
{
    [TestClass]
    //-------------------------------------------------------------------------------------------------------
    // class GraphTest
    //-------------------------------------------------------------------------------------------------------
    public class GraphTest
    {
        [TestMethod]
        //-------------------------------------------------------------------------------------------------------
        public void CreateConnectedSubGraphesTest()
        {
            // Arrange 
            IGraph<IVertex> graph = new Graph<IVertex>();
            graph.Vertices.Add(new CVertex("A1"));
            graph.Vertices.Add(new CVertex("A2"));
            graph.Vertices.Add(new CVertex("A3"));
            graph.Vertices.Add(new CVertex("B1"));
            graph.Vertices.Add(new CVertex("B2"));
            graph.Vertices.Add(new CVertex("B3"));
            graph.Vertices.Add(new CVertex("C1"));
            graph.Vertices.Add(new CVertex("C2"));
            graph.Vertices.Add(new CVertex("C3"));
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(3, 4);
            graph.AddEdge(3, 5);
            graph.AddEdge(6, 7);
            graph.AddEdge(6, 8);

            // Action
            GraphLib.GraphSplitter graphSplitter = new GraphLib.GraphSplitter(graph);
            List<IGraph<IVertex>> graphs = graphSplitter.CreateConnectedSubGraphes();
            
            // Assert
            Assert.IsNotNull(graphs, "No graph was created");
            Assert.AreEqual(graphs.Count, 3, "Incorrect number of Connected graphs. Excpected: 3");
            // graph A
            IGraph<IVertex> grapha = graphs.Where(g => g.Vertices.Any(v => v.Name.StartsWith("A"))).FirstOrDefault();
            Assert.IsNotNull(grapha, "Graph A is not found");
            Assert.AreEqual(grapha.Vertices.Count, 3, "Incorrect number of verteices in Graph A. Excpected: 3");
            Assert.AreEqual(grapha.GetEdgeCount(), 2, "Incorrect number of Edges in Graph A. Excpected: 2");
            Assert.AreEqual(grapha.IsContainEdge("A1", "A2"), true);
            Assert.AreEqual(grapha.IsContainEdge("A1", "A3"), true);
            // graph B
            IGraph<IVertex> graphb = graphs.Where(g => g.Vertices.Any(v => v.Name.StartsWith("B"))).FirstOrDefault();
            Assert.IsNotNull(graphb, "Graph B is not found");
            Assert.AreEqual(graphb.Vertices.Count, 3, "Incorrect number of verteices in Graph B. Excpected: 3");
            Assert.AreEqual(graphb.GetEdgeCount(), 2, "Incorrect number of Edges in Graph B. Excpected: 2");
            Assert.AreEqual(graphb.IsContainEdge("B1", "B2"), true);
            Assert.AreEqual(graphb.IsContainEdge("B1", "B3"), true);
            // graph C
            IGraph<IVertex> graphc = graphs.Where(g => g.Vertices.Any(v => v.Name.StartsWith("C"))).FirstOrDefault();
            Assert.IsNotNull(graphc, "Graph C is not found");
            Assert.AreEqual(graphc.Vertices.Count, 3, "Incorrect number of verteices in Graph C. Excpected: 3");
            Assert.AreEqual(graphc.GetEdgeCount(), 2, "Incorrect number of Edges in Graph C. Excpected: 2");
            Assert.AreEqual(graphc.IsContainEdge("C1", "C2"), true);
            Assert.AreEqual(graphc.IsContainEdge("C1", "C3"), true);

        }
        [TestMethod]
        //-------------------------------------------------------------------------------------------------------
        public void CreateStrongConnectedPartsTest()
        {
            // Arrange 
            IGraph<IVertex> graph = new Graph<IVertex>();
            graph.Vertices.Add(new CVertex("A1"));  // 0
            graph.Vertices.Add(new CVertex("A2"));  // 1
            graph.Vertices.Add(new CVertex("A3"));  // 2
            graph.Vertices.Add(new CVertex("B1"));  // 3
            graph.Vertices.Add(new CVertex("B2"));  // 4
            graph.Vertices.Add(new CVertex("B3"));  // 5
            graph.Vertices.Add(new CVertex("C1"));  // 6
            graph.Vertices.Add(new CVertex("C2"));  // 7
            graph.Vertices.Add(new CVertex("C3"));  // 8
            graph.Vertices.Add(new CVertex("C4"));  // 9
            graph.AddEdge(0, 1);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 2);

            graph.AddEdge(3, 4);
            graph.AddEdge(3, 5);
            graph.AddEdge(4, 5);

            graph.AddEdge(6, 7);
            graph.AddEdge(7, 8);
            graph.AddEdge(8, 9);
            graph.AddEdge(6, 9);

            graph.AddEdge(2, 6);
            graph.AddEdge(1, 3);

            // Action
            GraphLib.GraphSplitter graphSplitter = new GraphLib.GraphSplitter(graph);
            List<HashSet<int>> parts = graphSplitter.CreateStrongConnectedParts();

            // Assert
            HashSet<int> expectedSetA = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> expectedSetB = new HashSet<int>() { 3, 4, 5 };
            HashSet<int> expectedSetC = new HashSet<int>() { 6, 7, 8, 9 };


            Assert.IsNotNull(parts, "No part was created");
            Assert.AreEqual(parts.Count, 3, "Incorrect number of parts. Excpected: 3");
            
            // graph A
            HashSet<int> parta = parts.Where(g => g.Any(i => graph.Vertices[i].Name.StartsWith("A"))).FirstOrDefault();
            Assert.IsNotNull(parta, "Part A is not found");
            Assert.AreEqual(parta.Count, 3, "Incorrect number of verteices in part A. Excpected: 3");
            Assert.AreEqual(parta.SetEquals(expectedSetA), true);

            // graph B
            HashSet<int> partb = parts.Where(g => g.Any(i => graph.Vertices[i].Name.StartsWith("B"))).FirstOrDefault();
            Assert.IsNotNull(partb, "Part A is not found");
            Assert.AreEqual(partb.Count, 3, "Incorrect number of verteices in part B. Excpected: 3");
            Assert.AreEqual(partb.SetEquals(expectedSetB), true);

            // graph C
            HashSet<int> partc = parts.Where(g => g.Any(i => graph.Vertices[i].Name.StartsWith("C"))).FirstOrDefault();
            Assert.IsNotNull(partc, "Part C is not found");
            Assert.AreEqual(partc.Count, 4, "Incorrect number of verteices in part C. Excpected: 4");
            Assert.AreEqual(partc.SetEquals(expectedSetC), true);

        }
        //-------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------
}
