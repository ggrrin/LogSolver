using System;
using LogSolver.ProblemAbstration;

namespace LogSolver.DefaultStructuresImplementation
{
    public class AStarNode<TState> : Node<TState>, IComparable<AStarNode<TState>> where TState : class, IState
    {
        public int GoalPriceEstimate { get; }

        public AStarNode(Node<TState> parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator) : base(parent, action)
        {
            GoalPriceEstimate = PathPrice + estimator.CalculateEstimate(State);
        }

        public int CompareTo(AStarNode<TState> other) => GoalPriceEstimate.CompareTo(other.GoalPriceEstimate);
    }
}