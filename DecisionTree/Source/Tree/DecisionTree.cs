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

        public DecisionTreeNode buildDecisionTree(DataTable data)
        {
            if (data.IsEmpty)
            {
                return null;
            }
        }
    }
}
