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
        protected DepthFirstSearch dfs;

        public SearchMode Mode { get; }
        public uint ExpandedNodes => overallExpandedNodes + dfs?.ExpandedNodes ?? 0;
        public uint MaxDepth => Math.Max(overallMaxDepth, dfs?.ExpandedNodes ?? 0);
        public int DepthLimit { get; }
        public INodeExpander<State, Node<State>> Expander { get; }

        public IterativeDeepeningSearch(int depthLimit, INodeExpander<State, Node<State>> expander, SearchMode mode)
        {
            Expander = expander;
            Mode = mode;
            DepthLimit = depthLimit;
        }

        public IEnumerable<Node<State>> Search(Node<State> initialNode)
        {
            for (int i = 0; i < DepthLimit; i++)
            {
                dfs = new DepthFirstSearch(i, Expander, Mode);
                foreach (var resultNode in dfs.Search(initialNode))
                {
                    yield return resultNode;
                }
                overallExpandedNodes += dfs.ExpandedNodes;
                overallMaxDepth = Math.Max(overallMaxDepth, dfs.MaxDepth);
            }
            dfs = null;
        }

    }
}