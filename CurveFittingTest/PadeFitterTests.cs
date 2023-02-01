using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CurveFitting.Tests {
    [TestClass()]
    public class PadeFitterTests {
        [TestMethod()]
        public void ExecuteFittingWithInterceptTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);

            PadeFitter fitter = new(xs, ys, intercept: 0.75, numer: 4, denom: 3);

            Vector parameters = fitter.ExecuteFitting();

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            Assert.AreEqual(0.75, fitter.FittingValue(0, parameters));

            for (int i = 0; i < xs.Length; i++) {
                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-5,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void ExecuteFittingWithoutInterceptTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);

            PadeFitter fitter = new(xs, ys, numer: 4, denom: 3);

            Vector parameters = fitter.ExecuteFitting();

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            for (int i = 0; i < xs.Length; i++) {
                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-5,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void ExecuteWeightedFittingWithInterceptTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);
            ddouble[] ws = Vector.Fill(xs.Length, value: 0.5);

            ys[256] = 1e+8;
            ws[256] = 0;

            PadeFitter fitter = new(xs, ys, intercept: 0.75, numer: 4, denom: 3);

            Vector parameters = fitter.ExecuteFitting(ws);

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            Assert.AreEqual(0.75, fitter.FittingValue(0, parameters));

            for (int i = 0; i < xs.Length; i++) {
                if (i == 256) {
                    continue;
                }

                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-5,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void ExecuteWeightedFittingWithoutInterceptTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);
            ddouble[] ws = Vector.Fill(xs.Length, value: 0.5);

            ys[256] = 1e+8;
            ws[256] = 0;

            PadeFitter fitter = new(xs, ys, numer: 4, denom: 3);

            Vector parameters = fitter.ExecuteFitting(ws);

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            for (int i = 0; i < xs.Length; i++) {
                if (i == 256) {
                    continue;
                }

                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-5,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void ExecuteFittingWithInterceptWithCostTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);

            PadeFitter fitter = new(xs, ys, intercept: 0.75, numer: 4, denom: 3);

            Assert.IsTrue(fitter.ExecuteFitting().Norm > fitter.ExecuteFitting(norm_cost: 1e-8).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(norm_cost: 1e-8).Norm > fitter.ExecuteFitting(norm_cost: 1e-4).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(norm_cost: 1e-4).Norm > fitter.ExecuteFitting(norm_cost: 1e-2).Norm);

            Vector parameters = fitter.ExecuteFitting(norm_cost: 1e-8);

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            Assert.AreEqual(0.75, fitter.FittingValue(0, parameters));

            for (int i = 0; i < xs.Length; i++) {
                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-4,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void ExecuteFittingWithoutInterceptWithCostTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);

            PadeFitter fitter = new(xs, ys, numer: 4, denom: 3);

            Assert.IsTrue(fitter.ExecuteFitting().Norm > fitter.ExecuteFitting(norm_cost: 1e-8).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(norm_cost: 1e-8).Norm > fitter.ExecuteFitting(norm_cost: 1e-4).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(norm_cost: 1e-4).Norm > fitter.ExecuteFitting(norm_cost: 1e-2).Norm);

            Vector parameters = fitter.ExecuteFitting(norm_cost: 1e-8);

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            for (int i = 0; i < xs.Length; i++) {
                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-4,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void ExecuteWeightedFittingWithInterceptWithCostTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);
            ddouble[] ws = Vector.Fill(xs.Length, value: 0.5);

            ys[256] = 1e+8;
            ws[256] = 0;

            PadeFitter fitter = new(xs, ys, intercept: 0.75, numer: 4, denom: 3);

            Assert.IsTrue(fitter.ExecuteFitting(ws).Norm > fitter.ExecuteFitting(ws, norm_cost: 1e-8).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(ws, norm_cost: 1e-8).Norm > fitter.ExecuteFitting(ws, norm_cost: 1e-4).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(ws, norm_cost: 1e-4).Norm > fitter.ExecuteFitting(ws, norm_cost: 1e-2).Norm);

            Vector parameters = fitter.ExecuteFitting(ws, norm_cost: 1e-8);

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            Assert.AreEqual(0.75, fitter.FittingValue(0, parameters));

            for (int i = 0; i < xs.Length; i++) {
                if (i == 256) {
                    continue;
                }

                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-4,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void ExecuteWeightedFittingWithoutInterceptWithCostTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(xs, x => ddouble.Cos(x) - 0.25);
            ddouble[] ws = Vector.Fill(xs.Length, value: 0.5);

            ys[256] = 1e+8;
            ws[256] = 0;

            PadeFitter fitter = new(xs, ys, numer: 4, denom: 3);

            Assert.IsTrue(fitter.ExecuteFitting(ws).Norm > fitter.ExecuteFitting(ws, norm_cost: 1e-8).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(ws, norm_cost: 1e-8).Norm > fitter.ExecuteFitting(ws, norm_cost: 1e-4).Norm);
            Assert.IsTrue(fitter.ExecuteFitting(ws, norm_cost: 1e-4).Norm > fitter.ExecuteFitting(ws, norm_cost: 1e-2).Norm);

            Vector parameters = fitter.ExecuteFitting(ws, norm_cost: 1e-8);

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            for (int i = 0; i < xs.Length; i++) {
                if (i == 256) {
                    continue;
                }

                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-4,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }

        [TestMethod()]
        public void GenerateTableTest() {
            (ddouble x, ddouble y)[] vs = new (ddouble, ddouble)[] {
                (2, 11), (3, 13), (5, 17), (7, 19)
            };

            ddouble s(int xn, int yn) {
                return vs.Select(v => ddouble.Pow(v.x, xn) * ddouble.Pow(v.y, yn)).Sum();
            };

            SumTable table = new(vs.Select(v => v.x).ToArray(), vs.Select(v => v.y).ToArray());

            (Matrix m, Vector v) = PadeFitter.GenerateTable(table, 5, 4);

            Assert.AreEqual(m, m.Transpose);

            Assert.AreEqual(s(0, 0), m[0, 0]);
            Assert.AreEqual(s(1, 0), m[1, 0]);
            Assert.AreEqual(s(2, 0), m[2, 0]);
            Assert.AreEqual(s(3, 0), m[3, 0]);
            Assert.AreEqual(s(4, 0), m[4, 0]);
            Assert.AreEqual(-s(1, 1), m[5, 0]);
            Assert.AreEqual(-s(2, 1), m[6, 0]);
            Assert.AreEqual(-s(3, 1), m[7, 0]);

            Assert.AreEqual(s(2, 0), m[1, 1]);
            Assert.AreEqual(s(3, 0), m[2, 1]);
            Assert.AreEqual(s(4, 0), m[3, 1]);
            Assert.AreEqual(s(5, 0), m[4, 1]);
            Assert.AreEqual(-s(2, 1), m[5, 1]);
            Assert.AreEqual(-s(3, 1), m[6, 1]);
            Assert.AreEqual(-s(4, 1), m[7, 1]);

            Assert.AreEqual(s(4, 0), m[2, 2]);
            Assert.AreEqual(s(5, 0), m[3, 2]);
            Assert.AreEqual(s(6, 0), m[4, 2]);
            Assert.AreEqual(-s(3, 1), m[5, 2]);
            Assert.AreEqual(-s(4, 1), m[6, 2]);
            Assert.AreEqual(-s(5, 1), m[7, 2]);

            Assert.AreEqual(s(6, 0), m[3, 3]);
            Assert.AreEqual(s(7, 0), m[4, 3]);
            Assert.AreEqual(-s(4, 1), m[5, 3]);
            Assert.AreEqual(-s(5, 1), m[6, 3]);
            Assert.AreEqual(-s(6, 1), m[7, 3]);

            Assert.AreEqual(s(8, 0), m[4, 4]);
            Assert.AreEqual(-s(5, 1), m[5, 4]);
            Assert.AreEqual(-s(6, 1), m[6, 4]);
            Assert.AreEqual(-s(7, 1), m[7, 4]);

            Assert.AreEqual(s(2, 2), m[5, 5]);
            Assert.AreEqual(s(3, 2), m[6, 5]);
            Assert.AreEqual(s(4, 2), m[7, 5]);

            Assert.AreEqual(s(4, 2), m[6, 6]);
            Assert.AreEqual(s(5, 2), m[7, 6]);

            Assert.AreEqual(s(6, 2), m[7, 7]);

            Assert.AreEqual(s(0, 1), v[0]);
            Assert.AreEqual(s(1, 1), v[1]);
            Assert.AreEqual(s(2, 1), v[2]);
            Assert.AreEqual(s(3, 1), v[3]);
            Assert.AreEqual(s(4, 1), v[4]);
            Assert.AreEqual(-s(1, 2), v[5]);
            Assert.AreEqual(-s(2, 2), v[6]);
            Assert.AreEqual(-s(3, 2), v[7]);
        }
    }
}