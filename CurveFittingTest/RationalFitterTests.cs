using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CurveFitting.Tests {
    [TestClass()]
    public class RationalFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = { -10, -8, -5, -2, -1, 0, 2, 3, 5, 6, 7, 9, 10 }, ys = new ddouble[xs.Length];

            for (int i = 0; i < xs.Length; i++) {
                ddouble x = xs[i];

                ys[i] = (3 * x + 4 * x * x + 5 * x * x * x) / (1 + 2 * x + 6 * x * x + 7 * x * x * x + 8 * x * x * x * x);
            }

            RationalFitter fitting = new(xs, ys, 3, 4);

            (Vector n, Vector d) = fitting.ExecuteFitting();

            ddouble[] ys_approx = fitting.FittingValue(xs, n, d);

            Assert.IsTrue((new Vector(ys) - new Vector(ys_approx)).Norm < 2.5e-2);
        }
    }
}