using System;
using System.Collections.Generic;

namespace DecisionTree.Source.Tree
{
    internal class DecisionTreeNode
    {
        private string classLabel;
        private Tuple<int, string> splittingColumn;
        Dictionary<string, DecisionTreeNode> children;

        public DecisionTreeNode()
        {
            this.children = new Dictionary<string, DecisionTreeNode>();
            this.splittingColumn = null;
            this.classLabel = null;
        }

        public string ClassLabel
        {
            get
            {
                return classLabel;
            }

            set
            {
                classLabel = value;
            }
        }

        public Tuple<int, string> SplittingColumn
        {
            private get
            {
                return splittingColumn;
            }

            set
            {
                splittingColumn = value;
            }
        }

        public string SplittingColumnName
        {
            get
            {
                return splittingColumn.Item2;
            }
        }

        public int SplittingColumnIndex
        {
            get
            {
                return splittingColumn.Item1;
            }
        }

        internal bool IsLeaf
        {
            get
            {
                return classLabel != null;
            }
        }

        public bool HasChildren
        {
            get
            {
                return (children.Count != 0);
            }
        }

        public DecisionTreeNode GetChild(string attributeName)
        {
            return children[attributeName];
        }

        public bool HasChild(string attributename)
        {
            return children.ContainsKey(attributename);
        }

        public void AddChild(string attributeValue, DecisionTreeNode childNode)
        {
            this.children.Add(attributeValue, childNode);
        }
    }
}
