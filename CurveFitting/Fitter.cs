using Algebra;
using DoubleDouble;
using System;

namespace CurveFitting {
    /// <summary>フィッティング基本クラス</summary>
    public abstract class Fitter {

        /// <summary>フィッティング対象の独立変数</summary>
        public Vector X { get; private set; }

        /// <summary>フィッティング対象の従属変数</summary>
        public Vector Y { get; private set; }

        /// <summary>フィッティング対象数</summary>
        public int Points { get; private set; }

        /// <summary>パラメータ数</summary>
        public int Parameters { get; private set; }

        /// <summary>コンストラクタ</summary>
        public Fitter(Vector xs, Vector ys, int parameters) {
            if (xs.Dim < parameters || xs.Dim != ys.Dim) {
                throw new ArgumentException("mismatch size", $"{nameof(xs)},{nameof(ys)}");
            }
            if (parameters < 1) {
                throw new ArgumentOutOfRangeException(nameof(parameters));
            }

            this.X = xs.Copy();
            this.Y = ys.Copy();
            this.Points = xs.Dim;
            this.Parameters = parameters;
        }

        /// <summary>誤差二乗和</summary>
        public ddouble Cost(Vector parameters) {
            if (parameters.Dim != Parameters) {
                throw new ArgumentException("invalid size", nameof(parameters));
            }

            Vector errors = Error(parameters);
            ddouble cost = errors.SquareNorm;

            return cost;
        }

        /// <summary>誤差</summary>
        public Vector Error(Vector parameters) {
            if (parameters.Dim != Parameters) {
                throw new ArgumentException("invalid size", nameof(parameters));
            }

            Vector errors = FittingValue(X, parameters) - Y;

            return errors;
        }

        /// <summary>フィッティング値</summary>
        public abstract ddouble FittingValue(ddouble x, Vector parameters);

        /// <summary>フィッティング値</summary>
        public Vector FittingValue(Vector xs, Vector parameters) {
            ddouble[] ys = new ddouble[xs.Dim];

            for (int i = 0; i < xs.Dim; i++) {
                ys[i] = FittingValue(xs[i], parameters);
            }

            return ys;
        }
    }
}
