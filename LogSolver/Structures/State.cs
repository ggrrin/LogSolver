using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.DummyObjects;
using LogSolver.Helpers;

namespace LogSolver.Structures
{
    public class State : IState
    {
        public static HashSet<uint> AirportsIdentifiers { get; protected set; }
        public static IReadOnlyList<uint> PackagesDestinatioIdentifiers { get; protected set; }
        public static IReadOnlyList<uint> PlacesCitiesIdentifiers { get; protected set; }

        private readonly uint[] vansPlaceIdentifiers;
        private readonly uint[] planesLocationsIdentifiers;
        private readonly uint[] packagesLocationIdentifiers;
        private readonly PackageLocationEnum[] packagesLocationTypes;
        private readonly HashSet<uint>[] vansLoads;
        private readonly HashSet<uint>[] planesLoads;

        public IReadOnlyList<uint> VansPlaceIdentifiers => vansPlaceIdentifiers;
        public IReadOnlyList<uint> PlanesLocationsIdentifiers => planesLocationsIdentifiers;
        public IReadOnlyList<uint> PackagesLocationIdentifiers => packagesLocationIdentifiers;
        public IReadOnlyList<PackageLocationEnum> PackagesLocationTypes => packagesLocationTypes;
        public IReadOnlyList<HashSet<uint>> VansLoads => vansLoads;
        public IReadOnlyList<HashSet<uint>> PlanesLoads => planesLoads;


        public static State CreateInitState(uint[] placesCitiesIdentifiers, uint[] vansPlaceIdentifiers, uint[] planesLocationsIdentifiers,
            uint[] packagesLocationIdentifiers, uint[] airportsIdentifiers, uint[] packagesDestinationIdentifiers)
        {
            AirportsIdentifiers = new HashSet<uint>(airportsIdentifiers);
            PackagesDestinatioIdentifiers = packagesDestinationIdentifiers;
            PlacesCitiesIdentifiers = placesCitiesIdentifiers;

            return new State(vansPlaceIdentifiers, planesLocationsIdentifiers, packagesLocationIdentifiers,
                    Enumerable.Repeat(PackageLocationEnum.Store, packagesLocationIdentifiers.Length).ToArray(),
                    Enumerable.Repeat(new HashSet<uint>(), vansPlaceIdentifiers.Length).ToArray(),
                    Enumerable.Repeat(new HashSet<uint>(), planesLocationsIdentifiers.Length).ToArray());
        }

        protected State(uint[] vansPlaceIdentifiers, uint[] planesLocationsIdentifiers, uint[] packagesLocationIdentifiers,
            PackageLocationEnum[] packagesLocationTypes, HashSet<uint>[] vansLoads, HashSet<uint>[] planesLoads)
        {
            this.vansPlaceIdentifiers = vansPlaceIdentifiers;
            this.planesLocationsIdentifiers = planesLocationsIdentifiers;
            this.packagesLocationIdentifiers = packagesLocationIdentifiers;
            this.packagesLocationTypes = packagesLocationTypes;
            this.vansLoads = vansLoads;
            this.planesLoads = planesLoads;
        }


        public IEnumerable<Van> Vans => VansPlaceIdentifiers.Select((v, i) => new Van((uint)i, this));
        public IEnumerable<City> Cities => AirportsIdentifiers.Select((a, i) => new City((uint)i, this));
        public IEnumerable<Plane> Planes => PlanesLocationsIdentifiers.Select((p, i) => new Plane((uint)i, this));
        public IEnumerable<Place> Places => PlacesCitiesIdentifiers.Select((c, i) => new Place((uint)i, this));
        public IEnumerable<Package> Packages => PackagesLocationIdentifiers.Select((p, i) => new Package((uint)i, this));



        public State CloneChangingVanLocation(Van van, Place newPlace)
        {
            var xvansPlaceIdentifiers = VansPlaceIdentifiers.ToArray();
            xvansPlaceIdentifiers[van.Identifier] = newPlace.Identifier;

            var xpackagesLocationIdentifiers = packagesLocationIdentifiers.ToArray();
            foreach (var packageIdentifier in van.Packages)
                xpackagesLocationIdentifiers[packageIdentifier] = newPlace.Identifier;

            return new State(xvansPlaceIdentifiers, planesLocationsIdentifiers, xpackagesLocationIdentifiers, packagesLocationTypes, vansLoads, planesLoads);
        }

        public State CloneChangingPlaneLocation(Plane plane, City city)
        {
            var xplanesLocationIdentifiers = PlanesLocationsIdentifiers.ToArray();
            var airports = new HashSet<uint>(AirportsIdentifiers);
            var airportIdentifier = city.Places.First(p => airports.Contains(p.Identifier)).Identifier;
            xplanesLocationIdentifiers[plane.Identifier] = airportIdentifier;

            var xpackagesLocationIdentifiers = packagesLocationIdentifiers.ToArray();
            foreach (var packageIdentifier in plane.Packages)
                xpackagesLocationIdentifiers[packageIdentifier] = airportIdentifier;

            return new State(vansPlaceIdentifiers, xplanesLocationIdentifiers, xpackagesLocationIdentifiers, packagesLocationTypes, vansLoads, planesLoads);
        }

