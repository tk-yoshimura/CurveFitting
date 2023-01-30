using Algebra;
using DoubleDouble;
using System;

namespace CurveFitting {

    /// <summary>ロバスト線形フィッティング</summary>
    public class RobustLinearFitter : LinearFitter {

        /// <summary>コンストラクタ</summary>
        public RobustLinearFitter(Vector xs, Vector ys, ddouble? intercept = null)
            : base(xs, ys, intercept) { }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(int iter = 8, double eps = 1e-16) {
            double err_threshold, inv_err;
            double[] weights = new double[Points], errs = new double[Points];

            Vector coef = null;

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            while (iter > 0) {
                coef = base.ExecuteFitting(new Vector(weights));

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

                iter--;
            }

            return coef;
        }
    }
}
