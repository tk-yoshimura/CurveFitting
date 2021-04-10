using Algebra;
using System;

namespace CurveFitting {

    /// <summary>ロバスト線形フィッティング</summary>
    public class RobustLinearFittingMethod : FittingMethod {

        /// <summary>コンストラクタ</summary>
        public RobustLinearFittingMethod(FittingData[] data_list, bool is_enable_section)
            : base(data_list, is_enable_section ? 2 : 1) {

            IsEnableSection = is_enable_section;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; private set; }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != ParametersCount) {
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
            int n = data_list.Length;
            double err_threshold, inv_err;
            double[] weight_list = new double[n], err_list = new double[n], sort_err_list;
            Vector err, coef = null;
            WeightedLinearFittingMethod fitting;

            for (int i = 0; i < n; i++) {
                weight_list[i] = 1;
            }

            while (converge_times > 0) {
                fitting = new WeightedLinearFittingMethod(data_list, weight_list, IsEnableSection);

                coef = fitting.ExecuteFitting();

                err = fitting.Error(coef);

                for (int i = 0; i < n; i++) {
                    err_list[i] = Math.Abs(err[i]);
                }

                sort_err_list = (double[])err_list.Clone();

                Array.Sort(sort_err_list);

                err_threshold = sort_err_list[n / 2] * 1.25;
                if (err_threshold <= 1e-14) {
                    break;
                }

                inv_err = 1 / err_threshold;

                for (int i = 0; i < n; i++) {
                    if (err_list[i] >= err_threshold) {
                        weight_list[i] = 0;
                    }
                    else {
                        double r = (1 - err_list[i] * inv_err);
                        weight_list[i] = r * r;
                    }
                }

                converge_times--;
            }

            return coef;
        }
    }
}
