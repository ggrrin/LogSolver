using System.Collections.Generic;

namespace LogSolver.Architecture
{
    public interface ISearchAlgorithm<TState, TNode> where TState : class,IState where TNode : INode<TState,TNode>
    {
        uint ExpandedNodesStat { get; }
        uint MaxDepthStat { get; }
        INodeExpander<TState, TNode> Expander { get; }
        IEnumerable<TNode> Search(TNode initialNode);
    }
}