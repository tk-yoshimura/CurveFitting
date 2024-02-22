using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {
    public class SumTable {
        private readonly List<Vector> xs = [], ys = [];
        private Dictionary<(int xn, int yn), ddouble> table;

        private Vector? w = null;

        public SumTable(Vector x, Vector y) {
            if (x.Dim != y.Dim) {
                throw new ArgumentException("invalid size", $"{nameof(x)},{nameof(y)}");
            }

            this.xs.Add(x);
            this.ys.Add(y);
            this.table = new() {
                { (0, 0), x.Dim },
            };
        }

        public ddouble this[int xn, int yn] {
            get {
                if (xn < 0 || yn < 0) {
                    throw new ArgumentOutOfRangeException($"{nameof(xn)},{nameof(yn)}");
                }

                for (int i = xs.Count; i < xn; i++) {
                    int xn0 = (i + 1) / 2 - 1, xn1 = i - xn0 - 1;

                    xs.Add(xs[xn0] * xs[xn1]);
                }

                for (int i = ys.Count; i < yn; i++) {
                    int yn0 = (i + 1) / 2 - 1, yn1 = i - yn0 - 1;

                    ys.Add(ys[yn0] * ys[yn1]);
                }

                if (!table.ContainsKey((xn, yn))) {
                    ddouble s;

                    if (xn > 0 && yn > 0) {
                        Vector x = xs[xn - 1], y = ys[yn - 1];

                        s = w is null ? (x * y).Sum : (x * y * w).Sum;
                    }
                    else if (xn > 0) {
                        Vector x = xs[xn - 1];

                        s = w is null ? x.Sum : (x * w).Sum;
                    }
                    else {
                        Vector y = ys[yn - 1];

                        s = w is null ? y.Sum : (y * w).Sum;
                    }

                    table.Add((xn, yn), s);
                }

                return table[(xn, yn)];
            }
        }

        public Vector? W {
            get => w;
            set {
                if (value is not null && xs[0].Dim != value.Dim) {
                    throw new ArgumentException("invalid size", nameof(W));
                }

                this.w = value;
                this.table = new() {
                    { (0, 0), w is null ? xs[0].Dim : w.Sum },
                };
            }
        }
    }
}
