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

                return new Vector((b * x * ddouble.Exp(-a * x)) / (v * v), 1 / v);
            }

            for (int i = 0; i < xs.Length; i++) {
                ys[i] = fitting_func(xs[i], p);
            }

            GaussNewtonFitter fitting = new(xs, ys, new FittingFunction(2, fitting_func, fitting_diff_func));

            var v = fitting.ExecuteFitting(new Vector(3, 4));

            Assert.IsTrue((v - p).Norm < 1e-20);
        }
    }
}