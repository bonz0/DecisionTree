
namespace DecisionTree.Source.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public class DataRow
    {
        private IList<string> data;

        public DataRow(string[] dataRow)
        {
            this.data = dataRow.ToList();
        }

        public DataRow(DataRow rowToClone)
        {
            this.data = new List<string>(rowToClone.data);
        }

        public string ClassLabel
        {
            get
            {
                return data.Last();
            }
        }

        public int Count
        {
            get
            {
                return data.Count;
            }
        }

        public string GetValueAtIndex(int index)
        {
            return data[index];
        }

        public DataRow RemoveAttributeAt(int index)
        {
            var dataRow = new DataRow(this);
            dataRow.data.RemoveAt(index);
            return dataRow;
        }

        public override string ToString()
        {
            return string.Join("\t", data);
        }

        public override bool Equals(object obj)
        {
            var objDataRow = obj as DataRow;

            if (objDataRow == null)
            {
                return false;
            }

            return this.data.SequenceEqual(objDataRow.data);
        }

        public override int GetHashCode()
        {
            var hash = 19;
            foreach (var datum in data)
            {
                hash = hash * 31 + datum.GetHashCode();
            }
            return hash;
        }
    }
}
