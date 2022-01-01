using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class LinearFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = { 2, 3 }, ys = { 1, 8 };

            LinearFitter fitting1 = new(xs, ys, enable_intercept: true);
            LinearFitter fitting2 = new(xs, ys, enable_intercept: false);

            Assert.AreEqual(new Vector(-13, 7), fitting1.ExecuteFitting());
            Assert.AreEqual(new Vector(2d), fitting2.ExecuteFitting());
        }
    }
}