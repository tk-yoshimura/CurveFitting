using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {
    /// <summary>フィッティング基本クラス</summary>
    public abstract class Fitter {

        /// <summary>フィッティング対象の独立変数</summary>
        public IReadOnlyList<ddouble> X { get; private set; }

        /// <summary>フィッティング対象の従属変数</summary>
        public IReadOnlyList<ddouble> Y { get; private set; }

        /// <summary>フィッティング対象数</summary>
        public int Points { get; private set; }

        /// <summary>パラメータ数</summary>
        public int Parameters { get; private set; }

        /// <summary>コンストラクタ</summary>
        public Fitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, int parameters) {
            if (xs is null) {
                throw new ArgumentNullException(nameof(xs));
            }
            if (ys is null) {
                throw new ArgumentNullException(nameof(ys));
            }
            if (xs.Count < parameters || xs.Count != ys.Count) {
                throw new ArgumentException($"{nameof(xs.Count)}, {nameof(ys.Count)}");
            }
            if (parameters < 1) {
                throw new ArgumentException(null, nameof(parameters));
            }

            this.X = xs;
            this.Y = ys;
            this.Points = xs.Count;
            this.Parameters = parameters;
        }

        /// <summary>誤差二乗和</summary>
        public ddouble Cost(Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != Parameters) {
                throw new ArgumentException(null, nameof(parameters));
            }

            Vector errors = Error(parameters);
            ddouble cost = 0;
            for (int i = 0; i < errors.Dim; i++) {
                cost += errors[i] * errors[i];
            }

            return cost;
        }

        /// <summary>誤差</summary>
        public Vector Error(Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != Parameters) {
                throw new ArgumentException(null, nameof(parameters));
            }

            Vector errors = Vector.Zero(Points);

            for (int i = 0; i < Points; i++) {
                errors[i] = FittingValue(X[i], parameters) - Y[i];
            }

            return errors;
        }

        /// <summary>フィッティング値</summary>
        public abstract ddouble FittingValue(ddouble x, Vector parameters);
    }
}
