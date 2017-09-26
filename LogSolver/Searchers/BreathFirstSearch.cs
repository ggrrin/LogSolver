using System.Collections.Generic;
using System.Linq;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class BreathFirstSearch : ISearchAlgorithm<State, Node<State>>
    {
        public INodeExpander<State, Node<State>> Expander { get; }

        public BreathFirstSearch(INodeExpander<State, Node<State>> expander)
        {
            Expander = expander;
        }

        public IEnumerable<Node<State>> Search(Node<State> initialNode)
        {
            if (initialNode.IsGoalState())
            {
                yield return initialNode;
                yield break;
            }

            var fringesQueue = new Queue<IEnumerable<Node<State>>>();
            fringesQueue.Enqueue(Expander.ExpandNode(initialNode));

            while (fringesQueue.Any())
            {
                var currentFringe = fringesQueue.Dequeue();
                foreach (var currentNode in currentFringe)
                {
                    if (currentNode.IsGoalState())
                        yield return currentNode;
                    else
                        fringesQueue.Enqueue(Expander.ExpandNode(currentNode));
                }
            }
        }

    }
}