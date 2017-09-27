namespace LogSolver.Architecture
{
    public interface IRemainerPriceEstimator<TState> where TState : class, IState
    {
        int CalculateEstimate(TState state);
    }
}