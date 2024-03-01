using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurveFitting {
    public static class CurveFittingUtils {
        public static IEnumerable<(int m, int n)> EnumeratePadeDegree(int coef_counts, int degree_delta) {
            ArgumentOutOfRangeException.ThrowIfLessThan(coef_counts, 4);
            ArgumentOutOfRangeException.ThrowIfNegative(degree_delta);

            int d = (coef_counts + 1) / 2;

            if (coef_counts % 2 == 0 && d > 0) {
                yield return (d, d);

                for (int k = 1; k <= degree_delta / 2 && k + 1 < d; k++) {
                    yield return (d + k, d - k);
                    yield return (d - k, d + k);
                }
            }
            else if (coef_counts % 2 == 1 && d > 1 && degree_delta > 0) {
                yield return (d, d - 1);
                yield return (d - 1, d);

                for (int k = 1; k < (degree_delta + 1) / 2 && k + 2 < d; k++) {
                    yield return (d + k, d - 1 - k);
                    yield return (d - 1 - k, d + k);
                }
            }
        }

        public static bool HasLossDigitsPolynomialCoef(Vector coefs, ddouble xmin, ddouble xmax) {
            if (ddouble.IsZero(coefs[0])) {
                if (coefs.Dim > 1) {
                    return HasLossDigitsPolynomialCoef(coefs[1..], xmin, xmax);
                }

                return false;
            }

            ArgumentOutOfRangeException.ThrowIfGreaterThan(xmin, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(xmax, 0);

            ddouble xnmin = 1, xnmax = 1;

            for (int k = 1; k < coefs.Dim; k++) {
                xnmin *= xmin;
                xnmax *= xmax;

                ddouble cnmin = coefs[k] * xnmin;
                ddouble cnmax = coefs[k] * xnmax;

                if (ddouble.Sign(coefs[0]) * ddouble.Sign(cnmin) < 0 && (cnmin / coefs[0]) < -0.25) {
                    return true;
                }

                if (ddouble.Sign(coefs[0]) * ddouble.Sign(cnmax) < 0 && (cnmax / coefs[0]) < -0.25) {
                    return true;
                }
            }

            ddouble ymin = Vector.Polynomial(xmin, coefs);
            ddouble ymax = Vector.Polynomial(xmax, coefs);

            if (ddouble.Sign(coefs[0]) * ddouble.Sign(ymin) < 0 || (ymin / coefs[0]) < 0.5) {
                return true;
            }

            if (ddouble.Sign(coefs[0]) * ddouble.Sign(ymax) < 0 || (ymax / coefs[0]) < 0.5) {
                return true;
            }

            return false;
        }

        public static ddouble RelativeError(ddouble expected, ddouble actual) {
            if (ddouble.IsZero(expected)) {
                return ddouble.IsZero(actual) ? ddouble.Zero : ddouble.PositiveInfinity;
            }
            else {
                ddouble error = ddouble.Abs((expected - actual) / expected);

                return error;
            }
        }

        public static ddouble AbsoluteError(ddouble expected, ddouble actual) {
            ddouble error = ddouble.Abs(expected - actual);

            return error;
        }

        public static ddouble MaxRelativeError(Vector expected, Vector actual) {
            return Vector.Func(RelativeError, expected, actual).Select(item => item.val).Max();
        }

        public static ddouble MaxAbsoluteError(Vector expected, Vector actual) {
            return Vector.Func(AbsoluteError, expected, actual).Select(item => item.val).Max();
        }

        public static IEnumerable<(ddouble numer, ddouble denom)> EnumeratePadeCoef(Vector param, int m, int n) {
            if (param.Dim != checked(m + n)) {
                throw new ArgumentException("invalid param dims", nameof(param));
            }

            if (m >= n) {
                for (int i = 0; i < n; i++) {
                    yield return (param[..m][i], param[m..][i]);
                }
                for (int i = 0; i < m - n; i++) {
                    yield return (param[..m][n + i], 0);
                }
            }
            else {
                for (int i = 0; i < m; i++) {
                    yield return (param[..m][i], param[m..][i]);
                }
                for (int i = 0; i < n - m; i++) {
                    yield return (0, param[m..][m + i]);
                }
            }
        }

        public static (int exp_scale, Vector v_standardized) StandardizeExponent(Vector v) {
            if (v.Dim <= 0 || v.All(v => ddouble.IsZero(v.val))) {
                throw new ArgumentException("invalid vector because it zero vector", nameof(v));
            }

            if (v.Any(v => !ddouble.IsFinite(v.val))) {
                throw new ArgumentException("invalid vector because it contains infinite values", nameof(v));
            }

            int exp_scale = v.Max(v => double.ILogB((double)v.val));
            Vector v_standardized = (x => ddouble.Ldexp(x, -exp_scale), v);

            return (exp_scale, v_standardized);
        }
    }
}
