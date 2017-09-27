using System;
using System.Linq;
using LogSolver.Architecture;

namespace LogSolver.Structures
{
    public class AStarNode<TState> : Node<TState>, IComparable<AStarNode<TState>>, IEquatable<AStarNode<TState>> 
        where TState : class, IState
    {
        public int GoalPriceEstimate { get; }

        public AStarNode(Node<TState> parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator) : base(parent, action)
        {
            GoalPriceEstimate = PathPrice + estimator.CalculateEstimate(State);
        }

        public int CompareTo(AStarNode<TState> other) => GoalPriceEstimate.CompareTo(other.GoalPriceEstimate);

        public static bool operator <(AStarNode<TState> lh, AStarNode<TState> rh) => lh.CompareTo(rh) < 0;

        public static bool operator >(AStarNode<TState> lh, AStarNode<TState> rh) => lh.CompareTo(rh) > 0;

        public static bool operator <=(AStarNode<TState> lh, AStarNode<TState> rh) => lh.CompareTo(rh) <= 0;

        public static bool operator >=(AStarNode<TState> lh, AStarNode<TState> rh) =>  lh.CompareTo(rh) >= 0;

        public static bool operator ==(AStarNode<TState> lh, AStarNode<TState> rh) => lh != null && lh.Equals(rh);

        public static bool operator !=(AStarNode<TState> lh, AStarNode<TState> rh) => !(lh == rh);

        public bool Equals(AStarNode<TState> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GoalPriceEstimate == other.GoalPriceEstimate;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AStarNode<TState>) obj);
        }

        public override int GetHashCode()
        {
            return GoalPriceEstimate;
        }
        public override string ToString(int fullPathPrice) => $"{PathPrice}/{fullPathPrice}({GoalPriceEstimate})|| {string.Join(" ", State.Packages.Select(p => $"[{p.Location.Identifier}|{p.Destination.Identifier}]"))} {Action}";
    }
}