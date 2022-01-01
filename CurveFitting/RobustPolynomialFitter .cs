using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {

    /// <summary>ロバスト多項式フィッティング</summary>
    public class RobustPolynomialFitter : Fitter {

        /// <summary>コンストラクタ</summary>
        public RobustPolynomialFitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, int degree, bool enable_intercept)
            : base(xs, ys, degree + (enable_intercept ? 1 : 0)) {

            this.Degree = degree;
            this.EnableIntercept = enable_intercept;
        }

        /// <summary>次数</summary>
        public int Degree {
            get; private set;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool EnableIntercept { get; private set; }

        /// <summary>フィッティング値</summary>
        public override ddouble FittingValue(ddouble x, Vector coefficients) {
            if (EnableIntercept) {
                ddouble y = coefficients[0], ploy_x = 1;

                for (int i = 1; i < coefficients.Dim; i++) {
                    ploy_x *= x;
                    y += ploy_x * coefficients[i];
                }

                return y;
            }
            else {
                ddouble y = 0, ploy_x = 1;

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
            IReadOnlyList<ddouble> xs = X, ys = Y;
            double[] weights = new double[Points], errs = new double[Points], sort_err_list;
            Vector err, coef = null;
            WeightedPolynomialFitter fitting;

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            while (converge_times > 0) {
                fitting = new WeightedPolynomialFitter(xs, ys, weights, Degree, EnableIntercept);

                coef = fitting.ExecuteFitting();

                err = fitting.Error(coef);

                for (int i = 0; i < Points; i++) {
                    errs[i] = Math.Abs((double)err[i]);
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
