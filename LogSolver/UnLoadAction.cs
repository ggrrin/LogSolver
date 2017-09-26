namespace LogSolver
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
    }
}