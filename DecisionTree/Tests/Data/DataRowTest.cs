
namespace DecisionTree.Tests.Data
{
    using DecisionTree.Source.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataRowTest
    {
        [TestMethod]
        public void GeneralDataRowTest()
        {
            string[] dataWithClassLabel = { "iphone", "twitter", "mac", "yes" };
            DataRow dataRow1 = new DataRow(dataWithClassLabel);
            Assert.AreEqual("yes", dataRow1.ClassLabel);
            Assert.AreEqual(4, dataRow1.Count);
            Assert.AreEqual("iphone", dataRow1.GetValueAtIndex(0));
            Assert.AreEqual("twitter", dataRow1.GetValueAtIndex(1));
            Assert.AreEqual("mac", dataRow1.GetValueAtIndex(2));

            string[] data = { "iphone", "twitter", "mac", "yes" };
            DataRow dataRow2 = new DataRow(data);
            Assert.AreEqual("yes", dataRow2.ClassLabel);
            Assert.AreEqual(4, dataRow2.Count);
            Assert.AreEqual("iphone", dataRow2.GetValueAtIndex(0));
            Assert.AreEqual("twitter", dataRow2.GetValueAtIndex(1));
            Assert.AreEqual("mac", dataRow2.GetValueAtIndex(2));

            Assert.IsTrue(dataRow1.Equals(dataRow2));
            Assert.AreEqual(dataRow1.GetHashCode(), dataRow2.GetHashCode());
        }
    }
}
