using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimalPositionLib.Matrix;

namespace CaculationTest
{
    [TestClass]
    public class CMatrixOfDistanceTest
    {
        [TestMethod]
        public void GenerateTest()
        {
            CMatrixOfDistance matrix = new CMatrixOfDistance(5,5);

            Assert.AreEqual(matrix[12, 7], matrix[12, 11], "0 level error");
            Assert.AreEqual(matrix[12, 7], matrix[12, 17], "0 level error");
            Assert.AreEqual(matrix[12, 7], matrix[12, 13], "0 level error");
            Assert.AreEqual(matrix[12, 6], matrix[12, 16], "1 level error");
            Assert.AreEqual(matrix[12, 6], matrix[12, 18], "1 level error");
            Assert.AreEqual(matrix[12, 6], matrix[12, 8], "1 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 5], "2 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 15], "2 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 21], "2 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 23], "2 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 19], "2 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 9], "2 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 3], "2 level error");
            Assert.AreEqual(matrix[12, 1], matrix[12, 5], "2 level error");
            Assert.AreEqual(matrix[12, 0], matrix[12, 20], "3 level error");
            Assert.AreEqual(matrix[12, 0], matrix[12, 24], "3 level error");
            Assert.AreEqual(matrix[12, 0], matrix[12, 4], "3 level error");
            Assert.AreEqual(matrix[12, 2], matrix[12, 10], "4 error error");
            Assert.AreEqual(matrix[12, 2], matrix[12, 22], "4 level error");
            Assert.AreEqual(matrix[12, 2], matrix[12, 14], "4 level error");

            Assert.IsTrue(matrix[12, 7] < matrix[12, 6], "comparison error");
            Assert.IsTrue(matrix[12, 6] < matrix[12, 1], "comparison error");
            Assert.IsTrue(matrix[12, 1] < matrix[12, 0], "comparison error");
            Assert.IsTrue(matrix[12, 0] < matrix[12, 2], "comparison error");

            Assert.AreEqual(matrix[0, 1], matrix[0, 5], "0 level error");
            Assert.AreEqual(matrix[0, 7], matrix[0, 11], "2 level error");
            Assert.AreEqual(matrix[0, 8], matrix[0, 16], "3 level error");
            Assert.AreEqual(matrix[0, 2], matrix[0, 10], "5 level error");
            Assert.AreEqual(matrix[0, 13], matrix[0, 17], "7 level error");
            Assert.AreEqual(matrix[0, 14], matrix[0, 22], "8 level error");
            Assert.AreEqual(matrix[0, 3], matrix[0, 15], "10 level error");
            Assert.AreEqual(matrix[0, 19], matrix[0, 23], "11 level error");

            Assert.IsTrue(matrix[0, 1] < matrix[0, 6], "comparison error");
            Assert.IsTrue(matrix[0, 6] < matrix[0, 7], "comparison error");
            Assert.IsTrue(matrix[0, 7] < matrix[0, 8], "comparison error");
            Assert.IsTrue(matrix[0, 8] < matrix[0, 12], "comparison error");
            Assert.IsTrue(matrix[0, 12] < matrix[0, 2], "comparison error");
            Assert.IsTrue(matrix[0, 2] < matrix[0, 13], "comparison error");
            Assert.IsTrue(matrix[0, 13] < matrix[0, 14], "comparison error");
            Assert.IsTrue(matrix[0, 14] < matrix[0, 18], "comparison error");
       }
    }
}
