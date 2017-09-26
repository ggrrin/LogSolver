using System.Linq;

namespace LogSolver
{
    public class Node<TState> : INode<TState> where TState : IState
    {

        public TState State { get; set; }
        public IAction<TState> Action { get; }
        public int PathPrice { get; }
        public uint Depth { get; }

        public Node<TState> Parent { get; }

        public Node(Node<TState> parent, IAction<TState> action)
        {
            Parent = parent;
            State = action.PerformAction(parent.State);
            Action = action;
            PathPrice = parent.PathPrice + action.ActionCost; 
            Depth = parent.Depth + 1;
        }

        public bool IsGoalState()
        {
            var res = State.Packages.FirstOrDefault(p => !p.IsInDestination);
            return res == default(Package);
        }

    }
}