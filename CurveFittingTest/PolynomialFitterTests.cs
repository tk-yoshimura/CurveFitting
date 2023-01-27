using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class PolynomialFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = { 1, 3, 4, 7, 8, 9, 13, 15, 20 };
            ddouble[] ys1 = new ddouble[xs.Length], ys2 = new ddouble[xs.Length];
            Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);

            for (int i = 0; i < xs.Length; i++) {
                ddouble x = xs[i];

                ys1[i] = p1[0] + p1[1] * x + p1[2] * x * x + p1[3] * x * x * x;
                ys2[i] = p2[0] + p2[1] * x + p2[2] * x * x + p2[3] * x * x * x;
            }

            PolynomialFitter fitter1 = new(xs, ys1, 3);
            PolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

            Assert.IsTrue((fitter1.ExecuteFitting() - p1).Norm < 1e-24);
            Assert.IsTrue((fitter2.ExecuteFitting() - p2).Norm < 1e-24);
        }

        [TestMethod()]
        public void ExecuteWeightedFittingTest() {
            ddouble[] xs = { 1, 3, 4, 7, 8, 9, 13, 15, 20 };
            ddouble[] ys1 = new ddouble[xs.Length], ys2 = new ddouble[xs.Length];
            ddouble[] ws = { 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0 };
            Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);

            for (int i = 0; i < xs.Length; i++) {
                ddouble x = xs[i];

                ys1[i] = p1[0] + p1[1] * x + p1[2] * x * x + p1[3] * x * x * x;
                ys2[i] = p2[0] + p2[1] * x + p2[2] * x * x + p2[3] * x * x * x;
            }

            ys1[^1] = ys2[^1] = 1e+8;

            PolynomialFitter fitter1 = new(xs, ys1, 3);
            PolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

            Assert.IsTrue((fitter1.ExecuteFitting(ws) - p1).Norm < 1e-24);
            Assert.IsTrue((fitter2.ExecuteFitting(ws) - p2).Norm < 1e-24);
        }
    }
}