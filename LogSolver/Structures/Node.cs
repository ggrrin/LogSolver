using System.Linq;
using System.Text;
using LogSolver.Architecture;

namespace LogSolver.Structures
{
    public class Node<TState> : INode<TState> where TState : class, IState
    {

        public TState State { get; set; }
        public IAction<TState> Action { get; }
        public int PathPrice { get; }
        public uint Depth { get; }

        public Node<TState> Parent { get; }

        public Node(Node<TState> parent, IAction<TState> action)
        {
            Parent = parent;
            Action = action;
            PathPrice = (parent?.PathPrice ?? 0) + action.ActionCost;
            Depth = (uint)(((int?)(parent?.Depth) ?? -1) + 1);
            State = action.PerformAction(parent?.State);
        }

        public string Dump()
        {
            var sb = new StringBuilder();
            var node = this;
            while (node != null)
            {
                sb.AppendLine(node.ToString(PathPrice));
                node = node.Parent;
            }
            return sb.ToString();
        }

        public virtual string ToString(int fullPathPrice)
        {
            return
                $"{PathPrice}/{fullPathPrice}|| {string.Join(" ", State.Packages.Select(p => $"[{p.Location.Identifier}|{p.Destination.Identifier}]"))} {Action}";
        }

        public override string ToString() => ToString(0);

        public bool IsGoalState()
        {
            return State.Packages.All(p => p.IsInDestination);
        }

    }
}