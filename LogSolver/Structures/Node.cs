using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogSolver.Architecture;

namespace LogSolver.Structures
{
    public class Node<TState> : Node<TState, Node<TState>> where TState : class, IState
    {
        public Node(Node<TState> parent, IAction<TState> action) : base(parent, action)
        {
        }
    }

    public class Node<TState, TNode> : INode<TState, TNode> where TState : class, IState where TNode : Node<TState, TNode>
    {

        public TState State { get; set; }
        public IAction<TState> Action { get; }
        public int PathPrice { get; }
        public uint Depth { get; }
        public TNode Parent { get; }


        public Node(TNode parent, IAction<TState> action)
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
            var stack = new Stack<Node<TState,TNode>>();

            var node = this;
            while (node != null)
            {
                stack.Push(node);
                node = node.Parent;
            }


            foreach (var n in stack)
                sb.AppendLine(n.ToString(PathPrice));

            return sb.ToString();
        }

        public virtual string ToString(int fullPathPrice)
        {
            return
                $"Id[{Action.Id}]: {PathPrice}/{fullPathPrice}|| {string.Join(" ", State.Packages.Select(p => $"[{p.Location.Identifier}|{p.Destination.Identifier}]"))} {Action}";
        }

        public override string ToString() => ToString(0);

        public bool IsGoalState()
        {
            return State.Packages.All(p => p.IsInDestinationStore);
        }

    }
}