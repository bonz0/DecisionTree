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
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }
            if (dataTable.IsEmpty)
            {
                throw new ArgumentException("An empty DataTable cannot be trained.");
            }

            var classCounts = dataTable.GetClassCounts();
            var root = new DecisionTreeNode();

            this.columnNames = dataTable.Dimensions;
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
                var splitParams = dataTable.DecideSplitParams();
                root.SplitDetails = splitParams;
                var tableSplit = dataTable.Split(splitParams.DimensionIndex, splitParams.Value);
                if (!tableSplit.SplitTable.IsEmpty)
                {
                    var firstChildNode = train(tableSplit.SplitTable, new DecisionTreeNode());
                    root.FirstChildDetails = new Tuple<string, DecisionTreeNode>(splitParams.Value, firstChildNode);
                }
                if (!tableSplit.UnSplitTable.IsEmpty)
                {
                    root.SecondChild = train(tableSplit.UnSplitTable, new DecisionTreeNode());
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
            if (dataRow == null)
            {
                throw new ArgumentNullException(nameof(dataRow));
            }
            if (!IsTrained)
            {
                throw new InvalidOperationException("DecisionTree cannot predict without being trained");
            }

            /// If this node is a leaf node, then we have a successful prediction.
            if (root.IsLeaf)
            {
                return root.ClassLabel;
            }

            /// Get the split dimension's value and see if first child has that 
            /// value, if it does, then recurse into the first child to predict.
            var value = dataRow.GetValueAtIndex(root.SplitDimensionIndex);
            if (root.HasFirstChild(value))
            {
                return predict(dataRow, root.FirstChild);
            }
            /// If the first child's value doesn't match, then see if second child
            /// exists. If it does, then recurse into second child to predict.
            else if (root.SecondChild != null)
            {
                return predict(dataRow, root.SecondChild);
            }
            /// If the seecond doesn't exist, then predict the most frequest class label.
            return mostFrequentClassLabel;
        }
    }
}
