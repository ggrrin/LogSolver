using LogSolver.DefaultStructuresImplementation;

namespace LogSolver.ProblemAbstration
{
    public interface INodeFactory<TState, in TNode> where TState : IState where TNode : INode<TState>
    {
        Node<TState> CreateNode(TNode parentNode, IAction<TState> action);
    }
}