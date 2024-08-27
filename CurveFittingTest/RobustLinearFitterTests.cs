using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class RobustLinearFitterTests {
        [TestMethod()]
        public void FitTest() {
            ddouble[] xs = Vector.Arange(64);
            ddouble[] ys = Vector.Func(x => -13 + x * 7, xs);

            ys[32] = 12;

            RobustLinearFitter fitter1 = new(xs, ys);
            RobustLinearFitter fitter2 = new(xs, ys, intercept: -13);

            Assert.IsTrue((fitter1.Fit() - new Vector(-13, 7)).Norm < 1e-24);
            Assert.IsTrue((fitter2.Fit() - new Vector(-13, 7)).Norm < 1e-24);
        }
    }
}