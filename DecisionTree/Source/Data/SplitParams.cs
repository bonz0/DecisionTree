using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Source.Data
{
    internal class SplitParams
    {
        private int dimensionIndex;
        private string dimension;
        private string value;

        public SplitParams(int dimensionIndex, string dimension, string value)
        {
            this.dimensionIndex = dimensionIndex;
            this.dimension = dimension;
            this.value = value;
        }

        public int DimensionIndex
        {
            get
            {
                return dimensionIndex;
            }
        }

        public string Dimension
        {
            get
            {
                return dimension;
            }
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override bool Equals(object obj)
        {
            SplitParams other = obj as SplitParams;
            return (other == null)
                ? false
                : (dimensionIndex == other.dimensionIndex
                    && dimension.Equals(other.dimension)
                    && value.Equals(other.value));
        }

        public override int GetHashCode()
        {
            return (dimensionIndex.GetHashCode()
                + dimension.GetHashCode()
                + value.GetHashCode());
        }
    }
}
