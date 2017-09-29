using System;
using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class BatchedDropOffAction : ActionBase
    {
        private readonly Package[] packages;
        private readonly Plane plane;

        public BatchedDropOffAction(Package[] packages, Plane plane) : base("BatchedDropOff", packages.Length * 11)
        {
            if(plane.Packages.Count < packages.Length||
                packages.Any(package => package.LocationType != PackageLocationEnum.Plane) || 
                packages.Any(package => !plane.Packages.Contains(package.Identifier)))
                throw new ArgumentException();
            this.packages = packages;
            this.plane = plane;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingBatchedDropOffPackage(packages, plane);
        }

        public override string ToString() => $"{base.ToString()} [{string.Join(/*","*/Environment.NewLine, packages.Select(p => p.Identifier))}]";
        public override string Dump()
        {
            return string.Join(Environment.NewLine, packages.Select(p => $"dropOff {plane.Identifier} {p.Identifier}"));
        }
    }
}