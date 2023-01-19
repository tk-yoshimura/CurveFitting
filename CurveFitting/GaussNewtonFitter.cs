using Algebra;
using DoubleDouble;
using System.Collections.Generic;

namespace CurveFitting {
    /// <summary>Gauss-Newton法</summary>
    public class GaussNewtonFitter : Fitter {
        readonly FittingFunction func;

        /// <summary>コンストラクタ</summary>
        public GaussNewtonFitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, FittingFunction func)
            : base(xs, ys, func.Parameters) {

            this.func = func;
        }

        /// <summary>フィッティング値</summary>
        public override ddouble FittingValue(ddouble x, Vector parameters) {
            return func.F(x, parameters);
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(Vector parameters, double lambda = 0.75, int iter = 64) {
            Vector errors, dparam;
            Matrix jacobian;

            for (int j = 0; j < iter; j++) {
                errors = Error(parameters);
                jacobian = Jacobian(parameters);
                dparam = jacobian.Inverse * errors;

                if (!Vector.IsValid(dparam)) {
                    break;
                }

                parameters -= dparam * lambda;
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
