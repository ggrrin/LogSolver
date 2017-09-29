namespace LogSolver.Architecture
{
    public interface IAction<TState> where TState : IState
    {
        uint Id { get; }
        string Name { get; }
        int ActionCost { get; }
        TState PerformAction(TState parentState);
        string Dump();
    }
}