using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algebra;

namespace CurveFitting.Tests {
    [TestClass()]
    public class LinearFittingMethodTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            FittingData[] data_list = { new FittingData(2, 1), new FittingData(3, 8) };

            LinearFittingMethod fitting1 = new LinearFittingMethod(data_list, true);
            LinearFittingMethod fitting2 = new LinearFittingMethod(data_list, false);

            Assert.AreEqual(fitting1.ExecuteFitting(), new Vector(-13, 7));
            Assert.AreEqual(fitting2.ExecuteFitting(), new Vector(2));
        }
    }
}