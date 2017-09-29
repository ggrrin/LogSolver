using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.Helpers;

namespace LogSolver.Searchers
{
    public class DepthFirstSearch<TState, TNode> : ISearchAlgorithm<TState, TNode> where TState : class, IState where TNode : INode<TState,TNode>
    {
        public uint ExpandedNodesStat { get; protected set; }
        public uint MaxDepthStat { get; protected set; }
        public int DepthLimit { get; }
        public INodeExpander<TState, TNode> Expander { get; }
        public SearchMode Mode { get; }

        public DepthFirstSearch(INodeExpander<TState, TNode> expander, SearchMode mode, int depthLimit = Int32.MaxValue)
        {
            Expander = expander;
            Mode = mode;
            DepthLimit = depthLimit;
        }

        public IEnumerable<TNode> Search(TNode initialNode)
        {
            SearchInit();
            var closedNodes = new HashSet<TState>();
            var fringeStack = new Stack<TNode>();

            fringeStack.Push(initialNode);

            while (fringeStack.Any())
            {
                var currentNode = fringeStack.Pop();

                if (Mode == SearchMode.GraphSearch)
                {
                    if (closedNodes.Contains(currentNode.State))
                        continue;
                    else
                        closedNodes.Add(currentNode.State);
                }

                UpdateMaxDepth(currentNode);

                if (currentNode.IsGoalState())
                    yield return currentNode;
                else
                {
                    if (ExpandNodeDepthTest(currentNode))
                    {
                        ExpandedNodesStat++;
                        foreach (var successorNode in Expander.ExpandNode(currentNode))
                            fringeStack.Push(successorNode);
                    }
                }
            }
        }

        protected virtual void SearchInit()
        {
            MaxDepthStat = 0;
            ExpandedNodesStat = 0;
        }

        protected virtual void UpdateMaxDepth(TNode currentNode)
        {
            MaxDepthStat = Math.Max(MaxDepthStat, currentNode.Depth);
        }

        protected virtual bool ExpandNodeDepthTest(TNode currentNode)
        {
            return currentNode.Depth <= DepthLimit;
        }
    }
}