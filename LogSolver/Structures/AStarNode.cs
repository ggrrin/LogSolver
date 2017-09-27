using System;
using System.Linq;
using LogSolver.Architecture;

namespace LogSolver.Structures
{
    public class AStarNode<TState> : Node<TState>, IGoalEstimateNode<TState>, IComparable<AStarNode<TState>> where TState : class, IState
    {
        public virtual int GoalPriceEstimate { get; }

        public AStarNode(Node<TState> parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator) : base(parent, action)
        {
            GoalPriceEstimate = PathPrice + estimator.CalculateEstimate(State);
        }

        public int CompareTo(AStarNode<TState> other) => GoalPriceEstimate.CompareTo(other.GoalPriceEstimate);

        public override string ToString(int fullPathPrice) => $"{PathPrice}/{fullPathPrice}({GoalPriceEstimate})|| {string.Join(" ", State.Packages.Select(p => $"[{p.Location.Identifier}|{p.Destination.Identifier}]"))} {Action}";
    }
}