using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class UnLoadAction : ActionBase
    {
        private readonly Package package;

        public UnLoadAction(Package package) : base("UnLoad", 2)
        {
            this.package = package;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingUnLoadPackage(package);
        }

        public override string ToString() => $"{base.ToString()} {package}";
    }
}