namespace LogSolver
{
    public interface INode<TState> where TState : IState
    {
        IAction<TState> Action { get; }
        uint Depth { get; }
        Node<TState> Parent { get; }
        int PathPrice { get; }
        TState State { get; set; }

        bool IsGoalState();
    }
}