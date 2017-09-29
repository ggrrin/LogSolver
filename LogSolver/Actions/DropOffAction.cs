using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class DropOffAction : ActionBase
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

        public override string ToString() => $"{base.ToString()} {package}";
        public override string Dump()
        {
            return $"dropOff {package.Location.Planes.First(p => p.Packages.Contains(package.Identifier)).Identifier} {package.Identifier}";
        }
    }
}