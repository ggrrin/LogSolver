using LogSolver.DefaultStructuresImplementation;
using LogSolver.HelperDummyObjects;
using LogSolver.ProblemAbstration;

namespace LogSolver.Actions
{
    public class PickUpAction :ActionBase
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
        public override string ToString() => $"{Name} {package} to {plane}";
    }
}