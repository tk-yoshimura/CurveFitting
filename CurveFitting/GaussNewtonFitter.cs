using Algebra;
using DoubleDouble;
using System;

namespace CurveFitting {
    /// <summary>Gauss-Newton法</summary>
    public class GaussNewtonFitter : Fitter {
        readonly FittingFunction func;

        /// <summary>コンストラクタ</summary>
        public GaussNewtonFitter(Vector xs, Vector ys, FittingFunction func)
            : base(xs, ys, func.Parameters) {

            this.func = func;
        }

        /// <summary>フィッティング値</summary>
        public override ddouble Regress(ddouble x, Vector parameters) {
            return func.F(x, parameters);
        }

        /// <summary>フィッティング</summary>
        public Vector Fit(Vector parameters, double lambda = 0.75, int iter = 128, Vector? weights = null, Func<Vector, bool>? iter_callback = null) {
            Vector errors, dparam;
            Matrix jacobian;

            for (int j = 0; j < iter; j++) {
                errors = weights is null ? Error(parameters) : weights * Error(parameters);
                jacobian = Jacobian(parameters);
                dparam = jacobian.Inverse * errors;

                if (!Vector.IsValid(dparam)) {
                    break;
                }

                parameters -= dparam * lambda;

                if (iter_callback is not null) {
                    if (!iter_callback(parameters)) {
                        break;
                    }
                }
            }

            return parameters;
        }

        /// <summary>ヤコビアン行列</summary>
        private Matrix Jacobian(Vector parameters) {
            Matrix jacobian = Matrix.Zero(Points, func.Parameters);

            for (int i = 0, j; i < Points; i++) {
                Vector df = func.DiffF(X[i], parameters);

                for (j = 0; j < parameters.Dim; j++) {
                    jacobian[i, j] = df[j];
                }
            }

            return jacobian;
        }
    }
}
