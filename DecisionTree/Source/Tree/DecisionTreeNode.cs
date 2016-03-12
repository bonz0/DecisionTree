using DecisionTree.Source.Data;
using System;
using System.Collections.Generic;

namespace DecisionTree.Source.Tree
{
    internal class DecisionTreeNode
    {
        private string classLabel;
        private SplitParams splitDetails;
        Tuple<string, DecisionTreeNode> firstChildDetails;
        DecisionTreeNode secondChild;

        public DecisionTreeNode()
        {
            this.splitDetails = null;
            this.classLabel = null;
            this.firstChildDetails = null;
            this.secondChild = null;
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

        internal SplitParams SplitDetails
        {
            get
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
                return splitDetails.DimensionIndex;
            }
        }

        internal bool IsLeaf
        {
            get
            {
                return classLabel != null;
            }
        }

        internal DecisionTreeNode FirstChild
        {
            get
            {
                return firstChildDetails.Item2;
            }
        }

        internal Tuple<string, DecisionTreeNode> FirstChildDetails
        {
            set
            {
                firstChildDetails = value;
            }
        }

        internal DecisionTreeNode SecondChild
        {
            get
            {
                return secondChild;
            }

            set
            {
                secondChild = value;
            }
        }

        public bool HasFirstChild(string value)
        {
            return firstChildDetails.Item1.Equals(value);
        }

        public void SetFirstChildDetails(string value, DecisionTreeNode firstChild)
        {
            this.firstChildDetails = new Tuple<string, DecisionTreeNode>(value, firstChild);
        }
    }
}