        public State CloneChangingLoadPackage(Package package, Van van)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Van;

            var xvansLoads = vansLoads.ToArray();

            xvansLoads[van.Identifier] = new HashSet<uint>(xvansLoads[van.Identifier]); //clone only modified van
            xvansLoads[van.Identifier].Add(package.Identifier);

            return new State(vansPlaceIdentifiers, planesLocationsIdentifiers, packagesLocationIdentifiers, xpackagesLocationTypes, xvansLoads, planesLoads);
        }

        public State CloneChangingUnLoadPackage(Package package)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Store;

            var xvansLoads = vansLoads.ToArray();

            var van = package.Location.Vans.First(v => v.Packages.Contains(package.Identifier));
            xvansLoads[van.Identifier] = new HashSet<uint>(xvansLoads[van.Identifier]); //clone only modified van
            xvansLoads[van.Identifier].Remove(package.Identifier);

            return new State(vansPlaceIdentifiers, planesLocationsIdentifiers, packagesLocationIdentifiers, xpackagesLocationTypes, xvansLoads, planesLoads);
        }

        public State CloneChangingPickUpPackage(Package package, Plane plane)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Plane;

            var xplanesLoads = planesLoads.ToArray();

            xplanesLoads[plane.Identifier] = new HashSet<uint>(xplanesLoads[plane.Identifier]); //clone only modified van
            xplanesLoads[plane.Identifier].Add(package.Identifier);

            return new State(vansPlaceIdentifiers, planesLocationsIdentifiers, packagesLocationIdentifiers, xpackagesLocationTypes, vansLoads, xplanesLoads);
        }

        public State CloneChangingDropOffPackage(Package package)
        {
            var xpackagesLocationTypes = packagesLocationTypes.ToArray();
            xpackagesLocationTypes[package.Identifier] = PackageLocationEnum.Store;

            var xplanesLoads = planesLoads.ToArray();

            var plane = package.Location.Planes.First(v => v.Packages.Contains(package.Identifier));
            xplanesLoads[plane.Identifier] = new HashSet<uint>(xplanesLoads[plane.Identifier]); //clone only modified van
            xplanesLoads[plane.Identifier].Remove(package.Identifier);

            return new State(vansPlaceIdentifiers, planesLocationsIdentifiers, packagesLocationIdentifiers, xpackagesLocationTypes, vansLoads, xplanesLoads);
        }

        public bool Equals(IState other)
        {
            if (other == null)
                return false;

            bool res = Enumerable.SequenceEqual(VansPlaceIdentifiers, other.VansPlaceIdentifiers) &&
                       Enumerable.SequenceEqual(PlanesLocationsIdentifiers, other.PlanesLocationsIdentifiers) &&
                       Enumerable.SequenceEqual(PackagesLocationIdentifiers, other.PackagesLocationIdentifiers) &&
                       Enumerable.SequenceEqual(packagesLocationTypes, other.PackagesLocationTypes) &&

                       vansLoads.Zip(other.VansLoads,
                                (loads1, loads2) => loads1.All(l1 => loads2.Contains(l1)) && loads2.All(l2 => loads1.Contains(l2)))
                            .All(x => x == true) &&

                       planesLoads.Zip(other.PlanesLoads,
                                (loads1, loads2) => loads1.All(l1 => loads2.Contains(l1)) && loads2.All(l2 => loads1.Contains(l2)))
                            .All(x => x == true);
            return res;
        }

        public static bool operator ==(State s1, State s2) => !object.ReferenceEquals(s1, null) && s1.Equals(s2);

        public static bool operator !=(State s1, State s2)
        {
            return !(s1 == s2);
        }

        public override int GetHashCode()
        {
            int resultHash = ((IStructuralEquatable)vansPlaceIdentifiers).GetHashCode(EqualityComparer<uint>.Default) ^
                ((IStructuralEquatable)planesLocationsIdentifiers).GetHashCode(EqualityComparer<uint>.Default) ^
                ((IStructuralEquatable)packagesLocationIdentifiers).GetHashCode(EqualityComparer<uint>.Default) ^
                ((IStructuralEquatable)packagesLocationTypes).GetHashCode(EqualityComparer<PackageLocationEnum>.Default);

            foreach (HashSet<uint> loads in vansLoads)
                resultHash ^= ((IStructuralEquatable)loads.ToArray()).GetHashCode(EqualityComparer<uint>.Default);

            foreach (HashSet<uint> loads in planesLoads)
                resultHash ^= ((IStructuralEquatable)loads.ToArray()).GetHashCode(EqualityComparer<uint>.Default);

            return resultHash;
        }
    }
}