using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class PickUpAction : ActionBase
    {
        private readonly Plane plane;
        private readonly Package package;

        public PickUpAction(Plane plane, Package package) : base("PickUp", 14)
        {
            this.plane = plane;
            this.package = package;
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