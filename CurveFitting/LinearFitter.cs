using Algebra;
using DoubleDouble;
using System;
using System.Linq;

namespace CurveFitting {

    /// <summary>線形フィッティング</summary>
    public class LinearFitter : Fitter {

        private readonly SumTable sum_table;
        private readonly ddouble? intercept;

        /// <summary>コンストラクタ</summary>
        public LinearFitter(Vector xs, Vector ys, ddouble? intercept = null)
            : base(xs, (intercept is null) ? ys : ys.Select(y => y.val - intercept.Value).ToArray(),
                  parameters: 2) {

            this.sum_table = new(X, Y);
            this.intercept = intercept;
        }

        /// <summary>フィッティング値</summary>
        public override ddouble FittingValue(ddouble x, Vector parameters) {
            if (parameters.Dim != Parameters) {
                throw new ArgumentException("Illegal length.", nameof(parameters));
            }

            return parameters[0] + parameters[1] * x;
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(Vector? weights = null) {
            sum_table.W = weights;

            ddouble sum_wxx = sum_table[2, 0], sum_wxy = sum_table[1, 1];

            if (intercept is null) {
                ddouble sum_w = sum_table[0, 0], sum_wx = sum_table[1, 0], sum_wy = sum_table[0, 1];

                ddouble r = 1 / (sum_wx * sum_wx - sum_w * sum_wxx);
                ddouble a = (sum_wx * sum_wxy - sum_wxx * sum_wy) * r;
                ddouble b = (sum_wx * sum_wy - sum_w * sum_wxy) * r;

                return new Vector(a, b);
            }
            else {
                return new Vector(intercept.Value, sum_wxy / sum_wxx);
            }
        }
    }
}
