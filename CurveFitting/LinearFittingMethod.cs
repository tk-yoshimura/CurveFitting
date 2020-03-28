using System;
using Algebra;

namespace CurveFitting {

    /// <summary>線形フィッティング</summary>
    public class LinearFittingMethod : FittingMethod {

        /// <summary>コンストラクタ</summary>
        public LinearFittingMethod(FittingData[] data_list, bool is_enable_section) : base(data_list, is_enable_section ? 2 : 1) {
            IsEnableSection = is_enable_section;
        }

        /// <summary>y切片を有効にするか</summary>
        public bool IsEnableSection { get; private set; }

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
            if(IsEnableSection) {
                FittingData data;
                double sum_x = 0, sum_y = 0, sum_sq_x = 0, sum_xy = 0, n = data_list.Length;

                for(int i = 0; i < data_list.Length; i++) {
                    data = data_list[i];
                    sum_x += data.X;
                    sum_y += data.Y;
                    sum_sq_x += data.X * data.X;
                    sum_xy += data.X * data.Y;
                }

                double r = 1 / (sum_x * sum_x - n * sum_sq_x);
                double a = (sum_x * sum_xy - sum_sq_x * sum_y) * r;
                double b = (sum_x * sum_y - n * sum_xy) * r;

                return new Vector(a, b);
            }
            else {
                FittingData data;
                double sum_sq_x = 0, sum_xy = 0;

                for(int i = 0; i < data_list.Length; i++) {
                    data = data_list[i];
                    sum_sq_x += data.X * data.X;
                    sum_xy += data.X * data.Y;
                }

                return new Vector(sum_xy / sum_sq_x);
            }
        }

    }
}
