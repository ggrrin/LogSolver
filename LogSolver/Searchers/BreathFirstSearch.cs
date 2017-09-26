using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class BreathFirstSearch : ISearchAlgorithm<State, Node<State>>
    {
        public uint ExpandedNodes { get; protected set; }
        public uint MaxDepth { get; protected set; }
        public INodeExpander<State, Node<State>> Expander { get; }

        public BreathFirstSearch(INodeExpander<State, Node<State>> expander)
        {
            Expander = expander;
        }

        public IEnumerable<Node<State>> Search(Node<State> initialNode)
        {
            var fringesQueue = new Queue<IEnumerable<Node<State>>>();
            fringesQueue.Enqueue(new[] { initialNode });

            while (fringesQueue.Any())
            {
                var currentFringe = fringesQueue.Dequeue();
                foreach (var currentNode in currentFringe)
                {
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