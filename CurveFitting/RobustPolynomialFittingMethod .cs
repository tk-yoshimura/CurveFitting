﻿using Algebra;
using System;
using System.Linq;

namespace CurveFitting {

    /// <summary>ロバスト多項式フィッティング</summary>
    public class RobustPolynomialFittingMethod : FittingMethod {

        /// <summary>コンストラクタ</summary>
        public RobustPolynomialFittingMethod(double[] xs, double[] ys, int degree, bool is_enable_section)
            : base(xs, ys, degree + (is_enable_section ? 1 : 0)) {

            this.Degree = degree;
            this.IsEnableSection = is_enable_section;
        }

        /// <summary>次数</summary>
        public int Degree {
            get; private set;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; private set; }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector coefficients) {
            if (IsEnableSection) {
                double y = coefficients[0], ploy_x = 1;

                for (int i = 1; i < coefficients.Dim; i++) {
                    ploy_x *= x;
                    y += ploy_x * coefficients[i];
                }

                return y;
            }
            else {
                double y = 0, ploy_x = 1;

                for (int i = 0; i < coefficients.Dim; i++) {
                    ploy_x *= x;
                    y += ploy_x * coefficients[i];
                }

                return y;
            }
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(int converge_times = 8) {
            double err_threshold, inv_err;
            double[] xs = X.ToArray(), ys = Y.ToArray();
            double[] weights = new double[Points], errs = new double[Points], sort_err_list;
            Vector err, coef = null;
            WeightedPolynomialFittingMethod fitting;

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            while (converge_times > 0) {
                fitting = new WeightedPolynomialFittingMethod(xs, ys, weights, Degree, IsEnableSection);

                coef = fitting.ExecuteFitting();

                err = fitting.Error(coef);

                for (int i = 0; i < Points; i++) {
                    errs[i] = Math.Abs(err[i]);
                }

                sort_err_list = (double[])errs.Clone();

                Array.Sort(sort_err_list);

                err_threshold = sort_err_list[Points / 2] * 1.25;
                if (err_threshold <= 1e-14) {
                    break;
                }

                inv_err = 1 / err_threshold;

                for (int i = 0; i < Points; i++) {
                    if (errs[i] >= err_threshold) {
                        weights[i] = 0;
                    }
                    else {
                        double r = (1 - errs[i] * inv_err);
                        weights[i] = r * r;
                    }
                }

                converge_times--;
            }

            return coef;
        }
    }
}
