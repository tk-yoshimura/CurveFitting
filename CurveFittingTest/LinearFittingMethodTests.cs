using Algebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class LinearFittingMethodTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            double[] xs = { 2, 3 }, ys = { 1, 8 };

            LinearFittingMethod fitting1 = new(xs, ys, is_enable_section: true);
            LinearFittingMethod fitting2 = new(xs, ys, is_enable_section: false);

            Assert.AreEqual(new Vector(-13, 7), fitting1.ExecuteFitting());
            Assert.AreEqual(new Vector(2), fitting2.ExecuteFitting());
        }
    }
}