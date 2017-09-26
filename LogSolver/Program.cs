using LogSolver.Actions;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.Expanders;
using LogSolver.Searchers;

namespace LogSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodeFactory = new DefaultNodeFactory();
            var expander = new DummyNodeExpander(nodeFactory);
            var alg = new BreathFirstSearch(expander);
            var parser = new Parser();

            var results = alg.Search(nodeFactory.CreateNode(null, new InitAction(parser)));

        }


    }
}
