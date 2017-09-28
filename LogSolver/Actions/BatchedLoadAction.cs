using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class BatchedLoadAction : ActionBase
    {
        private readonly Package[] packages;
        private readonly Van van;

        public BatchedLoadAction(Package[] packages, Van van) : base("BatchedLoad", packages.Length*2)
        {
            this.packages = packages;
            this.van = van;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingBatchedLoadPackage(packages, van);
        }


        public override string ToString() => $"{base.ToString()} [{string.Join(", ",packages.Select(p => p.Identifier))}] to {van}";
    }
}