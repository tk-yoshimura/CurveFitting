using Algebra;

namespace CurveFitting {
    /// <summary>Gauss-Newton法</summary>
    public class GaussNewtonMethod : FittingMethod {
        readonly FittingFunction func;

        /// <summary>コンストラクタ</summary>
        public GaussNewtonMethod(double[] xs, double[] ys, FittingFunction func)
            : base(xs, ys, func.Parameters) {

            this.func = func;
        }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector parameters) {
            return func.F(x, parameters);
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(Vector parameters, double lambda = 0.75, int loop = 32) {
            Vector errors, dparam;
            Matrix jacobian;

            for (int j = 0; j < loop; j++) {
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
            Matrix jacobian = new(Points, func.Parameters);

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
