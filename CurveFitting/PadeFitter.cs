using Algebra;
using DoubleDouble;
using System;

namespace CurveFitting {
    public class PadeFitter : Fitter {

        private readonly SumTable sum_table;
        private readonly ddouble? intercept;

        /// <summary>分子係数</summary>
        public int Numer { get; private set; }

        /// <summary>分母係数</summary>
        public int Denom { get; private set; }

        /// <summary>コンストラクタ</summary>
        /// <param name="numer">分子係数</param>
        /// <param name="denom">分母係数</param>
        /// <param name="intercept">切片</param>
        public PadeFitter(Vector xs, Vector ys, int numer, int denom, ddouble? intercept = null)
            : base(xs, ys,
                  parameters:
                  (numer >= 2 && denom >= 2)
                      ? (numer + denom)
                      : throw new ArgumentOutOfRangeException($"{nameof(numer)},{nameof(denom)}")) {

            this.sum_table = new(X, Y);
            this.intercept = intercept;
            this.Numer = numer;
            this.Denom = denom;
        }

        public override ddouble FittingValue(ddouble x, Vector parameters) {
            if (parameters.Dim != Parameters) {
                throw new ArgumentException("invalid size", nameof(parameters));
            }

            (ddouble numer, ddouble denom) = Fraction(x, parameters);

            ddouble y = numer / denom;

            return y;
        }

        public (ddouble numer, ddouble denom) Fraction(ddouble x, Vector parameters) {
            ddouble n = Vector.Polynomial(x, parameters[..Numer]);
            ddouble d = Vector.Polynomial(x, parameters[Numer..]);

            return (n, d);
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(Vector? weights = null, ddouble? norm_cost = null) {
            sum_table.W = weights;
            (Matrix m, Vector v) = GenerateTable(sum_table, Numer, Denom);

            if (norm_cost is not null) {
                ddouble c = norm_cost.Value * sum_table[0, 0];

                for (int i = 0; i < m.Rows; i++) {
                    m[i, i] += c;
                }
            }

            if (intercept is null) {
                Vector x = Matrix.Solve(m, v);

                Vector parameters = Vector.Concat(x[..Numer], 1, x[Numer..]);

                return parameters;
            }
            else {
                v = v[1..] - intercept.Value * m[0, 1..];
                m = m[1.., 1..];

                Vector x = Matrix.Solve(m, v);

                Vector parameters = Vector.Concat(intercept.Value, x[..(Numer - 1)], 1, x[(Numer - 1)..]);

                return parameters;
            }
        }

        internal static (Matrix m, Vector v) GenerateTable(SumTable sum_table, int numer, int denom) {
            int dim = numer + denom - 1;

            ddouble[,] m = new ddouble[dim, dim];
            for (int i = 0, n = numer; i < n; i++) {
                for (int j = i; j < n; j++) {
                    m[i, j] = m[j, i] = sum_table[i + j, 0];
                }
            }
            for (int i = numer, n = dim; i < n; i++) {
                for (int j = 0; j < numer; j++) {
                    m[i, j] = m[j, i] = -sum_table[i + j - numer + 1, 1];
                }
            }
            for (int i = numer, n = dim; i < n; i++) {
                for (int j = i; j < n; j++) {
                    m[i, j] = m[j, i] = sum_table[i + j - 2 * numer + 2, 2];
                }
            }

            ddouble[] v = new ddouble[numer + denom - 1];
            for (int i = 0; i < numer; i++) {
                v[i] = sum_table[i, 1];
            }
            for (int i = numer; i < dim; i++) {
                v[i] = -sum_table[i - numer + 1, 2];
            }

            return (m, v);
        }
    }
}
