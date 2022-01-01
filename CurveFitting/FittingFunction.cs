using Algebra;
using DoubleDouble;
using System;

namespace CurveFitting {

    /// <summary>フィッティング関数</summary>
    public class FittingFunction {
        readonly Func<ddouble, Vector, ddouble> f;
        readonly Func<ddouble, Vector, Vector> df;

        /// <summary>コンストラクタ</summary>
        public FittingFunction(int parameters, Func<ddouble, Vector, ddouble> f, Func<ddouble, Vector, Vector> df) {
            this.Parameters = parameters;
            this.f = f;
            this.df = df;
        }

        /// <summary>パラメータ数</summary>
        public int Parameters {
            get; private set;
        }

        /// <summary>関数値</summary>
        public ddouble F(ddouble x, Vector v) {
            return f(x, v);
        }

        /// <summary>関数勾配</summary>
        public Vector DiffF(ddouble x, Vector v) {
            return df(x, v);
        }
    }
}