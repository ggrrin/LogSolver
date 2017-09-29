using System;
using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class BatchedLoadAction : ActionBase
    {
        private readonly Package[] packages;
        private readonly Van van;

        public BatchedLoadAction(Package[] packages, Van van) : base("BatchedLoad", packages.Length*2)
        {
            if(van.FreeStorageCount < packages.Length || packages.Any(package => package.LocationType != PackageLocationEnum.Store))
                throw new ArgumentException();
            this.packages = packages;
            this.van = van;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingBatchedLoadPackage(packages, van);
        }


        public override string ToString() => $"{base.ToString()} [{string.Join(/*", "*/Environment.NewLine,packages.Select(p => p.Identifier))}] to {van}";
        public override string Dump()
        {
            return string.Join(Environment.NewLine, packages.Select(p => $"load {van.Identifier} {p.Identifier}"));
        }
    }
}