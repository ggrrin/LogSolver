namespace LogSolver
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
    }
}