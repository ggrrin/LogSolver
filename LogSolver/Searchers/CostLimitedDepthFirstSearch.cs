using System;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class CostLimitedDepthFirstSearch<TState, TNode> : DepthFirstSearch<TState, TNode>
        where TState : class, IState
        where TNode : AStarNode<TState>
    {
        public int MaxCost { get; protected set; }

        public CostLimitedDepthFirstSearch(INodeExpander<TState, TNode> expander, SearchMode mode, int costLimit) 
            : base(expander, mode, costLimit)
        {
        }

        protected override void SearchInit()
        {
            base.SearchInit();
            MaxCost = 0;
        }

        protected override void UpdateMaxDepth(TNode currentNode)
        {
            base.UpdateMaxDepth(currentNode);
            MaxCost = Math.Max(MaxCost, currentNode.GoalPriceEstimate);
        }

        protected override bool ExpandNodeDepthTest(TNode currentNode)
        {
            return currentNode.GoalPriceEstimate <= DepthLimit;
        }
    }
}