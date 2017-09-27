using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.NodeFactories
{
    public class RBFSNodeFactory : INodeFactory<State, RBFSNode<State>> 
    {
        public IRemainerPriceEstimator<State> Estimator { get;  }

        public RBFSNodeFactory(IRemainerPriceEstimator<State> estimator)
        {
            Estimator = estimator;
        }

        public RBFSNode<State> CreateNode(RBFSNode<State> parentNode, IAction<State> action)
        {
            return new RBFSNode<State>(parentNode, action, Estimator, parentNode?.GoalPriceEstimate ?? 0);
        }

    }
}