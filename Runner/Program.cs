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
            var trainingData = FileInputParser.ReadDataTableFromFile(@"C:\Users\fazia\Documents\DecisionTree\DecisionTree\DecisionTree\TestData\RestaurantTrainData.csv", ',');
            var testingData = FileInputParser.ReadDataTableFromFile(@"C:\Users\fazia\Documents\DecisionTree\DecisionTree\DecisionTree\TestData\RestaurantTestData.csv", ',');
            var decisionTree = new DecisionTree.Source.Tree.DecisionTree();
            decisionTree.Train(trainingData);
            IList<string> predictions = decisionTree.Predict(testingData);
            Console.WriteLine(predictions.Count);
            foreach (var prediction in predictions)
            {
                Console.WriteLine(prediction);
            }
            Console.ReadLine();
        }
    }
}
