using Algebra;
using System;

namespace CurveFitting {

    /// <summary>線形フィッティング</summary>
    public class LinearFittingMethod : FittingMethod {

        /// <summary>コンストラクタ</summary>
        public LinearFittingMethod(double[] xs, double[] ys, bool enable_intercept)
            : base(xs, ys, enable_intercept ? 2 : 1) {

            EnableIntercept = enable_intercept;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool EnableIntercept { get; private set; }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != Parameters) {
                throw new ArgumentException(nameof(parameters));
            }

            if (EnableIntercept) {
                return parameters[0] + parameters[1] * x;
            }
            else {
                return parameters[0] * x;
            }
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting() {
            if (EnableIntercept) {
                double sum_x = 0, sum_y = 0, sum_sq_x = 0, sum_xy = 0, n = Points;

                for (int i = 0; i < Points; i++) {
                    double x = X[i], y = Y[i];

                    sum_x += x;
                    sum_y += y;
                    sum_sq_x += x * x;
                    sum_xy += x * y;
                }

                double r = 1 / (sum_x * sum_x - n * sum_sq_x);
                double a = (sum_x * sum_xy - sum_sq_x * sum_y) * r;
                double b = (sum_x * sum_y - n * sum_xy) * r;

                return new Vector(a, b);
            }
            else {
                double sum_sq_x = 0, sum_xy = 0;

                for (int i = 0; i < Points; i++) {
                    double x = X[i], y = Y[i];

                    sum_sq_x += x * x;
                    sum_xy += x * y;
                }

                return new Vector(sum_xy / sum_sq_x);
            }
        }

    }
}
