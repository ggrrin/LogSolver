using LogSolver.ProblemAbstration;

namespace LogSolver.DefaultStructuresImplementation
{
    public interface IRemainerPriceEstimator<in TState> where TState : class, IState
    {
        int CalculateEstimate(TState state);
    }
}