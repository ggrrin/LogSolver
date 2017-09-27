using System.Linq;
using System.Runtime.ExceptionServices;
using LogSolver.Actions;
using LogSolver.HelperDummyObjects;

namespace LogSolver.DefaultStructuresImplementation
{
    public class SimpleRemainerPriceEstimator : IRemainerPriceEstimator<State>
    {
        public int CalculateEstimate(State state)
        {
            return state.Packages.Where(p => !p.IsInDestination).Count() * 2;
        }
    }

    public class PathRemaninerPriceEstimator : IRemainerPriceEstimator<State>
    {
        public int CalculateEstimate(State state)
        {
            int freePlaneStorageCount = 1 + state.Planes.Min(p => p.FreeStorageCount);
            int freeVanStorageCount = 1 +state.Vans.Min(v => v.FreeStorageCount);
            
            int other = 0;
            int totalFlyCost = 0;
            int totalDriveCost = 0;
            foreach (var package in state.Packages.Where(p => !p.IsInDestination))
            {
                if (package.Location.City != package.Destination.City)
                {
                    if (!package.Location.IsAirport)
                    {
                        if (package.LocationType != PackageLocationEnum.Van)
                        {
                            other += new LoadAction(default(Package), default(Van)).ActionCost;
                        }
                        totalDriveCost += new DriveAction(default(Van), default(Place)).ActionCost;
                        other += new UnLoadAction(default(Package)).ActionCost;
                    }

                    if (package.LocationType != PackageLocationEnum.Plane)
                    {
                        other += new PickUpAction(default(Plane), default(Package)).ActionCost;
                    }

                    totalFlyCost += new FlyAction(default(City), default(Plane)).ActionCost;

                    other += new DropOffAction(default(Package)).ActionCost;

                    if (!package.Destination.IsAirport)
                    {
                        other += new LoadAction(default(Package), default(Van)).ActionCost;
                        totalDriveCost += new DriveAction(default(Van), default(Place)).ActionCost;
                        other += new UnLoadAction(default(Package)).ActionCost;
                    }
                }
            }
            return other + totalFlyCost / freePlaneStorageCount + totalDriveCost /freeVanStorageCount;
        }
    }
}