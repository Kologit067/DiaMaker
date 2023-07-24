using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimalPositionLib;
using Common.CommonLib.Interfaces;
using OptimalPositionLib.Matrix;
using Caculation.GraphLib;

namespace CalculationTest
{
    [TestClass]
    public class CVerteciesInTableEnumerateTest
    {
        private int lMaxDurability = 120000;
        [TestMethod]
        public void ComputeWeightTest()
        {
            IGraph<IVertex> lGraph = new Graph<IVertex>();
            IVertex v = new CVertex("Town");
            lGraph.Vertices.Add(v);
            v = new CVertex("Street");
            lGraph.Vertices.Add(v);
            v = new CVertex("Garden");
            lGraph.Vertices.Add(v);
            v = new CVertex("Market");
            lGraph.Vertices.Add(v);
            v = new CVertex("Monument");
            lGraph.Vertices.Add(v);
            lGraph.Vertices[1].EndPoints.Add(0);
            lGraph.Vertices[2].EndPoints.Add(0);
            lGraph.Vertices[3].EndPoints.Add(0);
            lGraph.Vertices[4].EndPoints.Add(0);
            int[] lRoute = new int[9] { 0, 0, 0, 0, 1, 2, 3, 5, 4 };
            IMatrixOfDistance lMatrixofDistance = new CMatrixOfDistance(3, 3);
            lMatrixofDistance.Generate();
            VerteciesInTablePlaceEnumerate lVerteciesInTableEnumerate = VerteciesInTableDefine.Create("Enumerate", lGraph, lMatrixofDistance, lMaxDurability) as VerteciesInTablePlaceEnumerate;
            int lWeightOfRoute = lVerteciesInTableEnumerate.ComputeWeightByPlace(lRoute);
            Assert.AreEqual(lWeightOfRoute, 2);

        }
    }
}
