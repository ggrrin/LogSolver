using System;
using System.Collections.Generic;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class AStarIterativeDeepeningSearch : ISearchAlgorithm<State, AStarNode<State>>
    {
        protected uint overallExpandedNodes;
        protected uint overallMaxDepth;
        protected int overallMaxCost;
        protected CostLimitedDepthFirstSearch<State, AStarNode<State>> cldfs;

        public SearchMode Mode { get; }
        public uint ExpandedNodesStat => overallExpandedNodes + cldfs?.ExpandedNodesStat ?? 0;
        public uint MaxDepthStat => Math.Max(overallMaxDepth, cldfs?.MaxDepthStat ?? 0);
        public int MaxCostLimitStat => Math.Max(overallMaxCost, cldfs?.MaxCost ?? 0);
        public int CostLimit { get; }
        public INodeExpander<State, AStarNode<State>> Expander { get; }

        public AStarIterativeDeepeningSearch(INodeExpander<State, AStarNode<State>> expander, SearchMode mode, int costLimit = Int32.MaxValue - 1)
        {
            Expander = expander;
            Mode = mode;
            CostLimit = costLimit;
        }

        public IEnumerable<AStarNode<State>> Search(AStarNode<State> initialNode)
        {
            var currentCostLimit = initialNode.GoalPriceEstimate;
            while(currentCostLimit >= 0 && currentCostLimit <= CostLimit )
            {
                cldfs = new CostLimitedDepthFirstSearch<State, AStarNode<State>>(Expander, Mode, currentCostLimit);
                foreach (var resultNode in cldfs.Search(initialNode))
                {
                    yield return resultNode;
                }
                currentCostLimit = cldfs.MaxCost;

                overallExpandedNodes += cldfs.ExpandedNodesStat;
                overallMaxDepth = Math.Max(overallMaxDepth, cldfs.MaxDepthStat);
                overallMaxCost = Math.Max(overallMaxCost, cldfs.MaxCost);
            }
            cldfs = null;
        }
    }
}