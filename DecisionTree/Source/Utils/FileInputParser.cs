using DecisionTree.Source.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Source.Utils
{
    public class FileInputParser
    {
        /// <summary>
        /// Read all lines from a file into a string array
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>A string array of all the lines in the file.</returns>
        private static string[] readFromFile(string path)
        {
            return System.IO.File.ReadAllLines(path);
        }

        /// <summary>
        /// Reads a file into memory and parses it into a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="delimiter">The delimiter used in the file.</param>
        /// <returns>The <see cref="DataTable"/> parsed from the file.</returns>
        public static DataTable ReadDataTableFromFile(string path, char delimiter)
        {
            string[] fileContents = readFromFile(path);
            DataTable dataTable = new DataTable(fileContents[0].Split(delimiter),
                fileContents.Skip(1).Select(r => r.Split(delimiter)).ToList());
            return dataTable;
        }
    }
}
