using System.Linq;
using LogSolver.Actions;
using LogSolver.Architecture;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Estimators
{
    public class YPriceEstimator : IRemainerPriceEstimator<State>
    {
        public int CalculateEstimate(State state)
        {
            int citiesWithNotDeliverd = 0;
            int placesWithNotDeliverd = 0;
            int placesWithoutVans = 0;
            int placesWithoutPlanes = 0;
            int notUnloadedVansPackages = 0;
            int notUnloadedPlanesPackages = 0;
            int vansPackages = state.Vans.Sum(v => v.Packages.Count);
            int airplanePackages = state.Planes.Sum(v => v.Packages.Count);

            foreach (var package in state.Packages.Where(p => !p.IsInDestination))
            {
                if (package.Location.City != package.Destination.City)
                    citiesWithNotDeliverd++;

                if (package.Location != package.Destination)
                    placesWithNotDeliverd++;
                else if (package.LocationType == PackageLocationEnum.Van)
                    notUnloadedVansPackages++;
                else if (package.LocationType == PackageLocationEnum.Plane)
                    notUnloadedPlanesPackages++;

                if (!package.Location.Vans.Any())
                    placesWithoutVans++;

                if (package.Location.IsAirport && !package.Location.Planes.Any())
                    placesWithoutPlanes++;
            }

            return placesWithoutVans * 2 * new DriveAction(default(Van), default(Place)).ActionCost +
                   notUnloadedVansPackages * new UnLoadAction(default(Package)).ActionCost +
                   vansPackages * new UnLoadAction(default(Package)).ActionCost +
                   placesWithNotDeliverd * new DriveAction(default(Van), default(Place)).ActionCost +
                   placesWithoutPlanes * 2 * new FlyAction(default(City), default(Plane)).ActionCost +
                   notUnloadedPlanesPackages * new DropOffAction(default(Package)).ActionCost +
                   airplanePackages * new DropOffAction(default(Package)).ActionCost +
                   citiesWithNotDeliverd * new FlyAction(default(City), default(Plane)).ActionCost;

            //fucing fast!!!
            //return placesWithoutVans * 2 * new DriveAction(default(Van), default(Place)).ActionCost +
            //       notUnloadedVansPackages * new UnLoadAction(default(Package)).ActionCost +
            //       vansPackages * new UnLoadAction(default(Package)).ActionCost +
            //       placesWithNotDeliverd * new DriveAction(default(Van), default(Place)).ActionCost;

        }
    }
}