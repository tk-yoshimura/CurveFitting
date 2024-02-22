using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class LinearFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = [2, 3], ys = [1, 8];

            LinearFitter fitter1 = new(xs, ys);
            LinearFitter fitter2 = new(xs, ys, intercept: 0);
            LinearFitter fitter3 = new(xs, ys, intercept: -13);

            Assert.AreEqual(new Vector(-13, 7), fitter1.ExecuteFitting());
            Assert.AreEqual(new Vector(0, 2), fitter2.ExecuteFitting());
            Assert.AreEqual(new Vector(-13, 7), fitter3.ExecuteFitting());

            Assert.AreEqual(new Vector(0, 0), fitter1.Error(fitter1.ExecuteFitting()));
            Assert.AreEqual(new Vector(0, 0), fitter3.Error(fitter3.ExecuteFitting()));
        }

        [TestMethod()]
        public void ExecuteWeightedFittingTest() {
            ddouble[] xs = [2, 3, 4], ys = [1, 8, 1e+8], ws = [0.5, 0.5, 0];

            LinearFitter fitter1 = new(xs, ys);
            LinearFitter fitter2 = new(xs, ys, intercept: 0);
            LinearFitter fitter3 = new(xs, ys, intercept: -13);

            Assert.AreEqual(new Vector(-13, 7), fitter1.ExecuteFitting(ws));
            Assert.AreEqual(new Vector(0, 2), fitter2.ExecuteFitting(ws));
            Assert.AreEqual(new Vector(-13, 7), fitter3.ExecuteFitting(ws));
        }
    }
}