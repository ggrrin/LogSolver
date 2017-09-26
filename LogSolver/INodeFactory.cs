namespace LogSolver
{
    public interface INodeFactory<TState, TNode> where TState : IState where TNode : INode<TState>
    {
        Node<TState> CreateNode(TNode parentNode, IAction<TState> action);
    }
}