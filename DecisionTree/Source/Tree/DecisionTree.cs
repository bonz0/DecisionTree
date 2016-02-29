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
        DecisionTreeNode root;

        public DecisionTreeNode BuildDecisionTree(DecisionTreeNode root, DataTable data)
        {
            throw new NotImplementedException();

            if (data.IsEmpty)
            {
                return null;
            }
            else if (data.IsHomogeneous)
            {
                root.ClassLabel = data.FirstClassLabel;
                return root;
            }
            else
            {
                int splittingColumnIndex = data.DecideSplittingParams();
                var attributeValueCounts = data.GetAttributeCountsForColumn(splittingColumnIndex);
                foreach (var attribute in attributeValueCounts)
                {
                    var node = new DecisionTreeNode(data.GetColumnName(splittingColumnIndex));
                    root.AddChild(attribute.Key, node);
                }
                return null;
            }
        }
    }
}
