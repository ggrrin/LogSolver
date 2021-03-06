using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.NodeFactories;
using LogSolver.Structures;

namespace LogSolver.Expanders
{
    public class SortedDummyNodeExpander<TNode> : DummyNodeExpander<TNode> where TNode : IGoalEstimateNode<State,TNode>
    {
        public bool DescendingOrder { get; }
        public SortedDummyNodeExpander(INodeFactory<State, TNode> nodeFactory, bool descendingOrder) : base(nodeFactory)
        {
            DescendingOrder = descendingOrder;
        }

        public override IEnumerable<TNode> ExpandNode(TNode nodeToExpand)
        {
            if(DescendingOrder)
                return base.ExpandNode(nodeToExpand).OrderByDescending(n => n.GoalPriceEstimate);
            else
                return base.ExpandNode(nodeToExpand).OrderBy(n => n.GoalPriceEstimate);
        }
    }
}