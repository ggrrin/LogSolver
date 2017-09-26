using LogSolver.DefaultStructuresImplementation;
using LogSolver.HelperDummyObjects;
using LogSolver.ProblemAbstration;

namespace LogSolver.Actions
{
    public class FlyAction: ActionBase
    {
        private readonly City city;
        private readonly Plane plane;

        public FlyAction(City city, Plane plane) : base("Fly", 1000)
        {
            this.city = city;
            this.plane = plane;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingPlaneLocation(plane, city);
        }

        public override string ToString() => $"{Name} {plane} to {city} with [{string.Join(" ,", plane.Packages)}]";
    }
}