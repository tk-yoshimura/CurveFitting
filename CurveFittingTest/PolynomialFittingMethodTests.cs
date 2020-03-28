using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algebra;

namespace CurveFitting.Tests {
    [TestClass()]
    public class PolynomialFittingMethodTests {
        [TestMethod()]
        public void ExecuteFittingTest() {
            double[] x_list = { 1, 3, 4, 7, 8, 9, 13, 15, 20 };
            Vector p1 = new Vector(2, -1, 1, 5), p2 = new Vector(4, 3, -1);
 
            FittingData[] data_list1 = new FittingData[x_list.Length], data_list2 = new FittingData[x_list.Length];

            for(int i = 0; i < x_list.Length; i++) {
                double x = x_list[i];

                data_list1[i].X = data_list2[i].X = x;
                data_list1[i].Y = p1[0] + p1[1] * x + p1[2] * x * x + p1[3] * x * x * x;
                data_list2[i].Y = p2[0] * x + p2[1] * x * x + p2[2] * x * x * x;
            }
            
            PolynomialFittingMethod fitting1 = new PolynomialFittingMethod(data_list1, 3, true);
            PolynomialFittingMethod fitting2 = new PolynomialFittingMethod(data_list2, 3, false);

            Assert.AreEqual((fitting1.ExecuteFitting() - p1).Norm, 0, 1e-8);
            Assert.AreEqual((fitting2.ExecuteFitting() - p2).Norm, 0, 1e-8);
        }
    }
}