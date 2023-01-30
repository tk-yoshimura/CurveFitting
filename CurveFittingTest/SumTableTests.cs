using DoubleDouble;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CurveFitting.Tests {
    [TestClass()]
    public class SumTableTests {
        [TestMethod()]
        public void IndexerTest() {
            (ddouble x, ddouble y)[] vs = new (ddouble, ddouble)[] {
                (2, 11), (3, 13), (5, 17), (7, 19)
            };
            ddouble s(int xn, int yn) {
                return vs.Select(v => ddouble.Pow(v.x, xn) * ddouble.Pow(v.y, yn)).Sum();
            };

            SumTable table = new(vs.Select(v => v.x).ToArray(), vs.Select(v => v.y).ToArray());

            for (int i = 0; i <= 16; i++) {
                for (int j = 0; j <= i; j++) {
                    Assert.AreEqual(s(i, j), table[i, j]);
                    Assert.AreEqual(s(j, i), table[j, i]);
                }
            }
        }

        [TestMethod()]
        public void ReverseIndexerTest() {
            (ddouble x, ddouble y)[] vs = new (ddouble, ddouble)[] {
                (2, 11), (3, 13), (5, 17), (7, 19)
            };
            ddouble s(int xn, int yn) {
                return vs.Select(v => ddouble.Pow(v.x, xn) * ddouble.Pow(v.y, yn)).Sum();
            };

            SumTable table = new(vs.Select(v => v.x).ToArray(), vs.Select(v => v.y).ToArray());

            for (int i = 16; i >= 0; i--) {
                for (int j = i; j >= 0; j--) {
                    Assert.AreEqual(s(i, j), table[i, j]);
                    Assert.AreEqual(s(j, i), table[j, i]);
                }
            }
        }
    }
}