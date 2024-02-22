using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CurveFitting.Tests {
    [TestClass()]
    public class RobustPadeFitterTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            ddouble[] xs = Vector.Arange(1024) / 1024;
            ddouble[] ys = Vector.Func(x => ddouble.Cos(x) - 0.25, xs);

            ys[512] = 256;

            RobustPadeFitter fitter = new(xs, ys, intercept: 0.75, numer: 4, denom: 3);

            Vector parameters = fitter.ExecuteFitting();

            Console.WriteLine($"Numer : {parameters[..fitter.Numer]}");
            Console.WriteLine($"Denom : {parameters[fitter.Numer..]}");

            Assert.AreEqual(0.75, fitter.FittingValue(0, parameters));

            for (int i = 0; i < xs.Length; i++) {
                if (i == 512) {
                    continue;
                }

                Assert.IsTrue(ddouble.Abs(ys[i] - fitter.FittingValue(xs[i], parameters)) < 1e-5,
                    $"\nexpected : {ys[i]}\n actual  : {fitter.FittingValue(xs[i], parameters)}"
                );
            }
        }
    }
}