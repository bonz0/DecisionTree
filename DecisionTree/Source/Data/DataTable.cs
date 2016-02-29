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
    /// |-------------------------|
    /// | col1 | col2 | className | -> columnNames
    /// |-------------------------|
    /// | val1 | valA | class1    | -> DataRow
    /// |-------------------------|
    /// | val2 | valB | class2    | -> DataRow
    /// |-------------------------|
    /// | val1 | valB | class2    | -> DataRow
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
            this.entropy = calculateTableEntropy();
        }

        public DataTable(IList<string> columnNames, IList<string[]> dataRows)
        {
            this.columnNames = columnNames;
            this.dataRows = new List<DataRow>();
            foreach (var d in dataRows)
            {
                this.dataRows.Add(new DataRow(d));
            }
            this.entropy = calculateTableEntropy();
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

        public IList<string> ClassLabels
        {
            get
            {
                return dataRows.Select(d => d.ClassLabel).ToList();
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
                var firstClass = dataRows[0].ClassLabel;
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

        public string FirstClassLabel
        {
            get
            {
                return dataRows.First().ClassLabel;
            }
        }

        internal IList<string> ColumnNames
        {
            get
            {
                return columnNames;
            }
        }

        internal IList<DataRow> Data
        {
            get
            {
                return dataRows;
            }
        }
        #endregion

        #region PublicMethods
        public string GetColumnName(int columnIndex)
        {
            return columnNames[columnIndex];
        }

        public DataTable PruneTable(string columnName, string attributeValue)
        {
            var columnIndex = columnNames.IndexOf(columnName);
            return PruneTable(columnIndex, attributeValue);
        }

        public DataTable PruneTable(int columnIndex, string attributeValue)
        {
            if (columnIndex == ColumnCount - 1)
            {
                throw new ArgumentException("Cannot prune DataTable based on the class.");
            }
            var clonedColumns = new List<string>(columnNames);
            clonedColumns.RemoveAt(columnIndex);
            var prunedData = dataRows
                .Where(r => r.GetAttributeAtIndex(columnIndex).Equals(attributeValue))
                .Select(d => d.RemoveAttributeAt(columnIndex))
                .ToList();
            return new DataTable(clonedColumns, prunedData);
        }

        public IDictionary<string, int> GetClassCounts()
        {
            return GetAttributeCountsForColumn(ColumnCount - 1);
        }

        public IDictionary<string, int> GetAttributeCountsForColumn(int columnIndex)
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
            var minEntropy = double.MaxValue;
            Tuple<int, string> splittingColumn = null;
            for (int i = 0; i < ColumnCount - 1; i++)
            {
                var attributeCounts = GetAttributeCountsForColumn(i);
                double columnEntropy = 0.0d;
                foreach (var attribute in attributeCounts.Keys)
                {
                    var prunedTable = PruneTable(i, attribute);
                    var prunedTableEntropy = prunedTable.Entropy;
                    var attributeCount = (double)attributeCounts[attribute];
                    var sizeOfCurrentTable = (double)RowCount;
                    columnEntropy += (attributeCount / sizeOfCurrentTable) * prunedTableEntropy;
                }

                if (columnEntropy <= minEntropy)
                {
                    minEntropy = columnEntropy;
                    splittingColumn = new Tuple<int, string>(i, GetColumnName(i));
                }
            }
            return splittingColumn;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join("\t", columnNames));
            foreach (var dataRow in dataRows)
            {
                sb.AppendLine(dataRow.ToString());
            }
            sb.AppendLine($"Entropy: {Entropy}");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as DataTable;

            if (other == null)
            {
                return false;
            }

            return (columnNames.SequenceEqual(other.columnNames)
                && dataRows.SequenceEqual(other.dataRows));
        }

        public override int GetHashCode()
        {
            var hash = 19;
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
        private void validateArgs(int numColumns, IEnumerable<DataRow> dataRows)
        {
            if (dataRows.Any(d => d.Count != numColumns))
            {
                throw new ArgumentException("Number of columns doesn't match the number of columns in dataRows");
            }
        }

        private double calculateTableEntropy()
        {
            var entropy = 0.0d;
            var classCounts = GetClassCounts();
            var size = (double)RowCount;
            foreach (var c in classCounts)
            {
                var fraction = (c.Value / size);
                var logOfFraction = Math.Log(fraction, 2.0d);
                entropy -= (fraction * logOfFraction);
            }
            return entropy;
        }
        #endregion
    }
}
