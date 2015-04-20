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

using OsmSharp.Math.TSP;
using OsmSharp.Math.TSP.EdgeAssemblyGenetic;
using OsmSharp.Math.TSP.Genetic;
using OsmSharp.Math.TSP.Genetic.Solver.Operations.CrossOver;
using OsmSharp.Math.TSP.Genetic.Solver.Operations.Generation;
using OsmSharp.Math.TSP.Genetic.Solver.Operations.Mutation;
using OsmSharp.Math.TSP.LocalSearch.HillClimbing3Opt;
using OsmSharp.TSPLIB.Parser;
using OsmSharp.TSPLIB.Problems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OsmSharp.TSPLIB.Benchmark
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            OsmSharp.Logging.Log.Enable();
            OsmSharp.Logging.Log.RegisterListener(new OsmSharp.WinForms.UI.Logging.ConsoleTraceListener());

            DoTests();

            OsmSharp.Logging.Log.TraceEvent("Program", Logging.TraceEventType.Information, "Finished!");
            Console.ReadLine();
        }

        static void DoTests()
        {
            var ga = false;
            var eax = true;
            var runs = 4;

            var problems = new List<TSPLIBProblem>();

            // real-life test problems.
            problems.Add(Program.CreateProblem(@"\Problems\DM\{0}", "031_K1040-06.atsp", 341));
            problems.Add(Program.CreateProblem(@"\Problems\DM\{0}", "036_K1210-01.atsp", 720));
            problems.Add(Program.CreateProblem(@"\Problems\DM\{0}", "061_K3511.atsp", 281));
            problems.Add(Program.CreateProblem(@"\Problems\DM\{0}", "072_K3510.atsp", 186));
            problems.Add(Program.CreateProblem(@"\Problems\DM\{0}", "098_K3089.atsp", 403));
            problems.Add(Program.CreateProblem(@"\Problems\DM\{0}", "122_K4052.atsp", 463));

            // ATSP instances.
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "br17.atsp", 39));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv33.atsp", 1286));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv35.atsp", 1473));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv38.atsp", 1530));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "p43.atsp", 5620));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv44.atsp", 1613));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv47.atsp", 1776));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ry48p.atsp", 14422));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ft53.atsp", 6905));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv55.atsp", 1608));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv64.atsp", 1839));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ft70.atsp", 38673));
            problems.Add(Program.CreateProblem(@"\Problems\ATSP\{0}", "ftv70.atsp", 1950));

            var solvers = new List<ISolver>();
            //solvers.Add(new HillClimbing3OptSolver(true, true));
            //solvers.Add(new CheapestInsertionSolver());
            //solvers.Add(new ArbitraryInsertionSolver());
            //Program.DoAddSolvers(solvers, ga, eax, 100, 20);
            Program.DoAddSolvers(solvers, ga, eax, 300, 20);

            var tester = new TSPLIBTester("log", problems, solvers, runs);
            tester.StartTests();
        }

        static void DoAddSolvers(List<ISolver> solvers, bool ga, bool eax, int population, int stagnation)
        {
            //int population = 100;
            //int stagnation = 20;

            if (ga)
            {
                //solvers.Add(new GeneticSolver(population, stagnation, 0,
                //        new BestPlacementMutationOperation(), 10,
                //        new EdgeAssemblyCrossover(5,
                //            EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                //            false), 90,
                //        new BestPlacementGenerationOperation()));
                //solvers.Add(new GeneticSolver(population, stagnation, 0,
                //        new BestPlacementMutationOperation(), 10,
                //        new EdgeAssemblyCrossover(30,
                //            EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                //            false), 90,
                //        new BestPlacementGenerationOperation()));
                solvers.Add(new GeneticSolver(population, stagnation, 0,
                          new BestPlacementMutationOperation(), 10,
                          new EdgeAssemblyCrossover(30,
                              EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                              true), 90,
                          new BestPlacementGenerationOperation()));
                //solvers.Add(new GeneticSolver(population, stagnation, 0,
                //        new BestPlacementMutationOperation(), 10,
                //        new EdgeAssemblyCrossover(30,
                //            EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.MultipleRandom,
                //            false), 90,
                //        new BestPlacementGenerationOperation()));
                //solvers.Add(new GeneticSolver(population, stagnation, 0,
                //        new BestPlacementMutationOperation(), 10,
                //        new EdgeAssemblyCrossover(30,
                //            EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.MultipleRandom,
                //            false), 90,
                //        new BestPlacementGenerationOperation()));
            }
            if (eax)
            {
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //    new BestPlacementGenerationOperation(),
                //     new EdgeAssemblyCrossover(1,
                //            EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.MultipleRandom,
                //            true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new BestPlacementGenerationOperation(),
                //      new EdgeAssemblyCrossover(5,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.MultipleRandom,
                //             true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new BestPlacementGenerationOperation(),
                //      new EdgeAssemblyCrossover(10,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.MultipleRandom,
                //             true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new BestPlacementGenerationOperation(),
                //      new EdgeAssemblyCrossover(30,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.MultipleRandom,
                //             true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new _3OptGenerationOperation(),
                //      new EdgeAssemblyCrossover(30,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.MultipleRandom,
                //             true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //    new BestPlacementGenerationOperation(),
                //     new EdgeAssemblyCrossover(1,
                //            EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                //            true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new BestPlacementGenerationOperation(),
                //      new EdgeAssemblyCrossover(5,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                //             true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new BestPlacementGenerationOperation(),
                //      new EdgeAssemblyCrossover(30,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                //             true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new _3OptGenerationOperation(),
                //      new EdgeAssemblyCrossover(30,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                //             false)));
                solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                     new _3OptGenerationOperation(),
                      new EdgeAssemblyCrossover(30,
                             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                             true)));
                //solvers.Add(new EdgeAssemblyCrossOverSolver(population, stagnation,
                //     new _3OptGenerationOperation(),
                //      new EdgeAssemblyCrossover(30,
                //             EdgeAssemblyCrossover.EdgeAssemblyCrossoverSelectionStrategyEnum.SingleRandom,
                //             true)));
            }
        }

        static TSPLIBProblem CreateProblem(string path, string file, float best)
        {
            TSPLIBProblem problem = TSPLIBProblemParser.ParseFromFile(new FileInfo(new FileInfo(
                    Assembly.GetExecutingAssembly().Location).DirectoryName + string.Format(path, file)));
            problem.Best = best;
            return problem;
        }
    }
}