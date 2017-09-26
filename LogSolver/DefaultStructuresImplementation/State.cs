using System.Collections.Generic;
using System.Linq;
using LogSolver.HelperDummyObjects;
using LogSolver.ProblemAbstration;

namespace LogSolver.DefaultStructuresImplementation
{
    public class State : IState
    {
        private readonly uint[] placesCitiesIdentifiers;
        private readonly uint[] airportsIdentifiers;
        private readonly uint[] vansPlaceIdentifiers;
        private readonly uint[] planesLocationsIdentifiers;
        private readonly uint[] packagesLocationIdentifiers;
        private readonly uint[] packagesDestinatioIdentifiers;
        private readonly PackageLocationEnum[] packagesLocationTypes;
        private readonly HashSet<uint>[] vansLoads;
        private readonly HashSet<uint>[] planesLoads;

        public IReadOnlyList<uint> PlacesCitiesIdentifiers => placesCitiesIdentifiers;
        public IReadOnlyList<uint> AirportsIdentifiers => airportsIdentifiers;
        public IReadOnlyList<uint> VansPlaceIdentifiers => vansPlaceIdentifiers;
        public IReadOnlyList<uint> PlanesLocationsIdentifiers => planesLocationsIdentifiers;
        public IReadOnlyList<uint> PackagesLocationIdentifiers => packagesLocationIdentifiers;
        public IReadOnlyList<uint> PackagesDestinatioIdentifiers => packagesDestinatioIdentifiers;
        public IReadOnlyList<PackageLocationEnum> PackagesLocationTypes => packagesLocationTypes;
        public IReadOnlyList<HashSet<uint>> VansLoads => vansLoads;
        public IReadOnlyList<HashSet<uint>> PlanesLoads => planesLoads;

        public State(uint[] placesCitiesIdentifiers, uint[] airportsIdentifiers, uint[] vansPlaceIdentifiers,
            uint[] planesLocationsIdentifiers, uint[] packagesLocationIdentifiers,
            uint[] packagesDestinatioIdentifiers, PackageLocationEnum[] packagesLocationTypes,
            HashSet<uint>[] vansLoads, HashSet<uint>[] planesLoads)
        {
            this.placesCitiesIdentifiers = placesCitiesIdentifiers;
            this.airportsIdentifiers = airportsIdentifiers;
            this.vansPlaceIdentifiers = vansPlaceIdentifiers;
            this.planesLocationsIdentifiers = planesLocationsIdentifiers;
            this.packagesLocationIdentifiers = packagesLocationIdentifiers;
            this.packagesDestinatioIdentifiers = packagesDestinatioIdentifiers;
            this.packagesLocationTypes = packagesLocationTypes;
            this.vansLoads = vansLoads;
            this.planesLoads = planesLoads;
        }


        public IEnumerable<Van> Vans => VansPlaceIdentifiers.Select((v,i) => new Van((uint)i, this));
        public IEnumerable<City> Cities => AirportsIdentifiers.Select((a,i) => new City((uint)i, this));
        public IEnumerable<Plane> Planes => PlanesLocationsIdentifiers.Select((p,i) => new Plane((uint)i, this));
        public IEnumerable<Place> Places => PlacesCitiesIdentifiers.Select((c,i) => new Place((uint)i, this));
        public IEnumerable<Package> Packages => PackagesLocationIdentifiers.Select((p,i) => new Package((uint)i, this));

        public State CloneChangingVanLocation(Van van, Place newPlace)
        {
            var vans = VansPlaceIdentifiers.ToArray();
            vans[van.Identifier] = newPlace.Identifier;

            var xpackagesLocationIdentifiers = packagesLocationIdentifiers.ToArray();
            foreach (var packageIdentifier in van.Packages)
                xpackagesLocationIdentifiers[packageIdentifier] = newPlace.Identifier;

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vans,
                planesLocationsIdentifiers,
                xpackagesLocationIdentifiers,
                packagesDestinatioIdentifiers,
                packagesLocationTypes,
                vansLoads,
                planesLoads);

            return res;
        }

        public State CloneChangingPlaneLocation(Plane plane, City city)
        {
            var planesLocations = PlanesLocationsIdentifiers.ToArray();
            var airports = new HashSet<uint>(AirportsIdentifiers);
            var airportIdentifier = city.Places.First(p => airports.Contains(p.Identifier)).Identifier;
            planesLocations[plane.Identifier] = airportIdentifier;

            var xpackagesLocationIdentifiers = packagesLocationIdentifiers.ToArray();
            foreach (var packageIdentifier in plane.Packages)
                xpackagesLocationIdentifiers[packageIdentifier] = airportIdentifier;

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vansPlaceIdentifiers,
                planesLocations,
                xpackagesLocationIdentifiers,
                packagesDestinatioIdentifiers,
                packagesLocationTypes,
                vansLoads,
                planesLoads);

            return res;
        }

        public State CloneChangingLoadPackage(Package package, Van van)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Van;

            var xvansLoads = vansLoads.ToArray();

            xvansLoads[van.Identifier] = new HashSet<uint>(xvansLoads[van.Identifier]); //clone only modified van
            xvansLoads[van.Identifier].Add(package.Identifier);

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vansPlaceIdentifiers,
                planesLocationsIdentifiers,
                packagesLocationIdentifiers,
                packagesDestinatioIdentifiers,
                xpackagesLocationTypes,
                xvansLoads,
                planesLoads);

            return res;
        }

        public State CloneChangingUnLoadPackage(Package package)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Store;

            var xvansLoads = vansLoads.ToArray();

            var van = package.Location.Vans.First(v => v.Packages.Contains(package.Identifier));
            xvansLoads[van.Identifier] = new HashSet<uint>(xvansLoads[van.Identifier]); //clone only modified van
            xvansLoads[van.Identifier].Remove(package.Identifier);

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vansPlaceIdentifiers,
                planesLocationsIdentifiers,
                packagesLocationIdentifiers,
                packagesDestinatioIdentifiers,
                xpackagesLocationTypes,
                xvansLoads,
                planesLoads);

            return res;
        }

        public State CloneChangingPickUpPackage(Package package, Plane plane)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Plane;

            var xplanesLoads = planesLoads.ToArray();

            xplanesLoads[plane.Identifier] = new HashSet<uint>(xplanesLoads[plane.Identifier]); //clone only modified van
            xplanesLoads[plane.Identifier].Add(package.Identifier);

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vansPlaceIdentifiers,
                planesLocationsIdentifiers,
                packagesLocationIdentifiers,
                packagesDestinatioIdentifiers,
                xpackagesLocationTypes,
                vansLoads,
                xplanesLoads);

            return res;
        }

        public State CloneChangingDropOffPackage(Package package)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Store;

            var xplanesLoads = planesLoads.ToArray();

            var plane = package.Location.Planes.First(v => v.Packages.Contains(package.Identifier));
            xplanesLoads[plane.Identifier] = new HashSet<uint>(xplanesLoads[plane.Identifier]); //clone only modified van
            xplanesLoads[plane.Identifier].Remove(package.Identifier);

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vansPlaceIdentifiers,
                planesLocationsIdentifiers,
                packagesLocationIdentifiers,
                packagesDestinatioIdentifiers,
                xpackagesLocationTypes,
                vansLoads,
                xplanesLoads);

            return res;
        }

    }
}