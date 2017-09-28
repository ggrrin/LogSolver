using System;
using System.Linq;
using LogSolver.Architecture;

namespace LogSolver.Structures
{
    public class RBFSNode<TState> : Node<TState>, IGoalEstimateNode<TState>, IComparable<RBFSNode<TState>> where TState : class, IState
    {
        public int GoalPriceEstimate { get; set; }

        public int InitialGoalPriceEstimate { get; }

        public RBFSNode(Node<TState> parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator, int parentEstimate) : base(parent, action)
        {
            InitialGoalPriceEstimate = GoalPriceEstimate = Math.Max(PathPrice + estimator.CalculateEstimate(State), parentEstimate);
        }

        public int CompareTo(RBFSNode<TState> other) => GoalPriceEstimate.CompareTo(other.GoalPriceEstimate);

        public override string ToString(int fullPathPrice) => $"{PathPrice}/{fullPathPrice}({InitialGoalPriceEstimate})|| Left:{State.Packages.Count(p => !p.IsInDestinationStore)}: {Action}";
    }
}