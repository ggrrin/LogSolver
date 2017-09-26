using LogSolver.DefaultStructuresImplementation;
using LogSolver.HelperDummyObjects;
using LogSolver.ProblemAbstration;

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
    }
}