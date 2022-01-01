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

            RationalFitter fitting1 = new(xs, ys, 3, 4, enable_intercept: true);
            RationalFitter fitting2 = new(xs, ys, 3, 4, enable_intercept: false);

            Vector p1 = fitting1.ExecuteFitting();
            Vector p2 = fitting2.ExecuteFitting();

            ddouble[] ys1 = fitting1.FittingValue(xs, p1);
            ddouble[] ys2 = fitting2.FittingValue(xs, p2);

            Assert.IsTrue((new Vector(ys) - new Vector(ys1)).Norm < 2.5e-2);
            Assert.IsTrue((new Vector(ys) - new Vector(ys2)).Norm < 2.5e-2);
        }
    }
}