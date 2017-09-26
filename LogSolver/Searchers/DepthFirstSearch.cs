using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class DepthFirstSearch : ISearchAlgorithm<State, Node<State>>
    {
        public uint ExpandedNodes { get; protected set; }
        public uint MaxDepth { get; protected set; }
        public int DepthLimit { get; }
        public INodeExpander<State, Node<State>> Expander { get; }
        public SearchMode Mode { get;}

        public DepthFirstSearch(int depthLimit, INodeExpander<State, Node<State>> expander, SearchMode mode)
        {
            Expander = expander;
            Mode = mode;
            DepthLimit = depthLimit;
        }

        public IEnumerable<Node<State>> Search(Node<State> initialNode)
        {
            var closedNodes = new HashSet<State>();

            var fringeStack = new Stack<Node<State>>();
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

                MaxDepth = Math.Max(MaxDepth, currentNode.Depth);

                if (currentNode.IsGoalState())
                    yield return currentNode;
                else
                {
                    if (currentNode.Depth < DepthLimit)
                    {
                        ExpandedNodes++;
                        foreach (var successorNode in Expander.ExpandNode(currentNode))
                            fringeStack.Push(successorNode);
                    }
                }
            }
        }

    }
}