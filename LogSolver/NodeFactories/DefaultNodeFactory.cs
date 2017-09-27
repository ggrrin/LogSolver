using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.NodeFactories
{
    public class DefaultNodeFactory : INodeFactory<State, Node<State>>
    {
        public Node<State> CreateNode(Node<State> parentNode, IAction<State> action)
        {
            return new Node<State>(parentNode, action);
        }
    }
}