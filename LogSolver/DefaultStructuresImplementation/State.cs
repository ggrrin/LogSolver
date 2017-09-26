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

        public IReadOnlyList<uint> PlacesCitiesIdentifiers
        {
            get { return placesCitiesIdentifiers; }
        }

        public IReadOnlyList<uint> AirportsIdentifiers
        {
            get { return airportsIdentifiers; }
        }

        public IReadOnlyList<uint> VansPlaceIdentifiers
        {
            get { return vansPlaceIdentifiers; }
        }

        public IReadOnlyList<uint> PlanesLocationsIdentifiers
        {
            get { return planesLocationsIdentifiers; }
        }

        public IReadOnlyList<uint> PackagesLocationIdentifiers
        {
            get { return packagesLocationIdentifiers; }
        }

        public IReadOnlyList<uint> PackagesDestinatioIdentifiers
        {
            get { return packagesDestinatioIdentifiers; }
        }

        public IReadOnlyList<PackageLocationEnum> PackagesLocationTypes
        {
            get { return packagesLocationTypes; }
        }

        public IReadOnlyList<HashSet<uint>> VansLoads
        {
            get { return vansLoads; }
        }

        public IReadOnlyList<HashSet<uint>> PlanesLoads
        {
            get { return planesLoads; }
        }


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


        public IEnumerable<Van> Vans => VansPlaceIdentifiers.Select((i, v) => new Van(i, this));
        public IEnumerable<City> Cities => AirportsIdentifiers.Select((i, c) => new City(i, this));
        public IEnumerable<Plane> Planes => PlanesLocationsIdentifiers.Select((i, p) => new Plane(i, this));
        public IEnumerable<Place> Places => PlacesCitiesIdentifiers.Select((i, c) => new Place(i, this));
        public IEnumerable<Package> Packages => PackagesLocationIdentifiers.Select((i, p) => new Package(i, this));

        public State CloneChangingVanLocation(Van van, Place newPlace)
        {
            var vans = VansPlaceIdentifiers.ToArray();
            vans[van.Identifier] = newPlace.Identifier;

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vans,
                planesLocationsIdentifiers,
                packagesLocationIdentifiers,
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
            planesLocations[plane.Identifier] = city.Places.First(p => airports.Contains(p.Identifier)).Identifier;

            var res = new State(placesCitiesIdentifiers,
                airportsIdentifiers,
                vansPlaceIdentifiers,
                planesLocations,
                packagesLocationIdentifiers,
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

    }
}