using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Searchers
{
    public class BreathFirstSearch : ISearchAlgorithm<State, Node<State>>
    {
        public uint ExpandedNodesStat { get; protected set; }
        public uint MaxDepthStat { get; protected set; }
        public INodeExpander<State, Node<State>> Expander { get; }

        public SearchMode Mode { get; }

        public BreathFirstSearch(INodeExpander<State, Node<State>> expander, SearchMode mode)
        {
            Expander = expander;
            Mode = mode;
        }

        public IEnumerable<Node<State>> Search(Node<State> initialNode)
        {
            var closedNodes = new HashSet<State>();


            var fringesQueue = new Queue<IEnumerable<Node<State>>>();
            fringesQueue.Enqueue(new[] { initialNode });

            while (fringesQueue.Any())
            {
                var currentFringe = fringesQueue.Dequeue();
                foreach (var currentNode in currentFringe)
                {
                    if (Mode == SearchMode.GraphSearch)
                    {
                        if (closedNodes.Contains(currentNode.State))
                            continue;
                        else
                            closedNodes.Add(currentNode.State);
                    }

                    MaxDepthStat = Math.Max(MaxDepthStat, currentNode.Depth);
                    if (currentNode.IsGoalState())
                        yield return currentNode;
                    else
                    {
                        fringesQueue.Enqueue(Expander.ExpandNode(currentNode));
                        ExpandedNodesStat++;
                    }
                }
            }
        }

    }
}