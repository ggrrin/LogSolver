using System;
using System.Linq;
using LogSolver.Architecture;

namespace LogSolver.Structures
{
    public class AStarNode<TState> : AStarNode<TState, AStarNode<TState>> where TState : class, IState
    {
        public AStarNode(AStarNode<TState> parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator) : base(parent, action, estimator)
        {
        }
    }

    public class AStarNode<TState,TNode> : Node<TState,TNode>, IGoalEstimateNode<TState,TNode>, IComparable<AStarNode<TState,TNode>> where TState : class, IState where TNode : Node<TState, TNode>
    {
        public virtual int GoalPriceEstimate { get; }

        public AStarNode(TNode parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator) : base(parent, action)
        {
            GoalPriceEstimate = PathPrice + estimator.CalculateEstimate(State);
        }

        public int CompareTo(AStarNode<TState, TNode> other) => GoalPriceEstimate.CompareTo(other.GoalPriceEstimate);

        public override string ToString(int fullPathPrice) => $"{PathPrice}/{fullPathPrice}({GoalPriceEstimate})|| {string.Join(" ", State.Packages.Select(p => $"[{p.Location.Identifier}|{p.Destination.Identifier}]"))} {Action}";

    }
}