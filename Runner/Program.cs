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
            var trainingData = FileInputParser.ReadDataTableFromFile(@"C:\Users\Harry\Documents\Visual Studio 2015\Projects\DecisionTree\DecisionTree\TestData\RestaurantTrainData.csv", ',');
            var testingData = FileInputParser.ReadDataTableFromFile(@"C:\Users\Harry\Documents\Visual Studio 2015\Projects\DecisionTree\DecisionTree\TestData\RestaurantTestData.csv", ',');
            Console.WriteLine("First splittin column: " + trainingData.DecideSplittingParams().Item2);
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
