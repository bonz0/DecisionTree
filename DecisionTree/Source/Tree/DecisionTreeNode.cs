using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Source.Tree
{
    public class DecisionTreeNode
    {
        string splittingColumn;
        Dictionary<string, DecisionTreeNode> children;

        public DecisionTreeNode(string splittingColumn)
        {
            this.splittingColumn = splittingColumn;
            this.children = new Dictionary<string, DecisionTreeNode>();
        }

        public string ClassLabel { get; set; }

        public string SplittingColumn
        {
            get
            {
                return splittingColumn;
            }
        }

        public bool HasChildren
        {
            get
            {
                return (children.Count != 0);
            }
        }

        public DecisionTreeNode GetChildForColumn(string column)
        {
            return children[column];
        }

        public bool HasChildForColumn(string column)
        {
            return children.ContainsKey(column);
        }

        public void AddChild(string attributeValue, DecisionTreeNode childNode)
        {
            this.children.Add(attributeValue, childNode);
        }
    }
}
