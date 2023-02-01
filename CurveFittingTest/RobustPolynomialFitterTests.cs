using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class RobustPolynomialFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);
            ddouble[] xs = Vector.Arange(64);
            ddouble[] ys1 = Vector.Polynomial(xs, p1), ys2 = Vector.Polynomial(xs, p2);

            ys1[32] = ys2[32] = -64;

            RobustPolynomialFitter fitter1 = new(xs, ys1, 3);
            RobustPolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

            Assert.IsTrue((fitter1.ExecuteFitting() - p1).Norm < 1e-20);
            Assert.IsTrue((fitter2.ExecuteFitting() - p2).Norm < 1e-20);
        }
    }
}