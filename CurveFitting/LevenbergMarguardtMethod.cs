using Algebra;

namespace CurveFitting {
	/// <summary>Levenberg-MarguardtMethod法</summary>
	public class LevenbergMarquardtMethod : FittingMethod{
        readonly FittingFunction func;

        /// <summary>コンストラクタ</summary>
        public LevenbergMarquardtMethod(FittingData[] data_list, FittingFunction func) : base(data_list, func.ParametersCount){
            this.func = func;
        }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector parameters) {
            return func.F(x, parameters);
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(Vector parameters, double lambda_init = 1, double lambda_decay = 0.9, int loop = 64) {
            Vector errors, dparam;
            Matrix jacobian, jacobian_transpose;

            double lambda = lambda_init;

            for(int j = 0; j < loop; j++) {
                errors = Error(parameters);
                jacobian = Jacobian(parameters);
                jacobian_transpose = jacobian.Transpose;

                dparam = (jacobian_transpose * jacobian + lambda * Matrix.Identity(ParametersCount)).Inverse * jacobian_transpose * errors;

                if(!Vector.IsValid(dparam)) {
                    break;
                }

                parameters -= dparam;

                lambda *= lambda_decay;
            }

            return parameters;
        }

        /// <summary>ヤコビアン行列</summary>
        private Matrix Jacobian(Vector parameters) {
            FittingData data;
            Matrix jacobian = new Matrix(data_list.Length, func.ParametersCount);

            for(int i = 0, j; i < data_list.Length; i++) {
                data = data_list[i];
                Vector df = func.DiffF(data.X, parameters);

                for(j = 0; j < parameters.Dim; j++) {
                    jacobian[i, j] = df[j];
                }
            }

            return jacobian;
        }
    }
}
