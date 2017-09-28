using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class BatchedUnLoadAction : ActionBase
    {
        private readonly Package[] packages;

        public BatchedUnLoadAction(Package[] packages) : base("BatchedUnLoad", packages.Length * 2)
        {
            this.packages = packages;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingBatchedUnLoadPackage(packages);
        }

        public override string ToString() => $"{base.ToString()} [{string.Join(", ", packages.Select(p => p.Identifier))}]";
    }
}