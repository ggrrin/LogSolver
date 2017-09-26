using System.Collections.Generic;

namespace LogSolver
{
    public interface ISearchAlgorithm<TState, TNode> where TState : IState where TNode : INode<TState>
    {
        INodeExpander<TState, TNode> Expander { get; }
        IEnumerable<TNode> Search(TNode initialNode);
    }
}