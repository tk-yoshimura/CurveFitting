using Algebra;
using System;
using System.Linq;

namespace CurveFitting {

    /// <summary>ロバスト線形フィッティング</summary>
    public class RobustLinearFittingMethod : FittingMethod {

        /// <summary>コンストラクタ</summary>
        public RobustLinearFittingMethod(double[] xs, double[] ys, bool is_enable_section)
            : base(xs, ys, is_enable_section ? 2 : 1) {

            IsEnableSection = is_enable_section;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; private set; }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != Parameters) {
                throw new ArgumentException(nameof(parameters));
            }

            if (IsEnableSection) {
                return parameters[0] + parameters[1] * x;
            }
            else {
                return parameters[0] * x;
            }
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(int converge_times = 8) {
            double err_threshold, inv_err;
            double[] xs = X.ToArray(), ys = Y.ToArray();
            double[] weights = new double[Points], errs = new double[Points], sort_err_list;
            Vector err, coef = null;
            WeightedLinearFittingMethod fitting;

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            while (converge_times > 0) {
                fitting = new WeightedLinearFittingMethod(xs, ys, weights, IsEnableSection);

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
