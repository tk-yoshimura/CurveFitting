using Algebra;
using System;

namespace CurveFitting {

    /// <summary>重み付き線形フィッティング</summary>
    public class WeightedLinearFittingMethod : FittingMethod {

        readonly double[] weight_list;

        /// <summary>コンストラクタ</summary>
        public WeightedLinearFittingMethod(double[] xs, double[] ys, double[] weights, bool is_enable_section)
            : base(xs, ys, is_enable_section ? 2 : 1) {

            IsEnableSection = is_enable_section;

            if (weights is null) {
                throw new ArgumentNullException(nameof(weights));
            }

            if (Points != weights.Length) {
                throw new ArgumentException($"{nameof(weights)}");
            }

            foreach (var weight in weights) {
                if (!(weight >= 0)) {
                    throw new ArgumentException(nameof(weights));
                }
            }

            this.weight_list = weights;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; private set; }

        /// <summary>重み付き誤差二乗和</summary>
        public double WeightedCost(Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != Parameters) {
                throw new ArgumentException(nameof(parameters));
            }

            Vector errors = Error(parameters);
            double cost = 0;
            for (int i = 0; i < errors.Dim; i++) {
                cost += weight_list[i] * errors[i] * errors[i];
            }

            return cost;
        }

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
        public Vector ExecuteFitting() {
            if (weight_list is null) {
                throw new InvalidOperationException();
            }

            if (IsEnableSection) {
                double w, sum_w = 0, sum_wx = 0, sum_wy = 0, sum_wxx = 0, sum_wxy = 0;

                for (int i = 0; i < Points; i++) {
                    double x = X[i], y = Y[i];

                    w = weight_list[i];
                    sum_w += w;
                    sum_wx += w * x;
                    sum_wy += w * y;
                    sum_wxx += w * x * x;
                    sum_wxy += w * x * y;
                }

                double r = 1 / (sum_wx * sum_wx - sum_w * sum_wxx);
                double a = (sum_wx * sum_wxy - sum_wxx * sum_wy) * r;
                double b = (sum_wx * sum_wy - sum_w * sum_wxy) * r;

                return new Vector(a, b);
            }
            else {
                double w, sum_wxx = 0, sum_wxy = 0;

                for (int i = 0; i < Points; i++) {
                    double x = X[i], y = Y[i];

                    w = weight_list[i];
                    sum_wxx += w * x * x;
                    sum_wxy += w * x * y;
                }

                return new Vector(sum_wxy / sum_wxx);
            }
        }
    }
}
