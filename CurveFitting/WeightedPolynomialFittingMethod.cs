using System;
using Algebra;

namespace CurveFitting {

    /// <summary>重み付き多項式フィッティング</summary>
    public class WeightedPolynomialFittingMethod : FittingMethod {
        
        readonly double[] weight_list;

        /// <summary>コンストラクタ</summary>
        public WeightedPolynomialFittingMethod(FittingData[] data_list, double[] weight_list, int degree, bool is_enable_section)
            : base(data_list, degree + (is_enable_section ? 1 : 0)) {

            this.Degree = degree;
            this.IsEnableSection = is_enable_section;

            if(weight_list == null) {
                throw new ArgumentNullException(nameof(weight_list));
            }

            if(data_list.Length != weight_list.Length) {
                throw new ArgumentException($"{nameof(data_list)},{nameof(weight_list)}");
            }

            foreach(var weight in weight_list) {
                if(!(weight >= 0)) {
                    throw new ArgumentException(nameof(weight_list));
                }
            }

            this.weight_list = (double[])weight_list.Clone();
        }

        /// <summary>次数</summary>
        public int Degree {
            get; private set;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; set; }

        /// <summary>重み付き誤差二乗和</summary>
        public double WeightedCost(Vector coefficients) {
            if(coefficients == null) {
                throw new ArgumentNullException(nameof(coefficients));
            }
            if(coefficients.Dim != ParametersCount) {
                throw new ArgumentException(nameof(coefficients));
            }

            Vector errors = Error(coefficients);
            double cost = 0;
            for(int i = 0; i < errors.Dim; i++) {
                cost += weight_list[i] * errors[i] * errors[i];
            }

            return cost;
        }

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

            Matrix m_transpose = m.Transpose;

            for(int i = 0; i < data_list.Length; i++) {
                for(int j = 0; j < m_transpose.Rows; j++) {
                    m_transpose[j, i] *= weight_list[i];
                }
            }

            return (m_transpose * m).Inverse * m_transpose * b;
        }
    }
}
