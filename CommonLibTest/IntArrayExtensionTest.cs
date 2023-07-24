using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArrayExtensions;

namespace CommonLibTest
{
    [TestClass]
    public class IntArrayExtensionTest
    {
        [TestMethod]
        public void InversePermitationTest()
        {
            // Arrange 
            int[] permitation = { 2, 0, 4, 1, 3 };
            int[] expextedinverse = { 1, 3, 0, 4, 2 };
            
            // Act 
            int[] reverse = permitation.InversePermitation();

            // Assert
            bool isInEqual = expextedinverse.Zip(reverse, (f, s) => f - s).Any(i => i != 0);
            Assert.IsFalse(isInEqual);
        }
        [TestMethod]
        public void InversePermitationFrom1Test()
        {
            // Arrange 
            int[] permitation = { 3, 1, 5, 2, 4 };
            int[] expextedinverse = { 1, 3, 0, 4, 2 };

            // Act 
            int[] reverse = permitation.InversePermitationFrom1();

            // Assert
            bool isInEqual = expextedinverse.Zip(reverse, (f, s) => f - s).Any(i => i != 0);
            Assert.IsFalse(isInEqual);
        }
    }
}
