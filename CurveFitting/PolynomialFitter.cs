using Algebra;
using DoubleDouble;
using System;
using System.Linq;

namespace CurveFitting {

    /// <summary>多項式フィッティング</summary>
    public class PolynomialFitter : Fitter {

        private readonly SumTable sum_table;
        private readonly ddouble? intercept;

        /// <summary>次数</summary>
        public int Degree {
            get; private set;
        }

        /// <summary>コンストラクタ</summary>
        public PolynomialFitter(Vector xs, Vector ys, int degree, ddouble? intercept = null)
            : base(xs, (intercept is null) ? ys : ys.Select(y => y.val - intercept.Value).ToArray(),
                  parameters: checked(degree + 1)) {

            this.sum_table = new(X, Y);
            this.intercept = intercept;
            this.Degree = degree;
        }

        /// <summary>フィッティング値</summary>
        public override ddouble FittingValue(ddouble x, Vector parameters) {
            if (parameters.Dim != Parameters) {
                throw new ArgumentException("invalid size", nameof(parameters));
            }

            ddouble y = parameters[parameters.Dim - 1];

            for (int i = parameters.Dim - 2; i >= 0; i--) {
                y = y * x + parameters[i];
            }

            return y;
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(Vector? weights = null) {
            bool enable_intercept = intercept is null;

            sum_table.W = weights;
            (Matrix m, Vector v) = GenerateTable(sum_table, Degree, enable_intercept);

            if (enable_intercept) {
                Vector parameters = Matrix.Solve(m, v);

                return parameters;
            }
            else {
                Vector parameters = Vector.Zero(Parameters);
                parameters[0] = intercept.Value;
                parameters[1..] = Matrix.Solve(m, v);

                return parameters;
            }
        }

        internal static (Matrix m, Vector v) GenerateTable(SumTable sum_table, int degree, bool enable_intercept) {
            int dim = degree + (enable_intercept ? 1 : 0);

            ddouble[,] m = new ddouble[dim, dim];
            ddouble[] v = new ddouble[dim];

            if (enable_intercept) {
                for (int i = 0; i < dim; i++) {
                    for (int j = i; j < dim; j++) {
                        m[i, j] = m[j, i] = sum_table[i + j, 0];
                    }
                }

                for (int i = 0; i < dim; i++) {
                    v[i] = sum_table[i, 1];
                }
            }
            else {
                for (int i = 0; i < dim; i++) {
                    for (int j = i; j < dim; j++) {
                        m[i, j] = m[j, i] = sum_table[i + j + 2, 0];
                    }
                }

                for (int i = 0; i < dim; i++) {
                    v[i] = sum_table[i + 1, 1];
                }
            }

            return (m, v);
        }
    }
}
