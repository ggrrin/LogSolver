using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LogSolver.Actions;
using LogSolver.Estimators;
using LogSolver.Expanders;
using LogSolver.Helpers;
using LogSolver.IO;
using LogSolver.NodeFactories;
using LogSolver.Searchers;
using LogSolver.Structures;

namespace LogSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            /////////////////
            //simple inputs//
            /////////////////

            //Uninformed tree search
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), BFS);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), DFS);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), IDS);

            //Uninformed graph search
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), BFS, SearchMode.GraphSearch);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), DFS, SearchMode.GraphSearch);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), IDS, SearchMode.GraphSearch);

            //Informed astar tree search
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), ABFS);
            //TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), AIDS);
            TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), RBFS);//winner


            //////////
            //inputs//
            //////////

            //TestInputs(new FileInfo(@"..\..\..\..\inputs\inputs.txt"), BFS);
        }

        static void IDS(string input, SearchMode mode)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander<Node<State>>(nodeFactory);
            var searcher = new IterativeDeepeningSearch(expander, mode);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepthStat}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodesStat}");
        }

        static void DFS(string input, SearchMode mode)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander<Node<State>>(nodeFactory);
            var searcher = new DepthFirstSearch<State, Node<State>>(expander, mode, 10000);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepthStat}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodesStat}");
        }

        static void BFS(string input, SearchMode mode)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander<Node<State>>(nodeFactory);
            var searcher = new BreathFirstSearch(expander, mode);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
            Console.WriteLine($"MaxDepth: {searcher.MaxDepthStat}");
            Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodesStat}");
        }

        static void ABFS(string input, SearchMode mode)
        {

            var nodeFactory = new AStarNodeFactory(new PathRemaninerPriceEstimator());//new SimpleRemainerPriceEstimator());
            var expander = new DummyNodeExpander<AStarNode<State>>(nodeFactory);
            var searcher = new AStarBreathFirstSearch(expander, mode);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            foreach (var res in results)
            {
                Console.WriteLine($"==Solution==");
                Console.WriteLine($"Result:\r\n{res.Dump()}");

                Console.WriteLine($"==Solution stats==");
                Console.WriteLine($"Depth: {res.Depth}");
                Console.WriteLine($"Price: {res.PathPrice}");
                Console.WriteLine($"PriceEstimate: {res.GoalPriceEstimate}");

                Console.WriteLine($"==Searcher stats==");
                Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodesStat}");
                Console.WriteLine($"MaxDepth: {searcher.MaxDepthStat}");
                break;
                if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                {
                    Console.WriteLine($"Aborting search...");
                    break;
                }
                Console.WriteLine($"Searching next solution: =>>>>>>>>>>>>>>>>>>>>>>");
            }
        }

        static void AIDS(string input, SearchMode mode)
        {
            var nodeFactory = new AStarNodeFactory(new PathRemaninerPriceEstimator());//new SimpleRemainerPriceEstimator());
            var expander = new SortedDummyNodeExpander<AStarNode<State>>(nodeFactory, descendingOrder: false);
            var searcher = new AStarIterativeDeepeningSearch(expander, mode);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            //var res = results.First();
            foreach (var res in results)
            {

                Console.WriteLine($"==Solution==");
                Console.WriteLine($"Result:\r\n{res.Dump()}");

                Console.WriteLine($"==Solution stats==");
                Console.WriteLine($"Depth: {res.Depth}");
                Console.WriteLine($"Price: {res.PathPrice}");
                Console.WriteLine($"PriceEstimate: {res.GoalPriceEstimate}");

                Console.WriteLine($"==Searcher stats==");
                Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodesStat}");
                Console.WriteLine($"MaxDepth: {searcher.MaxDepthStat}");
                Console.WriteLine($"MaxCostLimit: {searcher.MaxCostLimitStat}");
                Console.WriteLine($"LastCostOvercameLimit: {searcher.CostOvercameLimitState}");
                break;
                if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                {
                    Console.WriteLine($"Aborting search...");
                    break;
                }
                Console.WriteLine($"Searching next solution: =>>>>>>>>>>>>>>>>>>>>>>");
            }
        }

        static void RBFS(string input, SearchMode mode)
        {   
            //var nodeFactory = new RBFSNodeFactory(new YPriceEstimator()); 
            //var nodeFactory = new RBFSNodeFactory(new SimpleRemainerPriceEstimator());
            //var nodeFactory = new RBFSNodeFactory(new PathRemaninerPriceEstimator());
            var nodeFactory = new RBFSNodeFactory(new CombinePriceEstimator(new YPriceEstimator(), new PathRemaninerPriceEstimator()));
            var expander = new DummyNodeExpander<RBFSNode<State>>(nodeFactory);
            var searcher = new RecursiveBestFirstSearch<RBFSNode<State>>(expander);
            var parser = new Parser(input);

            var results = searcher.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            foreach (var res in results)
            {

                Console.WriteLine($"==Solution==");
                Console.WriteLine($"Result:\r\n{res.Dump()}");

                Console.WriteLine($"==Solution stats==");
                Console.WriteLine($"Depth: {res.Depth}");
                Console.WriteLine($"Price: {res.PathPrice}");
                Console.WriteLine($"PriceEstimate: {res.GoalPriceEstimate}");

                Console.WriteLine($"==Searcher stats==");
                Console.WriteLine($"ExpandedNodes: {searcher.ExpandedNodesStat}");
                Console.WriteLine($"MaxDepth: {searcher.MaxDepthStat}");
                break;
                if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                {
                    Console.WriteLine($"Aborting search...");
                    break;
                }
                Console.WriteLine($"Searching next solution: =>>>>>>>>>>>>>>>>>>>>>>");
            }
        }


        static void TestInputs(FileInfo inputsFile, TestFunc test, SearchMode mode = SearchMode.TreeSearch)
        {
            var inputs = File.ReadAllLines(inputsFile.FullName);
            foreach (var inputFileName in inputs.Where(s => !s.StartsWith("#")))
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine($"Running: {inputFileName}");

                var watch = Stopwatch.StartNew();
                test(Path.Combine(inputsFile.Directory.FullName, inputFileName), mode);
                watch.Stop();

                Console.WriteLine($"Time: {watch.Elapsed}");
            }
        }

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

        static void HeapTest()
        {
            Random r = new Random();
            var h = new Heap<int>();
            const int num = 1000;
            for (int i = 0; i < num; i++)
            {
                var x = r.Next(10000);
                h.Add(x);
                Console.Write($"{x} ");
            }

            Console.WriteLine();


            var prev = -1;
            for (int i = 0; i < num; i++)
            {
                var x = h.ExtractMin();
                Console.Write($"{x} ");

                if (x < prev)
                {
                    Console.Write($"Error ");
                    break;

                }
                prev = x;
            }
            Console.WriteLine();
        }
    }
}
