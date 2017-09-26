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

    public delegate void Test(string inputFilePath, SearchMode mode);

    class Program
    {
        static void HashTest()
        {
            var nodeFactory = new DefaultNodeFactory();
            var parser = new Parser(@"..\..\..\..\simple-inputs\output-1-3-1-1-1.in");

            var ninit = nodeFactory.CreateNode(null, new InitAction(parser));
            var nloaded = nodeFactory.CreateNode(ninit, new LoadAction(ninit.State.Packages.First(), ninit.State.Vans.First()));

            var nmove1 = nodeFactory.CreateNode(nloaded, new DriveAction(nloaded.State.Vans.First(), nloaded.State.Places.ElementAt(0)));

            var nmove2 = nodeFactory.CreateNode(nloaded, new DriveAction(nloaded.State.Vans.First(), nloaded.State.Places.ElementAt(1)));

            var nmove2x = nodeFactory.CreateNode(nmove2,
                new DriveAction(nmove2.State.Vans.First(), nmove2.State.Places.ElementAt(0)));

            var unload1 = nodeFactory.CreateNode(nmove1, new UnLoadAction(nmove1.State.Packages.ElementAt(0)));


            var unload2 = nodeFactory.CreateNode(nmove2x, new UnLoadAction(nmove2x.State.Packages.ElementAt(0)));

            int ninitHash = ninit.State.GetHashCode();
            int nmove1Hash = nmove1.State.GetHashCode();
            int nmove2Hash = nmove2.State.GetHashCode();
            int nmove2xHash = nmove2x.State.GetHashCode();
            int unload1Hash = unload1.State.GetHashCode();
            int unload2Hash = unload2.State.GetHashCode();

            bool r0 = nmove1Hash == nmove2xHash;
            bool r1 = unload1Hash == unload2Hash;
            bool r2 = unload1.State == unload2.State;


        }

        static void Main(string[] args)
        {
            var h = new Heap<int>(new []{5,3,21,1,4,35,432,2,32,12});
            h.Add(31);
            h.Add(310);
            h.Add(131);
            h.Add(1531);
            h.Sort();

            //simple inputs
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), BFS);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), DFS);
            TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), IDS);

            //TOO slow
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), BFS, SearchMode.GraphSearch);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), DFS, SearchMode.GraphSearch);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), IDS, SearchMode.GraphSearch);

            //TestInputs(new FileInfo(@"..\..\..\..\inputs\inputs.txt"), BFS);
        }

        static void IDS(string input, SearchMode mode)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var searcher = new IterativeDeepeningSearch(int.MaxValue, expander, mode);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepth}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodes}");
        }

        static void DFS(string input, SearchMode mode)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var searcher = new DepthFirstSearch(10000, expander, mode);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepth}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodes}");
        }

        static void BFS(string input, SearchMode mode)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var searcher = new BreathFirstSearch(expander, mode);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepth}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodes}");
        }

        static void TestInputs(FileInfo inputsFile, Test test, SearchMode mode = SearchMode.TreeSearch)
        {
            var inputs = File.ReadAllLines(inputsFile.FullName);
            foreach (var inputFileName in inputs)
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine($"Running: {inputFileName}");

                var watch = Stopwatch.StartNew();
                test(Path.Combine(inputsFile.Directory.FullName, inputFileName), mode);
                watch.Stop();

                Console.WriteLine($"Time: {watch.Elapsed}");
            }
        }
    }
}
