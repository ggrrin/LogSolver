using System.Linq;
using System.Runtime.ExceptionServices;
using LogSolver.Actions;
using LogSolver.Architecture;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Estimators
{
    public class PathRemaninerPriceEstimator : IRemainerPriceEstimator<State>
    {
        public int CalculateEstimate(State state)
        {
            int freePlaneStorageCount = 1;// + state.Planes.Min(p => p.FreeStorageCount);
            int freeVanStorageCount = 1;// + state.Vans.Min(v => v.FreeStorageCount);

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
                        //no empty van there
                        if (!package.Location.Vans.Any(v => !v.IsFull))
                            other += new DriveAction().ActionCost;

                        if (package.LocationType != PackageLocationEnum.Van)
                            other += new LoadAction().ActionCost;

                        totalDriveCost += new DriveAction().ActionCost;
                        other += new UnLoadAction().ActionCost;
                    }

                    //no plane there
                    if (!package.Location.City.Planes.Any(p => !p.IsFull))
                        other += new FlyAction().ActionCost;

                    //fly
                    if (package.LocationType != PackageLocationEnum.Plane)
                        other += new PickUpAction().ActionCost;

                    totalFlyCost += new FlyAction().ActionCost;

                    other += new DropOffAction().ActionCost;

                    //have to get from the airport
                    if (!package.Destination.IsAirport)
                    {
                        //no empty van there
                        if (!package.Destination.Vans.Any(v => !v.IsFull))
                            other += new DriveAction().ActionCost;

                        other += new LoadAction().ActionCost;
                        totalDriveCost += new DriveAction().ActionCost;
                        other += new UnLoadAction().ActionCost;
                    }
                }
                else //move just in city
                {
                    //no empty van there
                    if (!package.Location.Vans.Any(v => !v.IsFull))
                        other += new DriveAction().ActionCost;

                    if (package.LocationType == PackageLocationEnum.Plane)
                        other += new UnLoadAction().ActionCost;

                    if (package.LocationType != PackageLocationEnum.Van)
                        other += new LoadAction().ActionCost;

                    totalDriveCost += new DriveAction().ActionCost;

                    other += new UnLoadAction().ActionCost;
                }
            }
            return other + totalFlyCost / freePlaneStorageCount + totalDriveCost / freeVanStorageCount;
        }
    }
}