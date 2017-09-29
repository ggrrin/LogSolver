using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.IO
{
    public class ResultDumper<TNode> where TNode : INode<State,TNode>
    {
        public void Dump(TNode goalNode, string outputFilePath)
        {
            var sb = new StringBuilder();
            var stack = new Stack<INode<State,TNode>>();

            var node = goalNode;
            while (node != null)
            {
                stack.Push(node);
                node = node.Parent;
            }

            

            foreach (var n in stack.Skip(1))//skipp init
            {
                sb.AppendLine(n.Action.Dump());
            }

            File.WriteAllText(outputFilePath, sb.ToString());
        }

    }
}