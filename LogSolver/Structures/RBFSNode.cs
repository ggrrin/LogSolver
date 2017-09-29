using System;
using System.Linq;
using LogSolver.Architecture;

namespace LogSolver.Structures
{
    public class RBFSNode<TState> : RBFSNode<TState, RBFSNode<TState>> where TState : class, IState
    {
        public RBFSNode(RBFSNode<TState> parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator, int parentEstimate) : base(parent, action, estimator, parentEstimate)
        {
        }
    }

    public class RBFSNode<TState, TNode> : Node<TState, TNode>, IGoalEstimateNode<TState, TNode>, IComparable<RBFSNode<TState,TNode>>
        where TState : class, IState 
        where TNode : Node<TState, TNode>
    {

        public int GoalPriceEstimate { get; set; }

        public int InitialGoalPriceEstimate { get; }

        public RBFSNode(TNode parent, IAction<TState> action, IRemainerPriceEstimator<TState> estimator, int parentEstimate) : base(parent, action)
        {
            InitialGoalPriceEstimate = GoalPriceEstimate = Math.Max(PathPrice + estimator.CalculateEstimate(State), parentEstimate);
        }

        public int CompareTo(RBFSNode<TState, TNode> other) => GoalPriceEstimate.CompareTo(other.GoalPriceEstimate);

        public override string ToString(int fullPathPrice) => $"Id[{Action.Id}]: {PathPrice}/{fullPathPrice}({InitialGoalPriceEstimate})|| Left:{State.Packages.Count(p => !p.IsInDestinationStore)}: {Action}";

    }
}