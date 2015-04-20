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

using OsmSharp.Math.TSP.Problems;
using OsmSharp.Math.VRP.Core;
using System.Collections.Generic;

namespace OsmSharp.TSPLIB.Problems
{
    /// <summary>
    /// Represents and parses a TSPLIB problem.
    /// </summary>
    public class TSPLIBProblem : IProblem
    {
        private int _size;

        private double[][] _weights;

        public TSPLIBProblem(string name, string comment, int size, double[][] weights,
            TSPLIBProblemWeightTypeEnum weight_type,
            TSPLIBProblemTypeEnum problem_type)
        {
            this.Name = name;
            this.Type = problem_type;
            this.WeightType = weight_type;
            _weights = weights;
            _size = size;
        }

        public TSPLIBProblemTypeEnum Type { get; private set; }

        public TSPLIBProblemWeightTypeEnum WeightType { get; private set; }

        public double[][] WeightMatrix
        {
            get 
            {
                return _weights; 
            }
        }

        public int Size
        {
            get 
            {
                return _size;
            }
        }

        public double Weight(int from, int to)
        {
            return _weights[from][to];
        }


        public int? First
        {
            get 
            { 
                return 0; 
            }
        }

        public int? Last
        {
            get 
            { 
                return 0; 
            }
        }

        public bool Symmetric
        {
            get 
            {
                return this.Type == TSPLIBProblemTypeEnum.TSP; 
            }
        }

        public bool Euclidean
        {
            get 
            { 
                return true; 
            }
        }

        public float Best { get; set; }

        public string Name { get; private set; }

        public string Comment { get; private set; }


        #region Nearest Neighbour

        /// <summary>
        /// Keeps the nearest neighbour list.
        /// </summary>
        private NearestNeighbours10[] _neighbours;

        /// <summary>
        /// Generate the nearest neighbour list.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public NearestNeighbours10 Get10NearestNeighbours(int v)
        {
            if (_neighbours == null)
            {
                _neighbours = new NearestNeighbours10[this.Size];
            }
            NearestNeighbours10 result = _neighbours[v];
            if (result == null)
            {
                var neighbours = new SortedDictionary<double, List<int>>();
                for (int customer = 0; customer < this.Size; customer++)
                {
                    if (customer != v)
                    {
                        double weight = this.WeightMatrix[v][customer];
                        List<int> customers = null;
                        if (!neighbours.TryGetValue(weight, out customers))
                        {
                            customers = new List<int>();
                            neighbours.Add(weight, customers);
                        }
                        customers.Add(customer);
                    }
                }

                result = new NearestNeighbours10();
                foreach (KeyValuePair<double, List<int>> pair in neighbours)
                {
                    foreach (int customer in pair.Value)
                    {
                        if (result.Count < 10)
                        {
                            if (result.Max < pair.Key)
                            {
                                result.Max = pair.Key;
                            }
                            result.Add(customer);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                _neighbours[v] = result;
            }
            return result;
        }

        #endregion
    }
}