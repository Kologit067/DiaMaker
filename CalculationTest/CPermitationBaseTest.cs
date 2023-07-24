using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OptimalPositionLib;

namespace OptimalPositionLibTest
{
    //--------------------------------------------------------------------------------------
    // class CPermitationBaseTest
    //--------------------------------------------------------------------------------------
    [TestClass]
    public class CPermitationBaseTest
    {
        private int lMaxDurability = 120000;
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void ExecuteTest()
        {
            CPermitationEnumeration lEnumeration = new CPermitationEnumeration(4, 7, lMaxDurability);
            lEnumeration.ExecuteBody();
        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void PermitationEmptyLastTest()
        {
            CPermitationEmptyLastEnumeration lEnumeration = new CPermitationEmptyLastEnumeration(3, 5, lMaxDurability);
            lEnumeration.Execute();
        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void PermitationEmptyLastComparePermitationBaseTest()
        {
            PermitationEmptyLastComparePermitationBase(4, 9);
            PermitationEmptyLastComparePermitationBase(5, 9);
            PermitationEmptyLastComparePermitationBase(6, 9);
            PermitationEmptyLastComparePermitationBase(7, 9);
            PermitationEmptyLastComparePermitationBase(8, 9);
            PermitationEmptyLastComparePermitationBase(1, 7);
            PermitationEmptyLastComparePermitationBase(2, 7);
            PermitationEmptyLastComparePermitationBase(3, 7);
            PermitationEmptyLastComparePermitationBase(4, 7);
            PermitationEmptyLastComparePermitationBase(5, 7);
            PermitationEmptyLastComparePermitationBase(6, 7);
            PermitationEmptyLastComparePermitationBase(7, 7);
            PermitationEmptyLastComparePermitationBase(8, 7);
            PermitationEmptyLastComparePermitationBase(9, 7);

            PermitationBaseEmptyLastComparePermitationBase(4, 9);
            PermitationBaseEmptyLastComparePermitationBase(5, 9);
            PermitationBaseEmptyLastComparePermitationBase(6, 9);
            PermitationBaseEmptyLastComparePermitationBase(7, 9);
            PermitationBaseEmptyLastComparePermitationBase(8, 9);
            PermitationBaseEmptyLastComparePermitationBase(1, 7);
            PermitationBaseEmptyLastComparePermitationBase(2, 7);
            PermitationBaseEmptyLastComparePermitationBase(3, 7);
            PermitationBaseEmptyLastComparePermitationBase(4, 7);
            PermitationBaseEmptyLastComparePermitationBase(5, 7);
            PermitationBaseEmptyLastComparePermitationBase(6, 7);
            PermitationBaseEmptyLastComparePermitationBase(7, 7);
            PermitationBaseEmptyLastComparePermitationBase(8, 7);
            PermitationBaseEmptyLastComparePermitationBase(9, 7);

        }
        //--------------------------------------------------------------------------------------
        [TestMethod]
        public void PermitationEmptyLastBaseTest()
        {
            PermitationBaseEmptyLastComparePermitationBase(4, 9);
            PermitationBaseEmptyLastComparePermitationBase(5, 9);
            PermitationBaseEmptyLastComparePermitationBase(6, 9);
            PermitationBaseEmptyLastComparePermitationBase(7, 9);
            PermitationBaseEmptyLastComparePermitationBase(8, 9);
            PermitationBaseEmptyLastComparePermitationBase(1, 7);
            PermitationBaseEmptyLastComparePermitationBase(2, 7);
            PermitationBaseEmptyLastComparePermitationBase(3, 7);
            PermitationBaseEmptyLastComparePermitationBase(4, 7);
            PermitationBaseEmptyLastComparePermitationBase(5, 7);
            PermitationBaseEmptyLastComparePermitationBase(6, 7);
            PermitationBaseEmptyLastComparePermitationBase(7, 7);
            PermitationBaseEmptyLastComparePermitationBase(8, 7);
            PermitationBaseEmptyLastComparePermitationBase(9, 7);


        }
        //--------------------------------------------------------------------------------------
        public void PermitationEmptyLastComparePermitationBase(int pSize, int pNumberOfPlace)
        {
            CPermitationEnumeration lEnumeration = new CPermitationEnumeration(pSize, pNumberOfPlace, lMaxDurability);
            lEnumeration.ExecuteBody();
            CPermitationEmptyLastEnumeration lEnumerationEmptyLast = new CPermitationEmptyLastEnumeration(pSize, pNumberOfPlace, lMaxDurability);
            lEnumerationEmptyLast.Execute();
            lEnumeration.ResultStringSet.SymmetricExceptWith(lEnumerationEmptyLast.ResultStringSet);
            if (lEnumeration.ResultStringSet.Count > 0)
                Assert.AreEqual(lEnumeration.ResultStringSet.Count, 0, "CPermitationEnumeration and CPermitationEmptyLastEnumeration results are not equal");
        }
        //--------------------------------------------------------------------------------------
        public void PermitationBaseEmptyLastComparePermitationEmptyLast(int pSize, int pNumberOfPlace)
        {
            CPermitationEnumeration lEnumeration = new CPermitationEnumeration(pSize, pNumberOfPlace, lMaxDurability);
            lEnumeration.ExecuteBody();
            CPermitationBaseEmptyLastEnumeration lEnumerationEmptyLast = new CPermitationBaseEmptyLastEnumeration(pSize, pNumberOfPlace, lMaxDurability);
            lEnumerationEmptyLast.ExecuteBody();
            lEnumeration.ResultStringSet.SymmetricExceptWith(lEnumerationEmptyLast.ResultStringSet);
            if (lEnumeration.ResultStringSet.Count > 0)
                Assert.AreEqual(lEnumeration.ResultStringSet.Count, 0, "CPermitationEnumeration and CPermitationEmptyLastEnumeration results are not equal");
        }
        //--------------------------------------------------------------------------------------
        public void PermitationBaseEmptyLastComparePermitationBase(int pSize, int pNumberOfPlace)
        {
            CPermitationEmptyLastEnumeration lEnumerationEmptyLast = new CPermitationEmptyLastEnumeration(pSize, pNumberOfPlace, lMaxDurability);
            lEnumerationEmptyLast.Execute();
            CPermitationBaseEmptyLastEnumeration lEnumerationBaseEmptyLast = new CPermitationBaseEmptyLastEnumeration(pSize, pNumberOfPlace, lMaxDurability);
            lEnumerationBaseEmptyLast.ExecuteBody();
            Assert.AreEqual(lEnumerationEmptyLast.ResultStringList.Count,
                lEnumerationBaseEmptyLast.ResultStringList.Count);
            var wrongList = Enumerable.Range(0, lEnumerationEmptyLast.ResultStringSet.Count)
                    .Where(i => lEnumerationEmptyLast.ResultStringList[i] !=
                            lEnumerationBaseEmptyLast.ResultStringList[i]).ToList();
            if (wrongList.Count > 0)
                Assert.AreEqual(wrongList.Count, 0, "CPermitationEmptyLastEnumeration and CPermitationBaseEmptyLastEnumeration results are not equal");
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
