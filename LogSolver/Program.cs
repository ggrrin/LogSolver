using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LogSolver.Actions;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.Expanders;
using LogSolver.Searchers;

namespace LogSolver
{

    public delegate void Test(string inputFilePath);

    class Program
    {
        static void Main(string[] args)
        {
            TestInputs(new FileInfo(@"..\..\..\..\simple-inputs\inputs.txt"), BFS);
            //TestInputs(new FileInfo(@"..\..\..\..\inputs\inputs.txt"), BFS);
        }

        static void BFS(string input)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var alg = new BreathFirstSearch(expander);
            var parser = new Parser(input);

            var results = alg.Search(nodeFactory.CreateNode(null, new InitAction(parser)));
            var res = results.First();
            Console.WriteLine($"Result:\r\n{res.Dump()}");
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
