using LogSolver.Structures;

namespace LogSolver.Helpers
{
    public class StackFrame<TNode> where TNode : RBFSNode<State>
    {
        public TNode Node;
        public Heap<TNode> Successors;
        public int F_limit;
    }
}