using System;
using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class BatchedUnLoadAction : ActionBase
    {
        private readonly Package[] packages;
        private readonly Van van;

        public BatchedUnLoadAction(Package[] packages, Van van) : base("BatchedUnLoad", packages.Length * 2)
        {
            if(van.Packages.Count < packages.Length||
                packages.Any(package => package.LocationType != PackageLocationEnum.Van) || 
                packages.Any(package => !van.Packages.Contains(package.Identifier)))
                throw new ArgumentException();
            this.packages = packages;
            this.van = van;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingBatchedUnLoadPackage(packages, van);
        }

        public override string ToString() => $"{base.ToString()} [{string.Join(/*", "*/Environment.NewLine, packages.Select(p => p.Identifier))}]";
        public override string Dump()
        {
            return string.Join(Environment.NewLine, packages.Select(p => $"unload {van.Identifier} {p.Identifier}"));
        }
    }
}