
namespace DecisionTree.Source.Data
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    /// <summary>
    /// 
    /// DataTable:
    /// |-------------|
    /// | columnNames | 
    /// |             |-----------|
    /// | col1 | col2 | className |
    /// |-------------------------|
    /// | val1   valA   class1    | -> DataRow
    /// |-------------------------|
    /// | val2   valB   class2    | -> DataRow
    /// |-------------------------|
    /// </summary>
    public class DataTable
    {
        private readonly IList<string> columnNames;
        private readonly IList<DataRow> dataRows;
        private readonly double entropy;

        #region Ctors
        public DataTable(IList<string> columnNames, IList<DataRow> dataRows)
        {
            validateArgs(columnNames.Count, dataRows);
            this.columnNames = columnNames;
            this.dataRows = dataRows;
            this.entropy = calculateEntropy();
        }

        public DataTable(IList<string> columnNames, IList<string[]> dataRows)
        {
            this.columnNames = columnNames;
            this.dataRows = new List<DataRow>();
            foreach (var d in dataRows)
            {
                this.dataRows.Add(new DataRow(d));
            }
            this.entropy = calculateEntropy();
            validateArgs(this.columnNames.Count, this.dataRows);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gives the number of columns in the table (including the class label).
        /// </summary>
        public int ColumnCount
        {
            get
            {
                return columnNames.Count;
            }
        }

        /// <summary>
        /// Gives the number of rows of data in the table.
        /// </summary>
        public int RowCount
        {
            get
            {
                return dataRows.Count;
            }
        }

        /// <summary>
        /// The name of the class to predict.
        /// </summary>
        public string ClassName
        {
            get
            {
                return columnNames.Last();
            }
        }

        /// <summary>
        /// Determines the entropy of the data table.
        /// The entropy of a homogeneous table is 0.0.
        /// The entropy of an evenly split table is 1.0.
        /// </summary>
        public double Entropy
        {
            get
            {
                return entropy;
            }
        }

        /// <summary>
        /// Determines if all the data rows of the table have the same class label.
        /// </summary>
        public bool IsHomogeneous
        {
            get
            {
                if (RowCount == 0)
                {
                    return true;
                }
                string firstClass = dataRows[0].ClassLabel;
                return dataRows.All(d => d.ClassLabel.Equals(firstClass));
            }
        }

        /// <summary>
        /// Determines if the data table is empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return RowCount == 0;
            }
        }
        #endregion

        #region PublicMethods
        public DataTable PruneTable(string columnName, string attributeValue)
        {
            int columnIndex = columnNames.IndexOf(columnName);
            return PruneTable(columnIndex, attributeValue);
        }

        public DataTable PruneTable(int columnIndex, string attributeValue)
        {
            IList<string> clonedColumns = new List<string>(columnNames);
            IList<DataRow> prunedData = dataRows
                .Where(r => !r.GetAttributeAtIndex(columnIndex).Equals(attributeValue))
                .ToList();
            if (prunedData.Count == 0)
            {
                clonedColumns.RemoveAt(columnIndex);
            }
            return new DataTable(clonedColumns, prunedData);
        }

        internal IDictionary<string, int> GetAttributeCountsForColumn(int columnIndex)
        {
            var counts = new Dictionary<string, int>();
            foreach (var d in dataRows)
            {
                string attribute = d.GetAttributeAtIndex(columnIndex);
                counts[attribute] = (counts.ContainsKey(attribute)) ? counts[attribute] + 1 : 1;
            }
            return counts;
        }

        public Tuple<int, string> DecideSplittingParams()
        {
            double minEntropy = double.MaxValue;
            Tuple<int, string> columnAttributePair = null;
            for (int i = 0; i < ColumnCount - 1; i++)
            {
                IDictionary<string, int> attributeCounts = GetAttributeCountsForColumn(i);
                foreach (var attribute in attributeCounts.Keys)
                {
                    var prunedTable = PruneTable(i, attribute);
                    if (prunedTable.Entropy < minEntropy)
                    {
                        minEntropy = prunedTable.Entropy;
                        columnAttributePair = new Tuple<int, string>(i, attribute);
                    }
                }
            }
            return columnAttributePair;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", columnNames));
            foreach (var dataRow in dataRows)
            {
                sb.AppendLine(dataRow.ToString());
            }
            sb.AppendLine($"Entropy: {Entropy}");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            DataTable other = obj as DataTable;

            if (other == null)
            {
                return false;
            }

            return (columnNames.SequenceEqual(other.columnNames)
                && dataRows.SequenceEqual(other.dataRows));
        }

        public override int GetHashCode()
        {
            int hash = 19;
            foreach (var c in columnNames)
            {
                hash = hash * 31 + c.GetHashCode();
            }
            foreach (var d in dataRows)
            {
                hash = hash * 31 + d.GetHashCode();
            }
            return hash;
        }
        #endregion

        #region PrivateMethods
        internal IDictionary<string, int> getClassCounts()
        {
            return GetAttributeCountsForColumn(ColumnCount - 1);
        }

        private void validateArgs(int numColumns, IEnumerable<DataRow> dataRows)
        {
            if (dataRows.Any(d => d.Count != numColumns))
            {
                throw new ArgumentException("Number of columns doesn't match the number of columns in dataRows");
            }
        }

        internal bool isColumnHomogeneous(int columnIndex)
        {
            if (dataRows.Count == 0)
            {
                return true;
            } 
            string attribute = dataRows[0].GetAttributeAtIndex(columnIndex);
            return dataRows.All(d => attribute.Equals(d.GetAttributeAtIndex(columnIndex)));
        }

        private double calculateEntropy()
        {
            double entropy = 0.0d;
            IDictionary<string, int> classCounts = getClassCounts();
            double size = (double)RowCount;
            foreach (var c in classCounts)
            {
                double fraction = (c.Value / size);
                double logOfFraction = Math.Log(fraction, 2.0d);
                entropy -= (fraction * logOfFraction);
            }
            return entropy;
        }
        #endregion
    }
}
