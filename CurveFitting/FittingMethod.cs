using Algebra;
using System;

namespace CurveFitting {
    /// <summary>フィッティング基本クラス</summary>
    public abstract class FittingMethod {

        /// <summary>フィッティング対象のデータ</summary>
        protected readonly FittingData[] data_list;

        /// <summary>コンストラクタ</summary>
        public FittingMethod(FittingData[] data_list, int parameters) {
            if (data_list is null) {
                throw new ArgumentNullException(nameof(data_list));
            }
            if (data_list.Length < 1) {
                throw new ArgumentException(nameof(data_list));
            }
            if (parameters < 1) {
                throw new ArgumentException(nameof(parameters));
            }

            this.data_list = data_list;
            this.ParametersCount = parameters;
        }

        /// <summary>パラメータ数</summary>
        public int ParametersCount {
            get; private set;
        }

        /// <summary>誤差二乗和</summary>
        public double Cost(Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != ParametersCount) {
                throw new ArgumentException(nameof(parameters));
            }

            Vector errors = Error(parameters);
            double cost = 0;
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
            if (parameters.Dim != ParametersCount) {
                throw new ArgumentException(nameof(parameters));
            }

            Vector errors = Vector.Zero(data_list.Length);

            for (int i = 0; i < data_list.Length; i++) {
                errors[i] = FittingValue(data_list[i].X, parameters) - data_list[i].Y;
            }

            return errors;
        }

        /// <summary>フィッティング値</summary>
        public abstract double FittingValue(double x, Vector parameters);
    }
}
