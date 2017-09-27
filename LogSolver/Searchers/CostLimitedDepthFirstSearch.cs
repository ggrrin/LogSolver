using System;
using LogSolver.Architecture;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Searchers
{
    public class CostLimitedDepthFirstSearch<TState, TNode> : DepthFirstSearch<TState, TNode>
        where TState : class, IState
        where TNode : AStarNode<TState>
    {
        public int MaxCost { get; protected set; }
        public int MinCostOvercameLimit { get; protected set; }

        public CostLimitedDepthFirstSearch(INodeExpander<TState, TNode> expander, SearchMode mode, int costLimit)
            : base(expander, mode, costLimit)
        {
        }

        protected override void SearchInit()
        {
            base.SearchInit();
            MaxCost = 0;
            MinCostOvercameLimit = -1;
        }

        protected override void UpdateMaxDepth(TNode currentNode)
        {
            base.UpdateMaxDepth(currentNode);
            MaxCost = Math.Max(MaxCost, currentNode.GoalPriceEstimate);

            if (!ExpandNodeDepthTest(currentNode))
            {
                if (MinCostOvercameLimit == -1)
                    MinCostOvercameLimit = currentNode.GoalPriceEstimate;
                else
                    MinCostOvercameLimit = Math.Min(MinCostOvercameLimit, currentNode.GoalPriceEstimate);
            }
        }

        protected override bool ExpandNodeDepthTest(TNode currentNode)
        {
            bool res = currentNode.GoalPriceEstimate <= DepthLimit;


            return res;
        }
    }
}