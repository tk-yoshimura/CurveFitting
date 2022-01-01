using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class PolynomialFittingMethodTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = { 1, 3, 4, 7, 8, 9, 13, 15, 20 }, ys1 = new ddouble[xs.Length], ys2 = new ddouble[xs.Length];
            Vector p1 = new(2, -1, 1, 5), p2 = new(4, 3, -1);

            for (int i = 0; i < xs.Length; i++) {
                ddouble x = xs[i];

                ys1[i] = p1[0] + p1[1] * x + p1[2] * x * x + p1[3] * x * x * x;
                ys2[i] = p2[0] * x + p2[1] * x * x + p2[2] * x * x * x;
            }

            PolynomialFitter fitting1 = new(xs, ys1, 3, enable_intercept: true);
            PolynomialFitter fitting2 = new(xs, ys2, 3, enable_intercept: false);

            Assert.IsTrue((fitting1.ExecuteFitting() - p1).Norm < 1e-24);
            Assert.IsTrue((fitting2.ExecuteFitting() - p2).Norm < 1e-24);
        }
    }
}