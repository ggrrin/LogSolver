using System.Linq;
using LogSolver.Actions;
using LogSolver.HelperDummyObjects;

namespace LogSolver.DefaultStructuresImplementation
{
    public class PathRemaninerPriceEstimator : IRemainerPriceEstimator<State>
    {
        public int CalculateEstimate(State state)
        {
            int freePlaneStorageCount = 1;// + state.Planes.Min(p => p.FreeStorageCount);
            int freeVanStorageCount = 1;// +state.Vans.Min(v => v.FreeStorageCount);
            
            int other = 0;
            int totalFlyCost = 0;
            int totalDriveCost = 0;
            foreach (var package in state.Packages.Where(p => !p.IsInDestination))
            {
                //have to fly
                if (package.Location.City != package.Destination.City)
                {
                    //have to get to the airport
                    if (!package.Location.IsAirport)
                    {
                        if (package.LocationType != PackageLocationEnum.Van)
                        {
                            other += new LoadAction(default(Package), default(Van)).ActionCost;
                        }
                        totalDriveCost += new DriveAction(default(Van), default(Place)).ActionCost;
                        other += new UnLoadAction(default(Package)).ActionCost;
                    }

                    //fly
                    if (package.LocationType != PackageLocationEnum.Plane)
                        other += new PickUpAction(default(Plane), default(Package)).ActionCost;

                    totalFlyCost += new FlyAction(default(City), default(Plane)).ActionCost;

                    other += new DropOffAction(default(Package)).ActionCost;

                    //have to get from the airport
                    if (!package.Destination.IsAirport)
                    {
                        other += new LoadAction(default(Package), default(Van)).ActionCost;
                        totalDriveCost += new DriveAction(default(Van), default(Place)).ActionCost;
                        other += new UnLoadAction(default(Package)).ActionCost;
                    }
                }
                else //move just in city
                {
                    other += new LoadAction(default(Package), default(Van)).ActionCost;
                    totalDriveCost += new DriveAction(default(Van), default(Place)).ActionCost;
                    other += new UnLoadAction(default(Package)).ActionCost;
                }
            }
            return other + totalFlyCost / freePlaneStorageCount + totalDriveCost /freeVanStorageCount;
        }
    }
}