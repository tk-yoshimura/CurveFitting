using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class GaussNewtonFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = { 1, 3, 4, 7 }, ys = new ddouble[xs.Length];
            Vector p = new(2, 3);

            static ddouble fitting_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                return b / (1 + ddouble.Exp((-a) * x));
            }

            static Vector fitting_diff_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                ddouble v = ddouble.Exp(-a * x) + 1;

                return new Vector(b * x * ddouble.Exp(-a * x) / (v * v), 1 / v);
            }

            for (int i = 0; i < xs.Length; i++) {
                ys[i] = fitting_func(xs[i], p);
            }

            GaussNewtonFitter fitter = new(xs, ys, new FittingFunction(2, fitting_func, fitting_diff_func));

            var v = fitter.ExecuteFitting(new Vector(3, 4));

            Assert.IsTrue((v - p).Norm < 1e-20);
        }

        [TestMethod()]
        public void ExecuteWeightedFittingTest() {
            ddouble[] xs = { 1, 3, 4, 7, 8 }, ys = new ddouble[xs.Length];
            ddouble[] ws = { 0.5, 0.75, 0, 0.75, 0.5 };
            Vector p = new(2, 3);

            static ddouble fitting_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                return b / (1 + ddouble.Exp((-a) * x));
            }

            static Vector fitting_diff_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                ddouble v = ddouble.Exp(-a * x) + 1;

                return new Vector(b * x * ddouble.Exp(-a * x) / (v * v), 1 / v);
            }

            for (int i = 0; i < xs.Length; i++) {
                ys[i] = fitting_func(xs[i], p);
            }
            ys[2] = 1e+8;

            GaussNewtonFitter fitter = new(xs, ys, new FittingFunction(2, fitting_func, fitting_diff_func));

            var v = fitter.ExecuteFitting(new Vector(3, 4), weights: ws);

            Assert.IsTrue((v - p).Norm < 1e-20);
        }
    }
}