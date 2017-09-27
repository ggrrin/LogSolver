namespace LogSolver.Architecture
{
    public interface IRemainerPriceEstimator<in TState> where TState : class, IState
    {
        int CalculateEstimate(TState state);
    }
}