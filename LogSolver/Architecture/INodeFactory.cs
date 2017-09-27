namespace LogSolver.Architecture
{
    public interface INodeFactory<TState, TNode> where TState : class,IState where TNode : INode<TState>
    {
        TNode CreateNode(TNode parentNode, IAction<TState> action);
    }
}