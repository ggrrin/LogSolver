using System;
using System.Collections.Generic;
using LogSolver.DummyObjects;
using LogSolver.Helpers;

namespace LogSolver.Architecture
{
    public interface IState : IEquatable<IState>
    {
        IEnumerable<City> Cities { get; }
        IEnumerable<Package> Packages { get; }
        IEnumerable<Place> Places { get; }
        IEnumerable<Plane> Planes { get; }
        IEnumerable<Van> Vans { get; }

        IReadOnlyList<HashSet<uint>> PlanesLoads { get; }
        IReadOnlyList<HashSet<uint>> VansLoads { get; }
        IReadOnlyList<uint> PackagesLocationIdentifiers { get; }
        IReadOnlyList<uint> PlanesLocationsIdentifiers { get; }
        IReadOnlyList<uint> VansPlaceIdentifiers { get; }
        IReadOnlyList<PackageLocationEnum> PackagesLocationTypes { get; }
    }
}