using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CurveFitting.Tests {
    [TestClass()]
    public class RobustPolynomialFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = (new ddouble[64]).Select((_, i) => (ddouble)i).ToArray();
            ddouble[] ys1 = new ddouble[xs.Length], ys2 = new ddouble[xs.Length];
            Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);

            for (int i = 0; i < xs.Length; i++) {
                ddouble x = xs[i];

                ys1[i] = p1[0] + p1[1] * x + p1[2] * x * x + p1[3] * x * x * x;
                ys2[i] = p2[0] + p2[1] * x + p2[2] * x * x + p2[3] * x * x * x;
            }
            ys1[32] = ys2[32] = -64;

            RobustPolynomialFitter fitter1 = new(xs, ys1, 3);
            RobustPolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

            Assert.IsTrue((fitter1.ExecuteFitting() - p1).Norm < 1e-20);
            Assert.IsTrue((fitter2.ExecuteFitting() - p2).Norm < 1e-20);
        }
    }
}