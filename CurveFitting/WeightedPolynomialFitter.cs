using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {

    /// <summary>重み付き多項式フィッティング</summary>
    public class WeightedPolynomialFitter : Fitter {

        readonly double[] weight_list;

        /// <summary>コンストラクタ</summary>
        public WeightedPolynomialFitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, double[] weights, int degree, bool enable_intercept)
            : base(xs, ys, degree + (enable_intercept ? 1 : 0)) {

            this.Degree = degree;
            this.EnableIntercept = enable_intercept;

            if (weights is null) {
                throw new ArgumentNullException(nameof(weights));
            }

            if (Points != weights.Length) {
                throw new ArgumentException(null, $"{nameof(weights)}");
            }

            foreach (var weight in weights) {
                if (!(weight >= 0)) {
                    throw new ArgumentException(null, nameof(weights));
                }
            }

            this.weight_list = (double[])weights.Clone();
        }

        /// <summary>次数</summary>
        public int Degree {
            get; private set;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool EnableIntercept { get; set; }

        /// <summary>重み付き誤差二乗和</summary>
        public ddouble WeightedCost(Vector coefficients) {
            if (coefficients is null) {
                throw new ArgumentNullException(nameof(coefficients));
            }
            if (coefficients.Dim != Parameters) {
                throw new ArgumentException(null, nameof(coefficients));
            }

            Vector errors = Error(coefficients);
            ddouble cost = 0;
            for (int i = 0; i < errors.Dim; i++) {
                cost += weight_list[i] * errors[i] * errors[i];
            }

            return cost;
        }

        /// <summary>フィッティング値</summary>
        public override ddouble FittingValue(ddouble x, Vector coefficients) {
            if (EnableIntercept) {
                ddouble y = coefficients[0], ploy_x = 1;

                for (int i = 1; i < coefficients.Dim; i++) {
                    ploy_x *= x;
                    y += ploy_x * coefficients[i];
                }

                return y;
            }
            else {
                ddouble y = 0, ploy_x = 1;

                for (int i = 0; i < coefficients.Dim; i++) {
                    ploy_x *= x;
                    y += ploy_x * coefficients[i];
                }

                return y;
            }
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting() {
            Matrix m = new(Points, Parameters);
            Vector b = Vector.Zero(Points);

            if (EnableIntercept) {
                for (int i = 0; i < Points; i++) {
                    ddouble x = X[i];
                    b[i] = Y[i];

                    m[i, 0] = 1;

                    for (int j = 1; j <= Degree; j++) {
                        m[i, j] = m[i, j - 1] * x;
                    }
                }
            }
            else {
                for (int i = 0; i < Points; i++) {
                    ddouble x = X[i];
                    b[i] = Y[i];

                    m[i, 0] = x;

                    for (int j = 1; j < Degree; j++) {
                        m[i, j] = m[i, j - 1] * x;
                    }
                }
            }

            Matrix m_transpose = m.Transpose;

            for (int i = 0; i < Points; i++) {
                for (int j = 0; j < m_transpose.Rows; j++) {
                    m_transpose[j, i] *= weight_list[i];
                }
            }

            return (m_transpose * m).Inverse * m_transpose * b;
        }
    }
}
