using System.Collections.Generic;

namespace LogSolver.ProblemAbstration
{
    public interface INodeExpander<TState, TNode> where TState : IState where TNode : INode<TState>
    {
        INodeFactory<TState, TNode> NodeFactory { get; }
        IEnumerable<TNode> ExpandNode(TNode nodeToExpand);
    }
}