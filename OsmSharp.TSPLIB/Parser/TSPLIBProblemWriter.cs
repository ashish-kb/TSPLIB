// The MIT License (MIT)

// Copyright (c) 2015 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.IO;
using OsmSharp.TSPLIB.Problems;

namespace OsmSharp.TSPLIB
{
    /// <summary>
    /// Writes TSP problems in TSP lib format.
    /// </summary>
    public class TSPLIBProblemWriter
    {
        private const string TOKEN_NAME = "NAME:";
        private const string TOKEN_EOF = "EOF";
        private const string TOKEN_TYPE = "TYPE:";
        private const string TOKEN_DIMENSION = "DIMENSION:";
        private const string TOKEN_TYPE_VALUE_TSP = "TSP";
        private const string TOKEN_TYPE_VALUE_ATSP = "ATSP";
        private const string TOKEN_COMMENT = "COMMENT:";
        private const string TOKEN_EDGE_WEIGHT_TYPE = "EDGE_WEIGHT_TYPE:";
        private const string TOKEN_EDGE_WEIGHT_TYPE_VALUE_EUC_2D = "EUC_2D";
        private const string TOKEN_EDGE_WEIGHT_TYPE_EXPLICIT = "EXPLICIT";
        private const string TOKEN_EDGE_WEIGHT_FORMAT = "EDGE_WEIGHT_FORMAT:";
        private const string TOKEN_EDGE_WEIGHT_FORMAT_MATRIX = "MATRIX";
        private const string TOKEN_EDGE_WEIGHT_SECTION = "EDGE_WEIGHT_SECTION";
        private const string TOKEN_NODE_COORD_SECTION = "NODE_COORD_SECTION";

        /// <summary>
        /// Writes the given TSP problem to the given file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="problem"></param>
        public static void Write(FileInfo file, TSPLIBProblem problem)
        {
            StreamWriter writer = new StreamWriter(file.OpenWrite());

            TSPLIBProblemWriter.Write(writer, problem);
        }

        /// <summary>
        /// Writes the given TSP problem to the given file.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="problem"></param>
        public static void Write(StreamWriter writer, TSPLIBProblem problem)
        {
            if (problem.Symmetric)
            {
                TSPLIBProblemWriter.GenerateTSP(writer, problem);
            }
            else
            {
                TSPLIBProblemWriter.GenerateATSP(writer, problem);
            }
        }

        private static void GenerateTSP(StreamWriter writer, TSPLIBProblem problem)
        {
            writer.WriteLine(string.Format("NAME: {0}", problem.Name));
            writer.WriteLine("TYPE: TSP");
            writer.WriteLine(string.Format("COMMENT: {0}", problem.Comment));
            writer.WriteLine(string.Format("DIMENSION: {0}", problem.Size));
            writer.WriteLine("EDGE_WEIGHT_TYPE: EXPLICIT");
            writer.WriteLine("EDGE_WEIGHT_FORMAT: FULL_MATRIX");
            //writer.WriteLine("EDGE_WEIGHT_FORMAT: UPPER_ROW");
            writer.WriteLine("DISPLAY_DATA_TYPE: TWOD_DISPLAY");
            writer.WriteLine("EDGE_WEIGHT_SECTION");

            // get the biggest weight.
            int max = 0;
            int[][] upper_rows = new int[problem.Size][];
            for (int x = 0; x < problem.Size; x++)
            {
                upper_rows[x] = new int[problem.Size];
                for (int y = 0; y < problem.Size; y++)
                {
                    int value = (int)problem.WeightMatrix[x][y];
                    if (value > max)
                    {
                        max = value;
                    }
                    upper_rows[x][y] = value;
                }
            }

            int length = max.ToString().Length;
            for (int x = 0; x < upper_rows.Length; x++)
            {
                for (int y = 0; y < upper_rows[x].Length; y++)
                {
                    if (y > 0)
                    {
                        writer.Write(" ");
                    }
                    writer.Write(upper_rows[x][y].ToString().PadLeft(length));
                }
                writer.WriteLine();
            }
            writer.WriteLine("EOF");
            writer.Flush();
        }

        private static void GenerateATSP(StreamWriter writer, TSPLIBProblem problem)
        {
            writer.WriteLine(string.Format("NAME: {0}", problem.Name));
            writer.WriteLine("TYPE: ATSP");
            writer.WriteLine(string.Format("COMMENT: {0}", problem.Comment));
            writer.WriteLine(string.Format("DIMENSION: {0}", problem.Size));
            writer.WriteLine("EDGE_WEIGHT_TYPE: EXPLICIT");
            writer.WriteLine("EDGE_WEIGHT_FORMAT: FULL_MATRIX");
            //writer.WriteLine("EDGE_WEIGHT_FORMAT: UPPER_ROW");
            writer.WriteLine("DISPLAY_DATA_TYPE: TWOD_DISPLAY");
            writer.WriteLine("EDGE_WEIGHT_SECTION");

            // get the biggest weight.
            int max = 0;
            int[][] upper_rows = new int[problem.Size][];
            for (int x = 0; x < problem.Size; x++)
            {
                upper_rows[x] = new int[problem.Size];
                for (int y = 0; y < problem.Size; y++)
                {
                    int value = (int)problem.WeightMatrix[x][y];
                    if (value > max)
                    {
                        max = value;
                    }
                    upper_rows[x][y] = value;
                }
            }

            int length = max.ToString().Length;
            for (int x = 0; x < upper_rows.Length; x++)
            {
                for (int y = 0; y < upper_rows[x].Length; y++)
                {
                    if (y > 0)
                    {
                        writer.Write(" ");
                    }
                    writer.Write(upper_rows[x][y].ToString().PadLeft(length));
                }
                writer.WriteLine();
            }
            writer.WriteLine("EOF");
            writer.Flush();
        }
    }
}