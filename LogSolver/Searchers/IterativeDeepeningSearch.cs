using System;
using System.Collections.Generic;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class IterativeDeepeningSearch : ISearchAlgorithm<State, Node<State>>
    {
        protected uint overallExpandedNodes;
        protected uint overallMaxDepth;
        protected DepthFirstSearch<State, Node<State>> dfs;

        public SearchMode Mode { get; }
        public uint ExpandedNodesStat => overallExpandedNodes + dfs?.ExpandedNodesStat ?? 0;
        public uint MaxDepthStat => Math.Max(overallMaxDepth, dfs?.ExpandedNodesStat ?? 0);
        public int DepthLimit { get; }
        public INodeExpander<State, Node<State>> Expander { get; }

        public IterativeDeepeningSearch( INodeExpander<State, Node<State>> expander, SearchMode mode, int depthLimit = Int32.MaxValue - 1)
        {
            Expander = expander;
            Mode = mode;
            DepthLimit = depthLimit;
        }

        public IEnumerable<Node<State>> Search(Node<State> initialNode)
        {
            for (int i = 0; i <= DepthLimit; i++)
            {
                dfs = new DepthFirstSearch<State, Node<State>>(Expander, Mode, i);
                foreach (var resultNode in dfs.Search(initialNode))
                {
                    yield return resultNode;
                }
                overallExpandedNodes += dfs.ExpandedNodesStat;
                overallMaxDepth = Math.Max(overallMaxDepth, dfs.MaxDepthStat);
            }
            dfs = null;
        }
    }
}