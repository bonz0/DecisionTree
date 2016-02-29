using DecisionTree.Source.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Source.Tree
{
    public class DecisionTree
    {
        private DecisionTreeNode rooteNode;
        private string mostFrequentClassLabel;
        private IList<string> columnNames;

        public bool IsTrained { get; private set; }

        public DecisionTree()
        {
            this.IsTrained = false;
            this.mostFrequentClassLabel = null;
            this.columnNames = null;
        }

        public void Train(DataTable dataTable)
        {
            if (dataTable.IsEmpty)
            {
                throw new ArgumentException("An empty DataTable cannot be trained.");
            }

            var classCounts = dataTable.GetClassCounts();
            var root = new DecisionTreeNode();

            this.columnNames = dataTable.ColumnNames;
            this.mostFrequentClassLabel = classCounts.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            this.rooteNode = train(dataTable, root);
            this.IsTrained = true;
        }

        private DecisionTreeNode train(DataTable dataTable, DecisionTreeNode root)
        {
            if (dataTable.IsHomogeneous)
            {
                root.ClassLabel = dataTable.FirstClassLabel;
                return root;
            }
            else
            {
                var splittingColumn = dataTable.DecideSplittingParams();
                Console.WriteLine($"Splitting column: {splittingColumn.Item2}");
                var attributeValueCounts = dataTable.GetAttributeCountsForColumn(splittingColumn.Item1);
                root.SplittingColumn = splittingColumn;
                foreach (var valueCount in attributeValueCounts)
                {
                    var prunedTable = dataTable.PruneTable(splittingColumn.Item1, valueCount.Key);
                    Console.WriteLine(prunedTable.ToString());
                    if (!prunedTable.IsEmpty)
                    {
                        var childNode = new DecisionTreeNode();
                        root.AddChild(valueCount.Key, train(prunedTable, childNode));
                    }
                }
                return root;
            }
        }

        public string Predict(DataRow dataRow)
        {
            return predict(dataRow, rooteNode);
        }

        public IList<string> Predict(DataTable table)
        {
            IList<string> predictions = table.Data
                .Select(d => Predict(d))
                .ToList();
            double correct = 0.0d;
            IList<string> classLabels = table.ClassLabels;
            for (int i = 0; i < predictions.Count; i++)
            {
                if (predictions[i].Equals(classLabels[i]))
                {
                    correct++;
                }
            }
            var size = (double)predictions.Count;
            Console.WriteLine($"Prediction accuracy: {(correct / size) * 100}");
            return predictions;
        }

        private string predict(DataRow dataRow, DecisionTreeNode root)
        {
            if (!IsTrained)
            {
                throw new InvalidOperationException("DecisionTree cannot predict without being trained");
            }

            if (root.IsLeaf)
            {
                return root.ClassLabel;
            }

            var attributeValue = dataRow.GetAttributeAtIndex(root.SplittingColumnIndex);

            //if (root.HasChild(attributeValue))
            //{
            //    return predict(dataRow, root.GetChild(attributeValue));
            //}
            //return mostFrequentClassLabel;
            return (root.HasChild(attributeValue))
                ? predict(dataRow, root.GetChild(attributeValue))
                : mostFrequentClassLabel;
        }
    }
}
