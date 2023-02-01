using Algebra;
using DoubleDouble;
using System;

namespace CurveFitting {

    /// <summary>ロバストパデフィッティング</summary>
    public class RobustPadeFitter : PadeFitter {

        /// <summary>コンストラクタ</summary>
        public RobustPadeFitter(Vector xs, Vector ys, int numer, int denom, ddouble? intercept = null)
            : base(xs, ys, numer, denom, intercept) { }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(int iter = 8, ddouble? norm_cost = null, double eps = 1e-16) {
            double err_threshold, inv_err;
            double[] weights = new double[Points], errs = new double[Points];

            Vector coef = base.ExecuteFitting(norm_cost: norm_cost);

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            while (iter > 0) {
                Vector err = Error(coef);
                for (int i = 0; i < Points; i++) {
                    errs[i] = Math.Abs((double)err[i]);
                }

                double[] sort_err_list = (double[])errs.Clone();
                Array.Sort(sort_err_list);

                err_threshold = sort_err_list[Points / 2] * 1.25;
                if (err_threshold <= eps) {
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

                coef = base.ExecuteFitting(new Vector(weights), norm_cost);

                iter--;
            }

            return coef;
        }
    }
}
