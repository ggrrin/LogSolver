using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class DriveAction: ActionBase 
    {
        private readonly Van van;
        private readonly Place newPlace;

        public DriveAction(Van van, Place newPlace) : base("Drive", 17)
        {
            this.van = van;
            this.newPlace = newPlace;
        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingVanLocation(van, newPlace);
        }

        public override string ToString() => $"{base.ToString()} {van} to {newPlace} with [{string.Join(" ,", van.Packages)}]";
    }
}