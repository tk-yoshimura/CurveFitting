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
            if (!(eps > 0)) {
                throw new ArgumentOutOfRangeException(nameof(eps));
            }

            double err_threshold, inv_err;
            double[] weights = new double[Points], errs = new double[Points];

            Vector coef = base.ExecuteFitting(norm_cost: norm_cost);

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            double s = 4;
            while (iter > 0) {
                Vector err = Error(coef);
                for (int i = 0; i < Points; i++) {
                    errs[i] = Math.Abs((double)err[i]);
                }

                double[] sort_err_list = (double[])errs.Clone();
                Array.Sort(sort_err_list);

                err_threshold = sort_err_list[Points / 2] * s;
                if (err_threshold <= eps) {
                    break;
                }

                inv_err = 1 / (err_threshold + double.Epsilon);

                for (int i = 0; i < Points; i++) {
                    double v = errs[i] * inv_err;
                    double r = double.Max(0d, 1 - v * v);
                    weights[i] = r * r;
                }

                coef = base.ExecuteFitting(new Vector(weights), norm_cost);

                iter--;
                s = double.Max(s * 0.75, 1.25);
            }

            return coef;
        }
    }
}
