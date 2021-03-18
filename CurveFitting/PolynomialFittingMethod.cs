using Algebra;

namespace CurveFitting {

	/// <summary>多項式フィッティング</summary>
	public class PolynomialFittingMethod : FittingMethod {
        /// <summary>コンストラクタ</summary>
        public PolynomialFittingMethod(FittingData[] data_list, int degree, bool is_enable_section)
            : base(data_list, degree + (is_enable_section ? 1 : 0)) {

            this.Degree = degree;
            this.IsEnableSection = is_enable_section;
        }

        /// <summary>次数</summary>
        public int Degree {
            get; private set;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; set; }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector coefficients) {
            if(IsEnableSection) {
                double y = coefficients[0], ploy_x = 1;

                for(int i = 1; i < coefficients.Dim; i++) {
                    ploy_x *= x;
                    y += ploy_x * coefficients[i];
                }

                return y;
            }
            else {
                double y = 0, ploy_x = 1;

                for(int i = 0; i < coefficients.Dim; i++) {
                    ploy_x *= x;
                    y += ploy_x * coefficients[i];
                }

                return y;
            }
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting() {
            Matrix m = new Matrix(data_list.Length, ParametersCount);
            Vector b = Vector.Zero(data_list.Length);

            if(IsEnableSection) {
                for(int i = 0; i < data_list.Length; i++) {
                    double x = data_list[i].X;
                    b[i] = data_list[i].Y;

                    m[i, 0] = 1;

                    for(int j = 1; j <= Degree; j++) {
                        m[i, j] = m[i, j - 1] * x;
                    }
                }
            }
            else {
                for(int i = 0; i < data_list.Length; i++) {
                    double x = data_list[i].X;
                    b[i] = data_list[i].Y;

                    m[i, 0] = x;

                    for(int j = 1; j < Degree; j++) {
                        m[i, j] = m[i, j - 1] * x;
                    }
                }
            }

            return (m.Transpose * m).Inverse * m.Transpose * b;
        }
    }
}
