using Algebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class LinearFittingMethodTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            FittingData[] data_list = { new FittingData(2, 1), new FittingData(3, 8) };

            LinearFittingMethod fitting1 = new(data_list, true);
            LinearFittingMethod fitting2 = new(data_list, false);

            Assert.AreEqual(new Vector(-13, 7), fitting1.ExecuteFitting());
            Assert.AreEqual(new Vector(2), fitting2.ExecuteFitting());
        }
    }
}