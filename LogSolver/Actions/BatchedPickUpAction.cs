using System;
using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class BatchedPickUpAction : ActionBase
    {
        private readonly Plane plane;
        private readonly Package[] packages;

        public BatchedPickUpAction(Plane plane, Package[] packages) : base("BatchedPickUp", 14 * packages.Length)
        {
            this.plane = plane;
            this.packages = packages;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingBatchedPickUpPackage(packages, plane);
        }
        public override string ToString() => $"{base.ToString()} [{string.Join(/*", "*/Environment.NewLine, packages.Select(p => p.Identifier))}] to {plane}";
        public override string Dump()
        {
            return string.Join(Environment.NewLine, packages.Select(p => $"pickUp {plane.Identifier} {p.Identifier}"));
        }
    }
}