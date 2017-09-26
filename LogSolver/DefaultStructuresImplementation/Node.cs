using System.Linq;
using System.Text;
using LogSolver.HelperDummyObjects;
using LogSolver.ProblemAbstration;

namespace LogSolver.DefaultStructuresImplementation
{
    public class Node<TState> : INode<TState> where TState : class,IState
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
            Depth = (uint) (((int?)(parent?.Depth) ?? -1) + 1);
            State = action.PerformAction(parent?.State);
        }


        public string Dump()
        {
            var sb = new StringBuilder();
            var node = this;
            while (node != null)
            {
                sb.AppendLine(node.Action.ToString());
                node = node.Parent;
            }
            return sb.ToString();
        }

        public bool IsGoalState()
        {
            var res = State.Packages.FirstOrDefault(p => !p.IsInDestination);
            return res.IsDefault;
        }

    }
}