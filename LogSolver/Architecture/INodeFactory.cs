namespace LogSolver.Architecture
{
    public interface INodeFactory<TState, TNode> where TState : class,IState where TNode : INode<TState,TNode>
    {
        TNode CreateNode(TNode parentNode, IAction<TState> action);
    }
}