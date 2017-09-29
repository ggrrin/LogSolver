using LogSolver.Structures;

namespace LogSolver.Architecture
{
    public interface INode<TState, out TNode> where TState : class,IState
    {
        IAction<TState> Action { get; }
        uint Depth { get; }
        TNode Parent { get; }
        int PathPrice { get; }
        TState State { get; set; }

        bool IsGoalState();
    }
}