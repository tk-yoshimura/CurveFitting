﻿using Algebra;
using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class LevenbergMarquardtFitterTests {
        [TestMethod()]
        public void FitTest() {
            static ddouble fitting_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                return b / (1 + ddouble.Exp((-a) * x));
            }

            static Vector fitting_diff_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                ddouble v = ddouble.Exp(-a * x) + 1;

                return new Vector(b * x * ddouble.Exp(-a * x) / (v * v), 1 / v);
            }

            Vector p = new(2, 3);
            ddouble[] xs = [1, 3, 4, 7];
            ddouble[] ys = Vector.Func(x => fitting_func(x, p), xs);

            LevenbergMarquardtFitter fitter = new(xs, ys, new FittingFunction(2, fitting_func, fitting_diff_func));

            var v = fitter.Fit(new Vector(7, 2));

            Assert.IsTrue((v - p).Norm < 1e-20);
        }

        [TestMethod()]
        public void ExecuteWeightedFittingTest() {
            static ddouble fitting_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                return b / (1 + ddouble.Exp((-a) * x));
            }

            static Vector fitting_diff_func(ddouble x, Vector parameter) {
                ddouble a = parameter[0], b = parameter[1];

                ddouble v = ddouble.Exp(-a * x) + 1;

                return new Vector(b * x * ddouble.Exp(-a * x) / (v * v), 1 / v);
            }

            Vector p = new(2, 3);
            ddouble[] xs = [1, 3, 4, 7, 8];
            ddouble[] ys = Vector.Func(x => fitting_func(x, p), xs);
            ys[2] = 1e+8;

            ddouble[] ws = [0.5, 0.75, 0, 0.75, 0.5];

            LevenbergMarquardtFitter fitter = new(xs, ys, new FittingFunction(2, fitting_func, fitting_diff_func));

            var v = fitter.Fit(new Vector(3, 4), weights: ws);

            Assert.IsTrue((v - p).Norm < 1e-20);
        }
    }
}