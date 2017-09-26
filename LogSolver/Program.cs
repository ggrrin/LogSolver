using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LogSolver.Actions;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.Expanders;
using LogSolver.ProblemAbstration;
using LogSolver.Searchers;

namespace LogSolver
{

    public delegate void Test(string inputFilePath);

    class Program
    {
        static void Main(string[] args)
        {
            //simple inputs
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), BFS);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), DFS);
            TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), IDS);


            //TestInputs(new FileInfo(@"..\..\..\..\inputs\inputs.txt"), BFS);
        }

        static void IDS(string input)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var searcher = new IterativeDeepeningSearch(int.MaxValue, expander);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepth}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodes}");
        }

        static void DFS(string input)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var searcher = new DepthFirstSearch(10000, expander);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepth}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodes}");
        }

        static void BFS(string input)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var searcher = new BreathFirstSearch(expander);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepth}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodes}");
        }

        static void TestInputs(FileInfo inputsFile, Test test)
        {
            var inputs = File.ReadAllLines(inputsFile.FullName);
            foreach (var inputFileName in inputs)
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine($"Running: {inputFileName}");

                var watch = Stopwatch.StartNew();
                test(Path.Combine(inputsFile.Directory.FullName, inputFileName));
                watch.Stop();

                Console.WriteLine($"Time: {watch.Elapsed}");
            }
        }
    }
}
