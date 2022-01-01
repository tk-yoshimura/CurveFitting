using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {
    /// <summary>有理関数フィッティング</summary>
    public class RationalFitter : Fitter {
        readonly Func<ddouble, Vector, Vector, ddouble> func;

        /// <summary>分子部の係数の最大次数</summary>
        public int M { get; private set; }

        /// <summary>分母部の係数の最大次数</summary>
        public int N { get; private set; }

        public RationalFitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, int m, int n)
            : base(xs, ys, checked(m + n + 2)) {

            if (m < 1 || n < 1) {
                throw new ArgumentOutOfRangeException($"{nameof(m)},{nameof(n)}");
            }

            this.M = m;
            this.N = n;

            ddouble f(ddouble x, Vector numer_param, Vector denom_param) {
                ddouble sm = numer_param[M], sn = denom_param[N];
                for (int i = M - 1; i >= 0; i--) {
                    sm = sm * x + numer_param[i];
                }
                for (int i = N - 1; i >= 0; i--) {
                    sn = sn * x + denom_param[i];
                }

                ddouble y = sm / sn;

                return y;
            }

            this.func = f;
        }

        public override ddouble FittingValue(ddouble x, Vector parameters) {
            throw new NotImplementedException();
        }

        public ddouble FittingValue(ddouble x, Vector numer_parameters, Vector denom_parameters) {
            return func(x, numer_parameters, denom_parameters);
        }

        public ddouble[] FittingValue(IReadOnlyList<ddouble> xs, Vector numer_parameters, Vector denom_parameters) {
            List<ddouble> ys = new();

            for (int i = 0; i < xs.Count; i++) {
                ys.Add(FittingValue(xs[i], numer_parameters, denom_parameters));
            }

            return ys.ToArray();
        }

        /// <summary>フィッティング</summary>
        public (Vector numer, Vector denom) ExecuteFitting(int iter = 128) {
            ddouble[] vs = new ddouble[Points];
            double[] ws = new double[Points];
            (Vector numer, Vector denom) = InitialParameter();
            
            for (int j = 0; j < iter; j++) {
                ddouble[] new_ys = FittingValue(X, numer, denom);

                for (int i = 0; i < Points; i++) {
                    ddouble x = X[i], y = Y[i];

                    ddouble s = numer[M];
                    for (int k = M - 1; k >= 0; k--) {
                        s = s * x + numer[k];
                    }

                    ddouble v = s / y;

                    vs[i] = ddouble.IsFinite(v) ? v : 0;
                    ws[i] = ddouble.IsFinite(v) ? 1 : 0;
                }

                WeightedPolynomialFitter denom_fitter = new(X, vs, ws, N, enable_intercept: true);
                denom = (denom + denom_fitter.ExecuteFitting() * 3) / 4;

                new_ys = denom_fitter.FittingValue(X, denom);

                new_ys = FittingValue(X, numer, denom);

                for (int i = 0; i < Points; i++) {
                    ddouble x = X[i], y = Y[i];

                    ddouble s = denom[N];
                    for (int k = N - 1; k >= 0; k--) {
                        s = s * x + denom[k];
                    }

                    ddouble v = y / s;

                    vs[i] = ddouble.IsFinite(v) ? v : 0;
                    ws[i] = ddouble.IsFinite(v) ? 1 : 0;
                }

                WeightedPolynomialFitter numer_fitter = new(X, vs, ws, M, enable_intercept: true);
                numer = (numer + numer_fitter.ExecuteFitting() * 3) / 4;

                new_ys = numer_fitter.FittingValue(X, numer);

                Console.WriteLine("next");
            }

            ddouble n = denom[0];
            if (n != 0) {
                numer /= n;
                denom /= n;
            }

            return (numer, denom);
        }

        /// <summary>初期パラメータの決定</summary>
        private (Vector numer, Vector denom) InitialParameter() {
            PolynomialFitter poly_fitter = new(X, Y, M, enable_intercept: true);
            Vector numer = poly_fitter.ExecuteFitting();
            Vector denom = new Vector(N + 1);
            denom[0] = 1;

            return (numer, denom);
        }
    }
}
