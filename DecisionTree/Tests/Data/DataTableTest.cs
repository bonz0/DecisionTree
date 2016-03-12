

namespace DecisionTree.Tests.Data
{
    #region Using
    using DecisionTree.Source.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Source.Utils;
    using System;
    using System.Collections.Generic;
    #endregion

    [TestClass]
    public class DataTableTest
    {

        #region TestMethods
        [TestMethod]
        public void TestSampleData()
        {
            DataTable dataTable = getSampleData();

            Assert.AreEqual(3, dataTable.DimensionCount);
            Assert.AreEqual(6, dataTable.DataCount);
            Assert.IsTrue(0.9183d - dataTable.Entropy < 0.00001d);
            Assert.IsFalse(dataTable.IsHomogeneous);
        }

        [TestMethod]
        public void TestHomogeneousData()
        {
            string[] columnNames = { "Device", "Network", "Sex", "Cool?" };
            string[] row1 = { "ipod", "twitter", "m", "yes" };
            string[] row2 = { "iphone", "facebook", "f", "yes" };
            IList<string[]> dataRows = new List<string[]>();
            dataRows.Add(row1);
            dataRows.Add(row2);
            DataTable dataTable = new DataTable(columnNames, dataRows);

            Assert.AreEqual(4, dataTable.DimensionCount);
            Assert.AreEqual(2, dataTable.DataCount);
            Assert.AreEqual(0.0d, dataTable.Entropy);
            Assert.IsTrue(dataTable.IsHomogeneous);
        }

        [TestMethod]
        public void TestHalfSplitTable()
        {
            string[] columnNames = { "Device", "Cool?" };
            string[] row1 = { "ipod", "yes" };
            string[] row2 = { "iphone", "no" };
            IList<string[]> dataRows = new List<string[]>();
            dataRows.Add(row1);
            dataRows.Add(row2);
            DataTable dataTable = new DataTable(columnNames, dataRows);

            Assert.AreEqual(2, dataTable.DimensionCount);
            Assert.AreEqual(2, dataTable.DataCount);
            Assert.AreEqual(1.0d, dataTable.Entropy);
            Assert.IsFalse(dataTable.IsHomogeneous);
        }

        [TestMethod]
        public void TestInvalidArgsException()
        {
            string[] columnNames = { "Device", "Cool?" };
            string[] row1 = { "ipod" };
            string[] row2 = { "iphone", "no" };
            IList<string[]> dataRows = new List<string[]>();
            dataRows.Add(row1);
            dataRows.Add(row2);

            try
            {
                DataTable dataTable = new DataTable(columnNames, dataRows);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void TestPruneTable()
        {
            DataTable dataTable = getSampleData();

            //DataTable twitterTable = dataTable.PruneTable("Network", "twitter");
            //Assert.AreEqual(4, twitterTable.DataCount);
            //Assert.AreEqual(2, twitterTable.DimensionCount);
            //Assert.IsTrue(twitterTable.IsHomogeneous);

            //DataTable facebookTable = dataTable.PruneTable("Network", "facebook");
            //Assert.AreEqual(2, facebookTable.DataCount);
            //Assert.AreEqual(2, facebookTable.DimensionCount);
            //Assert.IsTrue(facebookTable.IsHomogeneous);

            //try
            //{
            //    DataTable twitterFacebookTable = twitterTable.PruneTable("Network", "facebook");
            //    Assert.Fail();
            //}
            //catch (Exception e)
            //{
            //    Assert.IsInstanceOfType(e, typeof(ArgumentOutOfRangeException));
            //}

            //DataTable ipodTable = dataTable.Split(0, "ipod");
            //Assert.AreEqual(3, ipodTable.DataCount);
            //Assert.AreEqual(2, ipodTable.DimensionCount);
            //Assert.IsTrue(ipodTable.IsHomogeneous);

            //DataTable ipodTwitterTbale = twitterTable.Split(0, "ipod");
            //Assert.AreEqual(3, ipodTwitterTbale.DataCount);
            //Assert.AreEqual(1, ipodTwitterTbale.DimensionCount);
            //Assert.IsTrue(ipodTwitterTbale.IsHomogeneous);

            //DataTable iPhoneTwitterTable = twitterTable.Split(0, "iphone");
            //Assert.AreEqual(1, iPhoneTwitterTable.DataCount);
            //Assert.AreEqual(1, iPhoneTwitterTable.DimensionCount);
            //Assert.IsTrue(ipodTable.IsHomogeneous);
        }

        [TestMethod]
        public void TestPruneTableBasedOnClass()
        {
            DataTable dataTable = getSampleData();
            //try
            //{
            //    DataTable classPrunedTable = dataTable.PruneTable("Cool?", "no");
            //    Assert.Fail();
            //}
            //catch (Exception e)
            //{
            //    Assert.IsInstanceOfType(e, typeof(ArgumentException));
            //}

            //try
            //{
            //    DataTable classPrunedTable = dataTable.Split(dataTable.DimensionCount - 1, "no");
            //    Assert.Fail();
            //}
            //catch (Exception e)
            //{
            //    Assert.IsInstanceOfType(e, typeof(ArgumentException));
            //}
        }

        [TestMethod]
        public void TestClassCounts()
        {
            DataTable dataTable = getSampleData();
            IDictionary<string, int> classCounts = dataTable.GetClassCounts();
            Assert.AreEqual(4, classCounts["no"]);
            Assert.AreEqual(2, classCounts["yes"]);
        }

        [TestMethod]
        public void TestDecideSplittingParams()
        {
            //DataTable dataTable = FileInputParser.ReadDataTableFromFile()
            //var splittingParams = dataTable.DecideSplitParams();
            //Console.WriteLine(splittingParams);
        }
        #endregion

        #region HelperMethods
        public static DataTable getSampleData()
        {
            string[] columnNames = { "Device", "Network", "Cool?" };
            string[] row1 = { "ipod", "twitter", "no" };
            string[] row2 = { "iphone", "facebook", "yes" };
            string[] row3 = { "ipod", "twitter", "no" };
            string[] row4 = { "iphone", "twitter", "no" };
            string[] row5 = { "none", "facebook", "yes" };
            string[] row6 = { "ipod", "twitter", "no" };
            IList<string[]> dataRows = new List<string[]>();
            dataRows.Add(row1);
            dataRows.Add(row2);
            dataRows.Add(row3);
            dataRows.Add(row4);
            dataRows.Add(row5);
            dataRows.Add(row6);
            return new DataTable(columnNames, dataRows);
        }
        #endregion
    }
}
