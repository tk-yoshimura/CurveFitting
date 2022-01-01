using Algebra;
using DoubleDouble;
using System.Collections.Generic;

namespace CurveFitting {

    /// <summary>多項式フィッティング</summary>
    public class PolynomialFitter : Fitter {
        /// <summary>コンストラクタ</summary>
        public PolynomialFitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, int degree, bool enable_intercept)
            : base(xs, ys, checked(degree + (enable_intercept ? 1 : 0))) {

            this.Degree = degree;
            this.EnableIntercept = enable_intercept;
        }

        /// <summary>次数</summary>
        public int Degree {
            get; private set;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool EnableIntercept { get; set; }

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

            return (m.Transpose * m).Inverse * m.Transpose * b;
        }
    }
}
