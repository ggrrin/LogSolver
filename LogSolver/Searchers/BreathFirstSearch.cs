using System.Collections.Generic;
using LogSolver.DefaultStructuresImplementation;
using LogSolver.ProblemAbstration;

namespace LogSolver.Searchers
{
    public class BreathFirstSearch : ISearchAlgorithm<State,Node<State>>
    {
        public INodeExpander<State, Node<State>> Expander { get; }

        public BreathFirstSearch(INodeExpander<State, Node<State>> expander)
        {
            Expander = expander;
        }


        public IEnumerable<Node<State>> Search(Node<State> initialNode)
        {
            throw new System.NotImplementedException();
        }

    }
}