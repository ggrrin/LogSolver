using System;
using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class DropOffAction : ActionBase
    {
        private readonly Package package;
        private readonly Plane plane;

        public DropOffAction(Package package, Plane plane) : base("DropOff", 11)
        {
            if(plane.IsEmpty || package.LocationType != PackageLocationEnum.Plane|| !plane.Packages.Contains(package.Identifier))
                throw new ArgumentException();
            this.package = package;
            this.plane = plane;
        }

        public DropOffAction() : base("",11)
        {
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingDropOffPackage(package, plane);
        }

        public override string ToString() => $"{base.ToString()} {package}";
        public override string Dump()
        {
            return $"dropOff {plane.Identifier} {package.Identifier}";
        }
    }
}