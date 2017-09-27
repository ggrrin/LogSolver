using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class LoadAction : ActionBase
    {
        private readonly Package package;
        private readonly Van van;

        public LoadAction(Package package, Van van) : base("Load", 2)
        {
            this.package = package;
            this.van = van;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingLoadPackage(package, van);
        }


        public override string ToString() => $"{base.ToString()} {package} to {van}";
    }
}