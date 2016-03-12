using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Source.Data
{
    internal class DataTableSplit
    {
        private readonly DataTable splitTable;
        private readonly DataTable unSplitTable;

        public DataTableSplit(DataTable splitTable, DataTable unSplitTable)
        {
            this.splitTable = splitTable;
            this.unSplitTable = unSplitTable;
        }

        public DataTable SplitTable
        {
            get
            {
                return splitTable;
            }
        }

        public DataTable UnSplitTable
        {
            get
            {
                return unSplitTable;
            }
        }
    }
}
