using DecisionTree.Source.Data;
using DecisionTree.Source.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DecisionTree.Tests.Utils
{
    [TestClass]
    public class FileInputParserTest
    {
        private const string testDataRelativePath = @"TestData\DeviceCoolData.csv";

        [TestMethod]
        public void TestFileParserHappyCase()
        {
            string rootDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            string testDataPath = Path.Combine(rootDirectory, testDataRelativePath);
            DataTable testData = FileInputParser.ReadDataTableFromFile(testDataPath, ',');
            Assert.AreEqual(4, testData.ColumnCount);
            Assert.AreEqual(16, testData.RowCount);
            Assert.IsFalse(testData.IsHomogeneous);
            Assert.IsFalse(testData.IsEmpty);
            Assert.AreEqual("Cool?", testData.ClassName);
        }
    }
}
