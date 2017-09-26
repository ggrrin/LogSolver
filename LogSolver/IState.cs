using System.Collections.Generic;

namespace LogSolver
{
    public interface IState
    {
        IReadOnlyList<uint> AirportsIdentifiers { get; }
        IEnumerable<City> Cities { get; }
        IEnumerable<Package> Packages { get; }
        IReadOnlyList<uint> PackagesDestinatioIdentifiers { get; }
        IReadOnlyList<uint> PackagesLocationIdentifiers { get; }
        IReadOnlyList<PackageLocationEnum> PackagesLocationTypes { get; }
        IEnumerable<Place> Places { get; }
        IReadOnlyList<uint> PlacesCitiesIdentifiers { get; }
        IEnumerable<Plane> Planes { get; }
        IReadOnlyList<HashSet<uint>> PlanesLoads { get; }
        IReadOnlyList<uint> PlanesLocationsIdentifiers { get; }
        IEnumerable<Van> Vans { get; }
        IReadOnlyList<HashSet<uint>> VansLoads { get; }
        IReadOnlyList<uint> VansPlaceIdentifiers { get; }
    }
}