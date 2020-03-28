using System;
using Algebra;

namespace CurveFitting {

    /// <summary>重み付き線形フィッティング</summary>
    public class WeightedLinearFittingMethod : FittingMethod {

        readonly double[] weight_list;

        /// <summary>コンストラクタ</summary>
        public WeightedLinearFittingMethod(FittingData[] data_list, double[] weight_list, bool is_enable_section) : base(data_list, is_enable_section ? 2 : 1) {
            IsEnableSection = is_enable_section;

            if(weight_list == null) {
                throw new ArgumentNullException(nameof(weight_list));
            }

            if(data_list.Length != weight_list.Length) {
                throw new ArgumentException($"{nameof(data_list)},{nameof(weight_list)}");
            }

            foreach(var weight in weight_list) {
                if(!(weight >= 0)) {
                    throw new ArgumentException(nameof(weight_list));
                }
            }

            this.weight_list = weight_list;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; private set; }

        /// <summary>重み付き誤差二乗和</summary>
        public double WeightedCost(Vector parameters) {
            if(parameters == null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if(parameters.Dim != ParametersCount) {
                throw new ArgumentException(nameof(parameters));
            }

            Vector errors = Error(parameters);
            double cost = 0;
            for(int i = 0; i < errors.Dim; i++) {
                cost += weight_list[i] * errors[i] * errors[i];
            }

            return cost;
        }

        /// <summary>フィッティング値</summary>
        public override double FittingValue(double x, Vector parameters) {
            if(parameters == null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if(parameters.Dim != ParametersCount) {
                throw new ArgumentException(nameof(parameters));
            }

            if(IsEnableSection) {
                return parameters[0] + parameters[1] * x;
            }
            else {
                return parameters[0] * x;
            }
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting() {
            if(weight_list == null) {
                throw new InvalidOperationException();
            }

            if(IsEnableSection) {
                FittingData data;
                double w, sum_w = 0, sum_wx = 0, sum_wy = 0, sum_wxx = 0, sum_wxy = 0;

                for(int i = 0; i < data_list.Length; i++) {
                    data = data_list[i];
                    w = weight_list[i];
                    sum_w += w;
                    sum_wx += w * data.X;
                    sum_wy += w * data.Y;
                    sum_wxx += w * data.X * data.X;
                    sum_wxy += w * data.X * data.Y;
                }

                double r = 1 / (sum_wx * sum_wx - sum_w * sum_wxx);
                double a = (sum_wx * sum_wxy - sum_wxx * sum_wy) * r;
                double b = (sum_wx * sum_wy - sum_w * sum_wxy) * r;

                return new Vector(a, b);
            }
            else {
                FittingData data;
                double w, sum_wxx = 0, sum_wxy = 0;

                for(int i = 0; i < data_list.Length; i++) {
                    data = data_list[i];
                    w = weight_list[i];
                    sum_wxx += w * data.X * data.X;
                    sum_wxy += w * data.X * data.Y;
                }

                return new Vector(sum_wxy / sum_wxx);
            }
        }
    }
}
