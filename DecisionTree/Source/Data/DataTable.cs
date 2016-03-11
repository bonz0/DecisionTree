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
    /// | dim1 | dim2 | className | -> dimensions
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
        private readonly IList<string> dimensions;
        private readonly IList<DataRow> dataRows;
        private readonly double entropy;

        #region Ctors
        public DataTable(IList<string> dimensions, IList<DataRow> dataRows)
        {
            validateArgs(dimensions.Count, dataRows);
            this.dimensions = dimensions;
            this.dataRows = dataRows;
            this.entropy = calculateTableEntropy();
        }

        public DataTable(IList<string> dimensions, IList<string[]> dataRows)
        {
            this.dimensions = dimensions;
            this.dataRows = new List<DataRow>();
            foreach (var d in dataRows)
            {
                this.dataRows.Add(new DataRow(d));
            }
            this.entropy = calculateTableEntropy();
            validateArgs(this.dimensions.Count, this.dataRows);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gives the number of columns in the table (including the class label).
        /// </summary>
        internal int DimensionCount
        {
            get
            {
                return dimensions.Count;
            }
        }

        internal IList<string> ClassLabels
        {
            get
            {
                return dataRows.Select(d => d.ClassLabel).ToList();
            }
        }

        /// <summary>
        /// Gives the number of rows of data in the table.
        /// </summary>
        internal int DataCount
        {
            get
            {
                return dataRows.Count;
            }
        }

        /// <summary>
        /// The name of the class to predict.
        /// </summary>
        internal string ClassName
        {
            get
            {
                return dimensions.Last();
            }
        }

        /// <summary>
        /// Determines the entropy of the data table.
        /// The entropy of a homogeneous table is 0.0.
        /// The entropy of an evenly split table is 1.0.
        /// </summary>
        internal double Entropy
        {
            get
            {
                return entropy;
            }
        }

        /// <summary>
        /// Determines if all the data rows of the table have the same class label.
        /// </summary>
        internal bool IsHomogeneous
        {
            get
            {
                if (DataCount == 0)
                {
                    return true;
                }
                var firstClass = dataRows.First().ClassLabel;
                return dataRows.All(d => d.ClassLabel.Equals(firstClass));
            }
        }

        /// <summary>
        /// Determines if the data table is empty
        /// </summary>
        internal bool IsEmpty
        {
            get
            {
                return DataCount == 0;
            }
        }

        internal string FirstClassLabel
        {
            get
            {
                return dataRows.First().ClassLabel;
            }
        }

        internal IList<string> Dimensions
        {
            get
            {
                return dimensions;
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
        internal string GetDimension(int dimensionIndex)
        {
            return dimensions[dimensionIndex];
        }

        internal DataTable PruneTable(string dimension, string value)
        {
            var columnIndex = dimensions.IndexOf(dimension);
            return Split(columnIndex, value);
        }

        internal DataTable Split(int dimensionIndex, string value)
        {
            if (dimensionIndex == DimensionCount - 1)
            {
                throw new ArgumentException("Cannot prune DataTable based on the class.");
            }
            var dataWithValue = dataRows
                .Where(r => r.GetValueAtIndex(dimensionIndex).Equals(value))
                .Select(d => d.RemoveAttributeAt(dimensionIndex))
                .ToList();

            var clonedDimensions = new List<string>(dimensions);
            clonedDimensions.RemoveAt(dimensionIndex);
            return new DataTable(clonedDimensions, dataWithValue);
        }

        internal IDictionary<string, int> GetClassCounts()
        {
            return GetValueCountsForDimension(DimensionCount - 1);
        }

        internal IDictionary<string, int> GetValueCountsForDimension(int columnIndex)
        {
            var counts = new Dictionary<string, int>();
            foreach (var d in dataRows)
            {
                string attribute = d.GetValueAtIndex(columnIndex);
                counts[attribute] = (counts.ContainsKey(attribute)) ? counts[attribute] + 1 : 1;
            }
            return counts;
        }

        internal SplitParams DecideSplitParams()
        {
            var minDimensionEntropy = double.MaxValue;
            SplitParams splitParams = null;
            for (int i = 0; i < DimensionCount - 1; i++)
            {
                var valueCounts = GetValueCountsForDimension(i);
                var dimensionEntropy = 0.0d;
                string minEntropyValue = null;
                var minValueEntropy = double.MaxValue;
                foreach (var value in valueCounts.Keys)
                {
                    var prunedTable = Split(i, value);
                    var prunedTableEntropy = prunedTable.Entropy;
                    var valueCount = (double)valueCounts[value];
                    var sizeOfCurrentTable = (double)DataCount;
                    dimensionEntropy += (valueCount / sizeOfCurrentTable) * prunedTableEntropy;
                    if (prunedTableEntropy < minValueEntropy)
                    {
                        minEntropyValue = value;
                    }
                }

                if (dimensionEntropy <= minDimensionEntropy)
                {
                    minDimensionEntropy = dimensionEntropy;
                    splitParams = new SplitParams(i, GetDimension(i), minEntropyValue);
                }
            }
            return splitParams;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join("\t", dimensions));
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

            return (dimensions.SequenceEqual(other.dimensions)
                && dataRows.SequenceEqual(other.dataRows));
        }

        public override int GetHashCode()
        {
            var hash = 19;
            foreach (var c in dimensions)
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
            var size = (double)DataCount;
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
