using Algebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurveFitting.Tests {
    [TestClass()]
    public class SumTableTests {
        [TestMethod()]
        public void IndexerTest() {
            SumTable table = new(new Vector(2, 3, 5, 7), new Vector(11, 13, 17, 19));

            Assert.AreEqual(4, table[0, 0]);
            Assert.AreEqual(2 + 3 + 5 + 7, table[1, 0]);
            Assert.AreEqual(2 * 2 + 3 * 3 + 5 * 5 + 7 * 7, table[2, 0]);
            Assert.AreEqual(2 * 2 * 2 + 3 * 3 * 3 + 5 * 5 * 5 + 7 * 7 * 7, table[3, 0]);

            Assert.AreEqual(11 + 13 + 17 + 19, table[0, 1]);
            Assert.AreEqual(2 * 11 + 3 * 13 + 5 * 17 + 7 * 19, table[1, 1]);
            Assert.AreEqual(2 * 2 * 11 + 3 * 3 * 13 + 5 * 5 * 17 + 7 * 7 * 19, table[2, 1]);
            Assert.AreEqual(2 * 2 * 2 * 11 + 3 * 3 * 3 * 13 + 5 * 5 * 5 * 17 + 7 * 7 * 7 * 19, table[3, 1]);

            Assert.AreEqual(11 * 11 + 13 * 13 + 17 * 17 + 19 * 19, table[0, 2]);
            Assert.AreEqual(2 * 11 * 11 + 3 * 13 * 13 + 5 * 17 * 17 + 7 * 19 * 19, table[1, 2]);
            Assert.AreEqual(2 * 2 * 11 * 11 + 3 * 3 * 13 * 13 + 5 * 5 * 17 * 17 + 7 * 7 * 19 * 19, table[2, 2]);
            Assert.AreEqual(2 * 2 * 2 * 11 * 11 + 3 * 3 * 3 * 13 * 13 + 5 * 5 * 5 * 17 * 17 + 7 * 7 * 7 * 19 * 19, table[3, 2]);

            Assert.AreEqual(11 * 11 * 11 + 13 * 13 * 13 + 17 * 17 * 17 + 19 * 19 * 19, table[0, 3]);
            Assert.AreEqual(2 * 11 * 11 * 11 + 3 * 13 * 13 * 13 + 5 * 17 * 17 * 17 + 7 * 19 * 19 * 19, table[1, 3]);
            Assert.AreEqual(2 * 2 * 11 * 11 * 11 + 3 * 3 * 13 * 13 * 13 + 5 * 5 * 17 * 17 * 17 + 7 * 7 * 19 * 19 * 19, table[2, 3]);
            Assert.AreEqual(2 * 2 * 2 * 11 * 11 * 11 + 3 * 3 * 3 * 13 * 13 * 13 + 5 * 5 * 5 * 17 * 17 * 17 + 7 * 7 * 7 * 19 * 19 * 19, table[3, 3]);
        }

        [TestMethod()]
        public void ReverseIndexerTest() {
            SumTable table = new(new Vector(2, 3, 5, 7), new Vector(11, 13, 17, 19));

            Assert.AreEqual(2 * 2 * 2 * 11 * 11 * 11 + 3 * 3 * 3 * 13 * 13 * 13 + 5 * 5 * 5 * 17 * 17 * 17 + 7 * 7 * 7 * 19 * 19 * 19, table[3, 3]);
            Assert.AreEqual(2 * 2 * 11 * 11 * 11 + 3 * 3 * 13 * 13 * 13 + 5 * 5 * 17 * 17 * 17 + 7 * 7 * 19 * 19 * 19, table[2, 3]);
            Assert.AreEqual(2 * 11 * 11 * 11 + 3 * 13 * 13 * 13 + 5 * 17 * 17 * 17 + 7 * 19 * 19 * 19, table[1, 3]);
            Assert.AreEqual(11 * 11 * 11 + 13 * 13 * 13 + 17 * 17 * 17 + 19 * 19 * 19, table[0, 3]);

            Assert.AreEqual(2 * 2 * 2 * 11 * 11 + 3 * 3 * 3 * 13 * 13 + 5 * 5 * 5 * 17 * 17 + 7 * 7 * 7 * 19 * 19, table[3, 2]);
            Assert.AreEqual(2 * 2 * 11 * 11 + 3 * 3 * 13 * 13 + 5 * 5 * 17 * 17 + 7 * 7 * 19 * 19, table[2, 2]);
            Assert.AreEqual(2 * 11 * 11 + 3 * 13 * 13 + 5 * 17 * 17 + 7 * 19 * 19, table[1, 2]);
            Assert.AreEqual(11 * 11 + 13 * 13 + 17 * 17 + 19 * 19, table[0, 2]);

            Assert.AreEqual(2 * 2 * 2 * 11 + 3 * 3 * 3 * 13 + 5 * 5 * 5 * 17 + 7 * 7 * 7 * 19, table[3, 1]);
            Assert.AreEqual(2 * 2 * 11 + 3 * 3 * 13 + 5 * 5 * 17 + 7 * 7 * 19, table[2, 1]);
            Assert.AreEqual(2 * 11 + 3 * 13 + 5 * 17 + 7 * 19, table[1, 1]);
            Assert.AreEqual(11 + 13 + 17 + 19, table[0, 1]);

            Assert.AreEqual(2 * 2 * 2 + 3 * 3 * 3 + 5 * 5 * 5 + 7 * 7 * 7, table[3, 0]);
            Assert.AreEqual(2 * 2 + 3 * 3 + 5 * 5 + 7 * 7, table[2, 0]);
            Assert.AreEqual(2 + 3 + 5 + 7, table[1, 0]);
            Assert.AreEqual(4, table[0, 0]);

        }
    }
}