using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.Expanders;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Searchers
{
    public class RecursiveBestFirstSearch<TNode> : ISearchAlgorithm<State, TNode> where TNode : RBFSNode<State>
    {
        public uint ExpandedNodesStat { get; protected set; }
        public uint MaxDepthStat { get; protected set; }

        public INodeExpander<State, TNode> Expander { get; }

        public RecursiveBestFirstSearch(INodeExpander<State, TNode> expander)
        {
            Expander = expander;
        }

        public IEnumerable<TNode> Search(TNode initialNode)
        {
            var (result, _) = RBFS(initialNode, Int32.MaxValue);
            if (result != null)
                yield return result;
            else
                yield break;
        }

        (TNode, int?) RBFS(TNode currentNode, int f_limit)
        {
            if (currentNode.IsGoalState())
            {
                return (currentNode, null);
            }
            else
            {
                //TODO test heap construction
                var successors = new Heap<TNode>(Expander.ExpandNode(currentNode));
                if (!successors.Any())
                {
                    return (null, int.MaxValue);
                }
                else
                {
                    while (true)
                    {
                        var bestSuccessor = successors.ExtractMin();
                        if (bestSuccessor.GoalPriceEstimate > f_limit)
                            return (null, bestSuccessor.GoalPriceEstimate);

                        var alternative = successors.PeekMin().GoalPriceEstimate;

                        var (result, best) = RBFS(bestSuccessor, Math.Min(f_limit, alternative));

                        if (result != null)
                            return (result, null);
                        else
                            bestSuccessor.GoalPriceEstimate = best.Value;
                    }
                }
            }
        }
    }
}