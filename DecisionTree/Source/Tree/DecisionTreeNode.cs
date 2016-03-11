using DecisionTree.Source.Data;
using System;
using System.Collections.Generic;

namespace DecisionTree.Source.Tree
{
    internal class DecisionTreeNode
    {
        private string classLabel;
        private SplitParams splitDetails;
        Dictionary<string, DecisionTreeNode> children;

        public DecisionTreeNode()
        {
            this.children = new Dictionary<string, DecisionTreeNode>();
            this.splitDetails = null;
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

        public SplitParams SplitDetails
        {
            private get
            {
                return splitDetails;
            }

            set
            {
                splitDetails = value;
            }
        }

        public string SplitDimension
        {
            get
            {
                return splitDetails.Dimension;
            }
        }

        public int SplitDimensionIndex
        {
            get
            {
                return splitDetails.Item1;
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
