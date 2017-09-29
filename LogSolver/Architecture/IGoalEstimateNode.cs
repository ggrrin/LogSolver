namespace LogSolver.Architecture
{
    public interface IGoalEstimateNode<TState, out TNode> : INode<TState,TNode> where TState : class, IState
    {
        int GoalPriceEstimate { get; }
    }
}