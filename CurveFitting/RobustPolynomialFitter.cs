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
        public Vector Fit(int iter = 8, double eps = 1e-16) {
            if (!(eps > 0)) {
                throw new ArgumentOutOfRangeException(nameof(eps));
            }

            double err_threshold, inv_err;
            double[] weights = new double[Points], errs = new double[Points];

            Vector coef = base.Fit();

            for (int i = 0; i < Points; i++) {
                weights[i] = 1;
            }

            double s = 4;
            while (iter > 0) {
                Vector err = Error(coef);
                for (int i = 0; i < Points; i++) {
                    errs[i] = double.Abs((double)err[i]);
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

                coef = base.Fit(new Vector(weights));

                iter--;
                s = double.Max(s * 0.75, 1.25);
            }

            return coef;
        }
    }
}
