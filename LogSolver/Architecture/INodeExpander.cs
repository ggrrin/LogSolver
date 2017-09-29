using System.Collections.Generic;

namespace LogSolver.Architecture
{
    public interface INodeExpander<TState, TNode> where TState : class,IState where TNode : INode<TState,TNode>
    {
        INodeFactory<TState, TNode> NodeFactory { get; }
        IEnumerable<TNode> ExpandNode(TNode nodeToExpand);
    }
}