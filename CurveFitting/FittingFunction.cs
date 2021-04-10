using Algebra;
using System;

namespace CurveFitting {

    /// <summary>フィッティング関数</summary>
    public class FittingFunction {
        readonly Func<double, Vector, double> f;
        readonly Func<double, Vector, Vector> df;

        /// <summary>コンストラクタ</summary>
        public FittingFunction(int parameters, Func<double, Vector, double> f, Func<double, Vector, Vector> df) {
            this.Parameters = parameters;
            this.f = f;
            this.df = df;
        }

        /// <summary>パラメータ数</summary>
        public int Parameters {
            get; private set;
        }

        /// <summary>関数値</summary>
        public double F(double x, Vector v) {
            return f(x, v);
        }

        /// <summary>関数勾配</summary>
        public Vector DiffF(double x, Vector v) {
            return df(x, v);
        }
    }
}