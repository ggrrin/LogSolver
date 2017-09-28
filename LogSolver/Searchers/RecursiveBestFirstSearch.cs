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
            var stack = new Stack<StackFrame<TNode>>();
            stack.Push(new StackFrame<TNode>
            {
                Node = initialNode,
                F_limit = int.MaxValue
            });

            while (stack.Any())
            {
                var currentLayer = stack.Peek();

                if (currentLayer.Node.IsGoalState())
                {
                    yield return currentLayer.Node;
                    yield break;
                }
                else
                {
                    //expand only if atanding for the first time
                    currentLayer.Successors = currentLayer.Successors ?? new Heap<TNode>(Expander.ExpandNode(currentLayer.Node));

                    if (!currentLayer.Successors.Any())
                    {
                        //Layer is empty mark parent node with worst value and throw out frame
                        PopCurrentLayerAndUpdateParentNodeEstimateAndPriorityQueue(int.MaxValue);
                    }
                    else
                    {
                        var bestSuccessor = currentLayer.Successors.PeekMin();
                        if (bestSuccessor.GoalPriceEstimate > currentLayer.F_limit)
                        {
                            //Mark layer with best estimate from his successors
                            PopCurrentLayerAndUpdateParentNodeEstimateAndPriorityQueue(bestSuccessor.GoalPriceEstimate);
                        }
                        else
                        {
                            var alternative = currentLayer.Successors.Any() ? currentLayer.Successors.PeekMin().GoalPriceEstimate : currentLayer.F_limit;

                            stack.Push(new StackFrame<TNode>
                            {
                                Node = bestSuccessor,
                                F_limit = Math.Min(currentLayer.F_limit, alternative),
                            });
                        }
                    }

                    void PopCurrentLayerAndUpdateParentNodeEstimateAndPriorityQueue(int value)
                    {
                        currentLayer.Node.GoalPriceEstimate = value;
                        stack.Pop();
                        if (stack.Any())
                        {
                            var parentLayer = stack.Peek();
                            parentLayer.Successors.UpdateKey(currentLayer.Node);
                        }
                    }
                }
            }
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

                        var alternative = successors.Any() ? successors.PeekMin().GoalPriceEstimate : f_limit;

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