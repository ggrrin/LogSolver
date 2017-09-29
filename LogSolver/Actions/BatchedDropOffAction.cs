using System;
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

        public override string ToString() => $"{base.ToString()} [{string.Join(/*","*/Environment.NewLine, packages.Select(p => p.Identifier))}]";
        public override string Dump()
        {
            return string.Join(Environment.NewLine, packages.Select(p => $"dropOff {p.Location.Planes.First(pl => pl.Packages.Contains(p.Identifier)).Identifier} {p.Identifier}"));
        }
    }
}