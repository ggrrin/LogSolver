using System;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class PickUpAction : ActionBase
    {
        private readonly Plane plane;
        private readonly Package package;

        public PickUpAction(Plane plane, Package package) : base("PickUp", 14)
        {
            if(plane.IsFull || package.LocationType != PackageLocationEnum.Store)
                throw new ArgumentException();
            this.plane = plane;
            this.package = package;
        }

        public PickUpAction() : base(String.Empty, 14)
        {
            

        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingPickUpPackage(package, plane);
        }
        public override string ToString() => $"{base.ToString()} {package} to {plane}";
        public override string Dump()
        {
            return $"pickUp {plane.Identifier} {package.Identifier}";
        }
    }
}