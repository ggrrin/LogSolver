﻿using System.Collections.Generic;

namespace LogSolver.ProblemAbstration
{
    public interface ISearchAlgorithm<TState, TNode> where TState : class,IState where TNode : INode<TState>
    {
        INodeExpander<TState, TNode> Expander { get; }
        IEnumerable<TNode> Search(TNode initialNode);
    }
}