using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Source.Tree
{
    internal class DecisionTreeNode
    {
        string splittingColumn;
        string classLabel;
        Dictionary<string, DecisionTreeNode> children;

        public DecisionTreeNode(string splittingColumn, string classLabel)
        {
            this.splittingColumn = splittingColumn;
            this.classLabel = classLabel;
            this.children = new Dictionary<string, DecisionTreeNode>();
        }

        public string ClassLabel
        {
            get
            {
                return classLabel;
            }
        }

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

        public void AddChild(string column, DecisionTreeNode childNode)
        {
            this.children.Add(column, childNode);
        }
    }
}
