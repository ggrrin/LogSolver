namespace LogSolver.ProblemAbstration
{
    public interface IAction<TState> where TState : IState
    {
        string Name { get; }
        int ActionCost { get; }
        TState PerformAction(TState parentState);
    }
}