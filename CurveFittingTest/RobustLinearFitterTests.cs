using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CurveFitting.Tests {
    [TestClass()]
    public class RobustLinearFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = (new ddouble[64]).Select((_, i) => (ddouble)i).ToArray();
            ddouble[] ys = xs.Select(x => -13 + x * 7).ToArray();

            ys[32] = 12;

            RobustLinearFitter fitter1 = new(xs, ys);
            RobustLinearFitter fitter2 = new(xs, ys, intercept: -13);

            Assert.IsTrue((fitter1.ExecuteFitting() - new Vector(-13, 7)).Norm < 1e-24);
            Assert.IsTrue((fitter2.ExecuteFitting() - new Vector(-13, 7)).Norm < 1e-24);
        }
    }
}