using LogSolver.DefaultStructuresImplementation;

namespace LogSolver.ProblemAbstration
{
    public interface INode<TState> where TState : class,IState
    {
        IAction<TState> Action { get; }
        uint Depth { get; }
        Node<TState> Parent { get; }
        int PathPrice { get; }
        TState State { get; set; }

        bool IsGoalState();
    }
}