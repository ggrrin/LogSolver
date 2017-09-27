using System.Collections.Generic;

namespace LogSolver.ProblemAbstration
{
    public interface ISearchAlgorithm<TState, TNode> where TState : class,IState where TNode : INode<TState>
    {
        uint ExpandedNodesStat { get; }
        uint MaxDepthStat { get; }
        INodeExpander<TState, TNode> Expander { get; }
        IEnumerable<TNode> Search(TNode initialNode);
    }
}