namespace LogSolver.Architecture
{
    public interface IGoalEstimateNode<TState> : INode<TState> where TState : class, IState
    {
        int GoalPriceEstimate { get; }
    }
}