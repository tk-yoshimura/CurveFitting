using Algebra;
using DoubleDouble;
using System;
using System.Collections.Generic;

namespace CurveFitting {
    internal class SumTable {
        private readonly List<Vector> xs = new(), ys = new();
        private Dictionary<(int xn, int yn), ddouble> table;

        private Vector? w = null;

        public SumTable(Vector x, Vector y) {
            if (x.Dim != y.Dim) {
                throw new ArgumentException("Illegal length.", $"{nameof(x)},{nameof(y)}");
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
                    xs.Add(xs[0] * xs[^1]);
                }

                for (int i = ys.Count; i < yn; i++) {
                    ys.Add(ys[0] * ys[^1]);
                }

                if (!table.ContainsKey((xn, yn))) {
                    if (xn > 0 && yn > 0) {
                        Vector x = xs[xn - 1], y = ys[yn - 1];

                        ddouble s = w is null ? (x * y).Sum : (x * y * w).Sum;

                        table.Add((xn, yn), s);
                    }
                    else if (xn > 0) {
                        Vector x = xs[xn - 1];

                        ddouble s = w is null ? x.Sum : (x * w).Sum;

                        table.Add((xn, yn), s);
                    }
                    else {
                        Vector y = ys[yn - 1];

                        ddouble s = w is null ? y.Sum : (y * w).Sum;

                        table.Add((xn, yn), s);
                    }
                }

                return table[(xn, yn)];
            }
        }

        public Vector? W {
            get => w;
            set {
                if (value is not null && xs[0].Dim != value.Dim) {
                    throw new ArgumentException("Illegal length.", nameof(w));
                }

                this.w = value;
                this.table = new() {
                    { (0, 0), w is null ? xs[0].Dim : w.Sum },
                };
            }
        }
    }
}
