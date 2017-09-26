using LogSolver.DefaultStructuresImplementation;
using LogSolver.HelperDummyObjects;
using LogSolver.ProblemAbstration;

namespace LogSolver.Actions
{
    public class DropOffAction :ActionBase
    {
        private readonly Package package;

        public DropOffAction(Package package) : base("DropOff", 11)
        {
            this.package = package;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingDropOffPackage(package);
        }

        public override string ToString() => $"{Name} {package}";
    }
}