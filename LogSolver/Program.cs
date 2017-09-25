using System.CodeDom;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LogSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var initalState = parser.Parse("TODO");

            ISearchAlgorithm alg = new BreathFirstSearch();
            var result = alg.Search(new Node { Action = Action.Init,
                Depth = 0, PathPrice = 0, State = initalState} );

        }


    }

    public class BreathFirstSearch : ISearchAlgorithm
    {
        public Node Search(Node initialNode)
        {
            throw new System.NotImplementedException();
        }
    }
}
