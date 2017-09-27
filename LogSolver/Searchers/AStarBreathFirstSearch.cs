using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class AStarBreathFirstSearch : ISearchAlgorithm<State, AStarNode<State>>
    {
        public uint ExpandedNodesStat { get; protected set; }
        public uint MaxDepthStat { get; protected set; }
        public INodeExpander<State, AStarNode<State>> Expander { get; }

        public SearchMode Mode { get; }

        public AStarBreathFirstSearch(INodeExpander<State, AStarNode<State>> expander, SearchMode mode)
        {
            Expander = expander;
            Mode = mode;
        }

        public IEnumerable<AStarNode<State>> Search(AStarNode<State> initialNode)
        {
            var closedNodes = new HashSet<State>();

            var fringesPriorityQueue = new Heap<AStarNode<State>>(new[] { initialNode });

            while (fringesPriorityQueue.Any())
            {
                var currentNode = fringesPriorityQueue.ExtractMin();
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
                    foreach (var node in Expander.ExpandNode(currentNode))
                    {
                        fringesPriorityQueue.Add(node);
                        ExpandedNodesStat++;
                    }
                }
            }
        }

    }
}