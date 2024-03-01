using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CurveFitting.Tests {
    [TestClass()]
    public class CurveFittingUtilsTests {
        [TestMethod()]
        public void EnumeratePadeDegreeTest() {
            for (int coef_counts = 4; coef_counts <= 16; coef_counts++) {
                for (int degree_delta = 0; degree_delta <= 8; degree_delta++) {
                    Console.WriteLine($"{nameof(coef_counts)} = {coef_counts}");
                    Console.WriteLine($"{nameof(degree_delta)} = {degree_delta}");

                    foreach ((int m, int n) in CurveFittingUtils.EnumeratePadeDegree(coef_counts, degree_delta)) {
                        Console.WriteLine($"{m},{n}");

                        Assert.AreEqual(coef_counts, m + n);
                        Assert.IsTrue(Math.Abs(m - n) <= degree_delta);
                        Assert.IsTrue(m > 1);
                        Assert.IsTrue(n > 1);
                    }
                }
            }
        }

        [TestMethod()]
        public void HasLossDigitsPolynomialCoefTest() {
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(Vector.Zero(8), -0.25, 0.25));

            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1), 0, 0.26));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1), -0.26, 0));

            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(0, 1, -1), 0, 0.26));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(0, 1, -1), -0.26, 0));

            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1), 0, 0.24));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1), -0.24, 0));

            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(0, 1, -1), 0, 0.24));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(0, 1, -1), -0.24, 0));

            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 0, -3), 0, 0.3));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 0, -2), 0, 0.3));

            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 0, -3), -0.3, 0));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 0, -2), -0.3, 0));

            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1 / 16d, -1 / 32d, -1 / 64d), 0, 2));
            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1.1 / 8d, -1.1 / 16d, -1.1 / 32d), 0, 2));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1 / 16d, -1 / 32d, -1 / 64d, -0.9 / 128d), 0, 2));
            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1 / 16d, -1 / 32d, -1 / 64d, -1.1 / 128d), 0, 2));

            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1 / 16d, 1 / 32d, 1 / 64d), 0, 2));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1.1 / 8d, 1.1 / 16d, 1.1 / 32d), 0, 2));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1 / 16d, 1 / 32d, 1 / 64d, 0.9 / 128d), 0, 2));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1 / 16d, 1 / 32d, 1 / 64d, 1.1 / 128d), 0, 2));

            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1 / 16d, 1 / 32d, -1 / 64d), -2, 0));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1.1 / 8d, 1.1 / 16d, -1.1 / 32d), -2, 0));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1 / 16d, 1 / 32d, -1 / 64d, 0.9 / 128d), -2, 0));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, -1 / 16d, 1 / 32d, -1 / 64d, 1.1 / 128d), -2, 0));

            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1 / 16d, -1 / 32d, 1 / 64d), -2, 0));
            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1.1 / 8d, -1.1 / 16d, 1.1 / 32d), -2, 0));
            Assert.IsFalse(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1 / 16d, -1 / 32d, 1 / 64d, -0.9 / 128d), -2, 0));
            Assert.IsTrue(CurveFittingUtils.HasLossDigitsPolynomialCoef(new Vector(1, 1 / 16d, -1 / 32d, 1 / 64d, -1.1 / 128d), -2, 0));
        }

        [TestMethod()]
        public void RelativeErrorTest() {
            Assert.AreEqual(0, CurveFittingUtils.RelativeError(0, 0));

            Assert.IsTrue(ddouble.IsPositiveInfinity(CurveFittingUtils.RelativeError(0, 1)));

            Assert.AreEqual(1, CurveFittingUtils.RelativeError(1, 0));

            Assert.AreEqual(0.5, CurveFittingUtils.RelativeError(1, 1.5));
            Assert.AreEqual(0.25, CurveFittingUtils.RelativeError(2, 1.5));

            Assert.AreEqual(0.5, CurveFittingUtils.RelativeError(-1, -1.5));
            Assert.AreEqual(0.25, CurveFittingUtils.RelativeError(-2, -1.5));
        }

        [TestMethod()]
        public void AbsoluteErrorTest() {
            Assert.AreEqual(0, CurveFittingUtils.AbsoluteError(0, 0));

            Assert.AreEqual(1, CurveFittingUtils.AbsoluteError(0, 1));

            Assert.AreEqual(1, CurveFittingUtils.AbsoluteError(1, 0));

            Assert.AreEqual(0.5, CurveFittingUtils.AbsoluteError(1, 1.5));
            Assert.AreEqual(0.5, CurveFittingUtils.AbsoluteError(2, 1.5));

            Assert.AreEqual(0.5, CurveFittingUtils.AbsoluteError(-1, -1.5));
            Assert.AreEqual(0.5, CurveFittingUtils.AbsoluteError(-2, -1.5));
        }

        [TestMethod()]
        public void MaxRelativeErrorTest() {
            Assert.AreEqual(1, CurveFittingUtils.MaxRelativeError(new Vector(2, 3, 4), new Vector(4, 3, 2)));
        }

        [TestMethod()]
        public void MaxAbsoluteErrorTest() {
            Assert.AreEqual(2, CurveFittingUtils.MaxAbsoluteError(new Vector(2, 3, 4), new Vector(4, 3, 2)));
        }

        [TestMethod()]
        public void EnumeratePadeCoefTest() {
            CollectionAssert.AreEqual(
                new (ddouble, ddouble)[] { (1, 4), (2, 5), (3, 6) },
                CurveFittingUtils.EnumeratePadeCoef(new Vector(1, 2, 3, 4, 5, 6), 3, 3).ToArray()
            );

            CollectionAssert.AreEqual(
                new (ddouble, ddouble)[] { (1, 5), (2, 6), (3, 0), (4, 0) },
                CurveFittingUtils.EnumeratePadeCoef(new Vector(1, 2, 3, 4, 5, 6), 4, 2).ToArray()
            );

            CollectionAssert.AreEqual(
                new (ddouble, ddouble)[] { (1, 3), (2, 4), (0, 5), (0, 6) },
                CurveFittingUtils.EnumeratePadeCoef(new Vector(1, 2, 3, 4, 5, 6), 2, 4).ToArray()
            );

            CollectionAssert.AreEqual(
                new (ddouble, ddouble)[] { (1, 6), (2, 0), (3, 0), (4, 0), (5, 0) },
                CurveFittingUtils.EnumeratePadeCoef(new Vector(1, 2, 3, 4, 5, 6), 5, 1).ToArray()
            );

            CollectionAssert.AreEqual(
                new (ddouble, ddouble)[] { (1, 2), (0, 3), (0, 4), (0, 5), (0, 6) },
                CurveFittingUtils.EnumeratePadeCoef(new Vector(1, 2, 3, 4, 5, 6), 1, 5).ToArray()
            );

            CollectionAssert.AreEqual(
                new (ddouble, ddouble)[] { (1, 5), (2, 0), (3, 0), (4, 0) },
                CurveFittingUtils.EnumeratePadeCoef(new Vector(1, 2, 3, 4, 5), 4, 1).ToArray()
            );

            CollectionAssert.AreEqual(
                new (ddouble, ddouble)[] { (1, 2), (0, 3), (0, 4), (0, 5) },
                CurveFittingUtils.EnumeratePadeCoef(new Vector(1, 2, 3, 4, 5), 1, 4).ToArray()
            );
        }

        [TestMethod()]
        public void StandardizeExponentTest() {
            Vector v = new double[] { 0, 0.125, 0.25, -0.125 };

            (long exp_scale, Vector u) = CurveFittingUtils.StandardizeExponent(v);

            Assert.AreEqual(-2, exp_scale);
            Assert.AreEqual(new Vector(0, 0.5, 1, -0.5), u);

            Assert.ThrowsException<ArgumentException>(() => {
                _ = CurveFittingUtils.StandardizeExponent(new double[] { 0, 0, 0 });
            });

            Assert.ThrowsException<ArgumentException>(() => {
                _ = CurveFittingUtils.StandardizeExponent(new double[] { 1, 1, double.PositiveInfinity });
            });

            Assert.ThrowsException<ArgumentException>(() => {
                _ = CurveFittingUtils.StandardizeExponent(new double[] { 1, 1, double.NaN });
            });
        }
    }
}