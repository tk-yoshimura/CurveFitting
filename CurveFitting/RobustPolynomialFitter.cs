using Algebra;
using DoubleDouble;
using System;

namespace CurveFitting {

    /// <summary>ロバスト多項式フィッティング</summary>
    public class RobustPolynomialFitter : PolynomialFitter {

        /// <summary>コンストラクタ</summary>
        public RobustPolynomialFitter(Vector xs, Vector ys, int degree, ddouble? intercept = null)
            : base(xs, ys, degree, intercept) { }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(int converge_times = 8) {
            double err_threshold, inv_err;
            double[] weights = new double[Points], errs = new double[Points];

            Vector coef = null;

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            while (converge_times > 0) {
                coef = base.ExecuteFitting(new Vector(weights));

                Vector err = Error(coef);
                for (int i = 0; i < Points; i++) {
                    errs[i] = Math.Abs((double)err[i]);
                }

                double[] sort_err_list = (double[])errs.Clone();
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
