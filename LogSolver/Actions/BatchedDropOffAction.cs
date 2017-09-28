using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class BatchedDropOffAction : ActionBase
    {
        private readonly Package[] packages;

        public BatchedDropOffAction(Package[] packages) : base("BatchedDropOff", packages.Length * 11)
        {
            this.packages = packages;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingBatchedDropOffPackage(packages);
        }

        public override string ToString() => $"{base.ToString()} [{string.Join(",", packages.Select(p => p.Identifier))}]";
    }
}