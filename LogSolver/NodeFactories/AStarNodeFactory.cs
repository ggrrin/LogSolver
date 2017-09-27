using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.NodeFactories
{
    public class AStarNodeFactory : INodeFactory<State, AStarNode<State>>
    {
        public IRemainerPriceEstimator<State> Estimator { get; }
        public AStarNodeFactory(IRemainerPriceEstimator<State> estimator)
        {
            Estimator = estimator;
        }

        public AStarNode<State> CreateNode(AStarNode<State> parentNode, IAction<State> action)
        {
            return new AStarNode<State>(parentNode, action, Estimator);
        }
    }
}