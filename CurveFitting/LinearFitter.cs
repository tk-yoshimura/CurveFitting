﻿using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {

    /// <summary>線形フィッティング</summary>
    public class LinearFitter : Fitter {
        /// <summary>y切片を有効にするか</summary>
        public bool EnableIntercept { get; private set; }

        /// <summary>コンストラクタ</summary>
        public LinearFitter(IReadOnlyList<ddouble> xs, IReadOnlyList<ddouble> ys, bool enable_intercept)
            : base(xs, ys, enable_intercept ? 2 : 1) {

            EnableIntercept = enable_intercept;
        }

        /// <summary>フィッティング値</summary>
        public override ddouble FittingValue(ddouble x, Vector parameters) {
            if (parameters is null) {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (parameters.Dim != Parameters) {
                throw new ArgumentException(null, nameof(parameters));
            }

            if (EnableIntercept) {
                return parameters[0] + parameters[1] * x;
            }
            else {
                return parameters[0] * x;
            }
        }

        /// <summary>フィッティング</summary>
        public Vector ExecuteFitting() {
            if (EnableIntercept) {
                ddouble sum_x = 0, sum_y = 0, sum_sq_x = 0, sum_xy = 0, n = Points;

                for (int i = 0; i < Points; i++) {
                    ddouble x = X[i], y = Y[i];

                    sum_x += x;
                    sum_y += y;
                    sum_sq_x += x * x;
                    sum_xy += x * y;
                }

                ddouble r = 1 / (sum_x * sum_x - n * sum_sq_x);
                ddouble a = (sum_x * sum_xy - sum_sq_x * sum_y) * r;
                ddouble b = (sum_x * sum_y - n * sum_xy) * r;

                return new Vector(a, b);
            }
            else {
                ddouble sum_sq_x = 0, sum_xy = 0;

                for (int i = 0; i < Points; i++) {
                    ddouble x = X[i], y = Y[i];

                    sum_sq_x += x * x;
                    sum_xy += x * y;
                }

                return new Vector(sum_xy / sum_sq_x);
            }
        }

    }
}
