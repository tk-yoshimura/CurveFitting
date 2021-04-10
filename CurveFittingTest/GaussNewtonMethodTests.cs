﻿using Algebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CurveFitting.Tests {
    [TestClass()]
    public class GaussNewtonMethodTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            double[] x_list = { 1, 3, 4, 7 };
            Vector p = new(2, 3);

            Func<double, Vector, double> fitting_func = (x, parameter) => {
                double a = parameter[0], b = parameter[1];

                return b / (1 + Math.Exp((-a) * x));
            };

            Func<double, Vector, Vector> fitting_diff_func = (x, parameter) => {
                double a = parameter[0], b = parameter[1];

                double v = Math.Exp(-a * x) + 1;

                return new Vector((b * x * Math.Exp(-a * x)) / (v * v), 1 / v);
            };

            FittingData[] data_list = new FittingData[x_list.Length];

            for (int i = 0; i < x_list.Length; i++) {
                double x = x_list[i];

                data_list[i].X = x;
                data_list[i].Y = fitting_func(x, p);
            }

            GaussNewtonMethod fitting = new(data_list, new FittingFunction(2, fitting_func, fitting_diff_func));

            Assert.AreEqual(0, (fitting.ExecuteFitting(new Vector(3, 4)) - p).Norm, 1e-10);
        }
    }
}