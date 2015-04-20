// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

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

using OsmSharp.TSPLIB.Problems;
using OsmSharp.Math.TSP;

namespace OsmSharp.TSPLIB.Convertor.ATSP_TSP
{
    /// <summary>
    /// A A-TSP to S-TSP convertor.
    /// </summary>
    public static class ATSP_TSPConvertor
    {
        /// <summary>
        /// Converts the given A-TSP to a symetric equivalent S-TSP.
        /// </summary>
        /// <param name="atsp"></param>
        /// <returns></returns>
        public static TSPLIBProblem Convert(TSPLIBProblem atsp)
        {
            // check if the problem is not already symmetric.
            if (atsp.Symmetric)
            {
                return atsp;
            }
            
            // the problem is not symmetric, convert it.
            var name = atsp.Name + "(SYM)";
            var comment = atsp.Comment;

            // convert the problem to a symetric one.
            var symetric = atsp.ConvertToSymmetric();

            return new TSPLIBProblem(name, comment, symetric.Size, symetric.WeightMatrix,
                TSPLIBProblemWeightTypeEnum.Explicit, TSPLIBProblemTypeEnum.TSP);
        }
    }
}