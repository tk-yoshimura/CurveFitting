using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {
    /// <summary>有理関数フィッティング</summary>
    public class RationalFitter : Fitter {
        readonly FittingFunction func;

        /// <summary>y切片を有効にするか</summary>
        public bool EnableIntercept { get; private set; }

        /// <summary>分母部の係数の数</summary>
        public int M { get; private set; }

        /// <summary>分子部の係数の数</summary>
        public int N { get; private set; }

        public RationalFitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, int m, int n, bool enable_intercept)
            : base(xs, ys, checked(m + n + (enable_intercept ? 1 : 0))) {

            if (m < 1 || n < 1) {
                throw new ArgumentOutOfRangeException($"{nameof(m)},{nameof(n)}");
            }

            this.M = m + (enable_intercept ? 1 : 0);
            this.N = n;

            if (enable_intercept) {
                ddouble f(ddouble x, Vector parameter) {
                    ddouble sm = parameter[M - 1], sn = parameter[Parameters - 1];
                    for (int i = M - 2; i >= 0; i--) {
                        sm = sm * x + parameter[i];
                    }
                    for (int i = Parameters - 2; i >= M; i--) {
                        sn = sn * x + parameter[i];
                    }
                    sn = sn * x + 1;

                    ddouble y = sm / sn;

                    return y;
                }

                Vector df(ddouble x, Vector parameter) {
                    ddouble sm = parameter[M - 1], sn = parameter[Parameters - 1];
                    for (int i = M - 2; i >= 0; i--) {
                        sm = sm * x + parameter[i];
                    }
                    for (int i = Parameters - 2; i >= M; i--) {
                        sn = sn * x + parameter[i];
                    }
                    sn = sn * x + 1;

                    ddouble u = 1d / sn, v = -x * sm / (sn * sn);

                    ddouble[] d = new ddouble[Parameters];

                    for (int i = 0; i < M; i++) {
                        d[i] = u;
                        if (i < M - 1) {
                            u *= x;
                        }
                    }

                    for (int i = 0; i < N; i++) {
                        d[M + i] = v;
                        if (i < N - 1) {
                            v *= x;
                        }
                    }

                    return new(d);
                }

                this.func = new FittingFunction(Parameters, f, df);
            }
            else {
                ddouble f(ddouble x, Vector parameter) {
                    ddouble sm = parameter[M - 1], sn = parameter[Parameters - 1];
                    for (int i = M - 2; i >= 0; i--) {
                        sm = sm * x + parameter[i];
                    }
                    for (int i = Parameters - 2; i >= M; i--) {
                        sn = sn * x + parameter[i];
                    }
                    sm = sm * x;
                    sn = sn * x + 1;

                    ddouble y = sm / sn;

                    return y;
                }

                Vector df(ddouble x, Vector parameter) {
                    ddouble sm = parameter[M - 1], sn = parameter[Parameters - 1];
                    for (int i = M - 2; i >= 0; i--) {
                        sm = sm * x + parameter[i];
                    }
                    for (int i = Parameters - 2; i >= M; i--) {
                        sn = sn * x + parameter[i];
                    }
                    sm = sm * x;
                    sn = sn * x + 1;

                    ddouble u = x / sn, v = -x * sm / (sn * sn);

                    ddouble[] d = new ddouble[Parameters];

                    for (int i = 0; i < M; i++) {
                        d[i] = u;
                        if (i < M - 1) {
                            u *= x;
                        }
                    }

                    for (int i = 0; i < N; i++) {
                        d[M + i] = v;
                        if (i < N - 1) {
                            v *= x;
                        }
                    }

                    return new(d);
                }

                this.func = new FittingFunction(Parameters, f, df);
            }
        }

        public override ddouble FittingValue(ddouble x, Vector parameters) {
            return func.F(x, parameters);
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting(double lambda_init = 1, double lambda_decay = 0.995, int iter = 128) {
            LevenbergMarquardtFitter fitter = new(X, Y, func);
            Vector param = fitter.ExecuteFitting(InitialParameter(), lambda_init, lambda_decay, iter);

            return param;
        }

        /// <summary>初期パラメータの決定</summary>
        private Vector InitialParameter() {
            PolynomialFitter poly_fitter = new(X, Y, M + N, enable_intercept: true);
            Vector c = poly_fitter.ExecuteFitting();

            int k = M + N;

            Matrix a = new(k, k);
            Vector u = new(k);

            for (int i = 0; i < M; i++) {
                a[i, i] = 1;
            }

            for (int i = M; i < k; i++) {
                for (int j = i - M, r = 0; j < k; j++, r++) {
                    a[j, i] = -c[r];
                }
            }

            for (int i = 0; i < k; i++) {
                u[i] = c[i + 1];
            }

            Vector v = a.Inverse * u;

            if (!EnableIntercept) {
                return v;
            }
            else {
                Vector p = new Vector(k + 1);
                p[0] = c[0];
                for (int i = 0; i < k; i++) {
                    p[i + 1] = v[i];
                }

                return p;
            }
        }
    }
}
