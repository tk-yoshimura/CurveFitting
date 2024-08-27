using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class PolynomialFitterTests {
        [TestMethod()]
        public void FitTest() {
            Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);
            ddouble[] xs = [1, 3, 4, 7, 8, 9, 13, 15, 20];
            ddouble[] ys1 = Vector.Polynomial(xs, p1), ys2 = Vector.Polynomial(xs, p2);

            PolynomialFitter fitter1 = new(xs, ys1, 3);
            PolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

            Assert.IsTrue((fitter1.Fit() - p1).Norm < 1e-24);
            Assert.IsTrue((fitter2.Fit() - p2).Norm < 1e-24);

            Assert.IsTrue(fitter1.Error(fitter1.Fit()).Norm < 1e-24);
            Assert.IsTrue(fitter2.Error(fitter2.Fit()).Norm < 1e-24);
        }

        [TestMethod()]
        public void ExecuteWeightedFittingTest() {
            Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);
            ddouble[] xs = [1, 3, 4, 7, 8, 9, 13, 15, 20];
            ddouble[] ys1 = Vector.Polynomial(xs, p1), ys2 = Vector.Polynomial(xs, p2);
            ddouble[] ws = [0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0];

            ys1[^1] = ys2[^1] = 1e+8;

            PolynomialFitter fitter1 = new(xs, ys1, 3);
            PolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

            Assert.IsTrue((fitter1.Fit(ws) - p1).Norm < 1e-24);
            Assert.IsTrue((fitter2.Fit(ws) - p2).Norm < 1e-24);
        }
    }
}