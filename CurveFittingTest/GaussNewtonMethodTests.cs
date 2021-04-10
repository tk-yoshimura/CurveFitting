using Algebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CurveFitting.Tests {
    [TestClass()]
    public class GaussNewtonMethodTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            double[] xs = { 1, 3, 4, 7 }, ys = new double[xs.Length];
            Vector p = new(2, 3);

            static double fitting_func(double x, Vector parameter) {
                double a = parameter[0], b = parameter[1];

                return b / (1 + Math.Exp((-a) * x));
            }

            static Vector fitting_diff_func(double x, Vector parameter) {
                double a = parameter[0], b = parameter[1];

                double v = Math.Exp(-a * x) + 1;

                return new Vector((b * x * Math.Exp(-a * x)) / (v * v), 1 / v);
            }

            for (int i = 0; i < xs.Length; i++) {
                ys[i] = fitting_func(xs[i], p);
            }

            GaussNewtonMethod fitting = new(xs, ys, new FittingFunction(2, fitting_func, fitting_diff_func));

            Assert.AreEqual(0, (fitting.ExecuteFitting(new Vector(3, 4)) - p).Norm, 1e-10);
        }
    }
}