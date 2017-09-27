using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class AStarBreathFirstSearch : ISearchAlgorithm<State, AStarNode<State>>
    {
        public uint ExpandedNodes { get; protected set; }
        public uint MaxDepth { get; protected set; }
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

                MaxDepth = Math.Max(MaxDepth, currentNode.Depth);
                if (currentNode.IsGoalState())
                    yield return currentNode;
                else
                {
                    foreach (var node in Expander.ExpandNode(currentNode))
                    {
                        fringesPriorityQueue.Add(node);
                        ExpandedNodes++;
                    }
                }
            }
        }

    }


    public class BreathFirstSearch : ISearchAlgorithm<State, Node<State>>
    {
        public uint ExpandedNodes { get; protected set; }
        public uint MaxDepth { get; protected set; }
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

                    MaxDepth = Math.Max(MaxDepth, currentNode.Depth);
                    if (currentNode.IsGoalState())
                        yield return currentNode;
                    else
                    {
                        fringesQueue.Enqueue(Expander.ExpandNode(currentNode));
                        ExpandedNodes++;
                    }
                }
            }
        }

    }
}