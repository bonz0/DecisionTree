using DecisionTree.Source.Data;
using DecisionTree.Source.Utils;
using System;
using System.Collections.Generic;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dataTable = FileInputParser.ReadDataTableFromFile(@"C:\Users\Harry\Documents\Visual Studio 2015\Projects\DecisionTree\DecisionTree\TestData\DeviceCoolData.csv", ',');
            DataTable iPodPrunedTable = dataTable.PruneTable("Device", "ipod");
            DataTable iPhonePrunedTable = dataTable.PruneTable("Device", "iphone");
            DataTable nonePrunedTable = dataTable.PruneTable("Device", "none");
            DataTable twitterPrunedTable = dataTable.PruneTable("Network", "twitter");
            DataTable fbPrunedTable = dataTable.PruneTable("Network", "facebook");
            DataTable pcPrunedTable = dataTable.PruneTable("Laptop", "pc");
            DataTable macPrunedTable = dataTable.PruneTable("Laptop", "mac");
            Console.WriteLine(nameof(dataTable));
            Console.WriteLine(dataTable.ToString());
            Console.WriteLine(nameof(iPodPrunedTable));
            Console.WriteLine(iPodPrunedTable.ToString());
            Console.WriteLine(nameof(iPhonePrunedTable));
            Console.WriteLine(iPhonePrunedTable.ToString());
            Console.WriteLine(nameof(nonePrunedTable));
            Console.WriteLine(nonePrunedTable.ToString());
            Console.WriteLine(nameof(twitterPrunedTable));
            Console.WriteLine(twitterPrunedTable.ToString());
            Console.WriteLine(nameof(fbPrunedTable));
            Console.WriteLine(fbPrunedTable.ToString());
            Console.WriteLine(nameof(pcPrunedTable));
            Console.WriteLine(pcPrunedTable.ToString());
            Console.WriteLine(nameof(macPrunedTable));
            Console.WriteLine(macPrunedTable.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Split on: {dataTable.DecideSplittingParams()}");
            double[] entropies = new double[]
            {
                dataTable.Entropy,
                iPodPrunedTable.Entropy,
                iPhonePrunedTable.Entropy,
                nonePrunedTable.Entropy,
                twitterPrunedTable.Entropy,
                fbPrunedTable.Entropy,
                pcPrunedTable.Entropy,
                macPrunedTable.Entropy,
            };
            Console.ReadKey();

            string[] columnNames = { "Device", "Laptop", "Cool?" };
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
            DataTable newDataTable = new DataTable(columnNames, dataRows);
            Console.WriteLine(dataTable.Entropy);
            Console.ReadKey();
        }
    }
}
