using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LogSolver
{
    public struct Van : IEquatable<Van>
    {
        private readonly uint identifier;
        private readonly State state;

        public Van(uint identifier, State state)
        {
            this.identifier = identifier;
            this.state = state;
        }

        public Place Place => new Place(state.PlacesCitiesIdentifiers[(int)identifier], state);

        public bool Equals(Van other) => identifier == other.identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Van && Equals((Van)obj);
        }

        public override int GetHashCode() => (int)identifier;

        public static bool operator ==(Van v1, Van v2) => v1.Equals(v2);

        public static bool operator !=(Van v1, Van v2) => !(v1 == v2);
    }

    public struct City : IEquatable<City>
    {
        private readonly State state;
        private readonly uint identifier;

        public City(uint identifier, State state)
        {
            this.state = state;
            this.identifier = identifier;
        }

        public IEnumerable<Place> Places
        {
            get
            {
                for (uint i = 0; i < state.PlacesCitiesIdentifiers.Count; i++)
                {
                    if (state.PlacesCitiesIdentifiers[(int)i] == identifier)
                        yield return new Place(i, state);
                }
            }
        }

        public IEnumerable<Van> Vans => Places.SelectMany(p => p.Vans);
        public IEnumerable<Plane> Planes => Places.SelectMany(p => p.Planes);
        public IEnumerable<Package> Packages => Places.SelectMany(p => p.Packages);

        public bool Equals(City other) => identifier == other.identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is City && Equals((City)obj);
        }

        public override int GetHashCode() => (int)identifier;

        public static bool operator ==(City c1, City c2) => c1.Equals(c2);

        public static bool operator !=(City c1, City c2) => !(c1 == c2);
    }

    public struct Plane : IEquatable<Plane>
    {
        private readonly uint identifier;
        private readonly State state;

        public Plane(uint identifier, State state)
        {
            this.identifier = identifier;
            this.state = state;
        }

        public Place Place => new Place(state.PlanesLocationsIdentifiers[(int)identifier], state);

        public bool Equals(Plane other) => identifier == other.identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Plane && Equals((Plane)obj);
        }

        public override int GetHashCode() => (int)identifier;

        public static bool operator ==(Plane p1, Plane p2) => p1.Equals(p2);

        public static bool operator !=(Plane p1, Plane p2) => !(p1 == p2);
    }

    public struct Place : IEquatable<Place>
    {
        private readonly State state;
        private readonly uint identifier;

        public Place(uint identifier, State state)
        {
            this.state = state;
            this.identifier = identifier;
        }

        public City City => new City(state.PlacesCitiesIdentifiers[(int)identifier], state);

        public IEnumerable<Plane> Planes
        {
            get
            {
                for (int i = 0; i < state.PlanesLocationsIdentifiers.Count; i++)
                {
                    if (state.PlanesLocationsIdentifiers[i] == identifier)
                        yield return new Plane((uint)i, state);
                }
            }
        }

        public IEnumerable<Van> Vans
        {
            get
            {
                for (uint i = 0; i < state.VansPlaceIdentifiers.Count; i++)
                {
                    if (state.VansPlaceIdentifiers[(int)i] == identifier)
                        yield return new Van(i, state);
                }
            }
        }

        public IEnumerable<Package> Packages
        {
            get
            {
                for (uint i = 0; i < state.PackagesLocationIdentifiers.Count; i++)
                {
                    if (state.PackagesLocationIdentifiers[(int)i] == identifier)
                        yield return new Package(i, state);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Place && Equals((Place)obj);
        }

        public bool Equals(Place other) => identifier == other.identifier;

        public override int GetHashCode() => (int)identifier;

        public static bool operator ==(Place p1, Place p2) => p1.Equals(p2);

        public static bool operator !=(Place p1, Place p2) => !(p1 == p2);
    }

    public struct Package : IEquatable<Package>
    {
        private readonly uint identifier;
        private readonly State state;

        public Place Destination => new Place(state.PackagesDestinatioIdentifiers[(int)identifier], state);
        public Place Location => new Place(state.PackagesLocationIdentifiers[(int)identifier], state);

        public Package(uint identifier, State state)
        {
            this.state = state;
            this.identifier = identifier;
        }

        public bool Equals(Package other) => identifier == other.identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Package && Equals((Package)obj);
        }

        public override int GetHashCode() => (int)identifier;

        public static bool operator ==(Package p1, Package p2) => p1.Equals(p2);

        public static bool operator !=(Package p1, Package p2) => !(p1 == p2);
    }

    public class State
    {
        public IReadOnlyList<uint> PlacesCitiesIdentifiers { get; }
        public IReadOnlyList<uint> AirportsIdentifiers { get; }
        public IReadOnlyList<uint> VansPlaceIdentifiers { get; }
        public IReadOnlyList<uint> PlanesLocationsIdentifiers { get; }
        public IReadOnlyList<uint> PackagesLocationIdentifiers { get; }
        public IReadOnlyList<uint> PackagesDestinatioIdentifiers { get; }

        public IReadOnlyList<uint> TransportIdentifier

        public State(uint[] placesCitiesIdentifiers, uint[] airportsIdentifiers, uint[] vansPlaceIdentifiers, uint[] planesLocationsIdentifiers, uint[] packagesLocationIdentifiers, uint[] packagesDestinatioIdentifiers)
        {
            PlacesCitiesIdentifiers = placesCitiesIdentifiers;
            AirportsIdentifiers = airportsIdentifiers;
            VansPlaceIdentifiers = vansPlaceIdentifiers;
            PlanesLocationsIdentifiers = planesLocationsIdentifiers;
            PackagesLocationIdentifiers = packagesLocationIdentifiers;
            PackagesDestinatioIdentifiers = packagesDestinatioIdentifiers;
        }


        public IEnumerable<Van> Vans => VansPlaceIdentifiers.Select((i, v) => new Van(i, this));
        public IEnumerable<City> Cities => AirportsIdentifiers.Select((i, c) => new City(i, this));
        public IEnumerable<Plane> Planes => PlanesLocationsIdentifiers.Select((i, p) => new Plane(i, this));
        public IEnumerable<Place> Places => PlacesCitiesIdentifiers.Select((i, c) => new Place(i, this));
        public IEnumerable<Package> Packages => PackagesLocationIdentifiers.Select((i, p) => new Package(i, this));

    }

    public interface IAction
    {
    }

    public enum Action
    {
        Drive,
        Load,
        Unload,
        Fly,
        PickUp,
        DropOff,
    }

    public class Node
    {
        public State State { get; set; }
        public Action Action { get; set; }
        public int PathPrice { get; set; }
        public uint Depth { get; set; }

    }

    public class Expand
    {
        public IEnumerable<Node> Expand(Node nodeToExpand)
        {
            var state = nodeToExpand.State;
            //drive everywhere with vans
            foreach (var van in state.Vans)
            {
                foreach (var place in van.Place.City.Places)
                {
                    if (place != van.Place)
                    {
                        yield return new Node
                        {
                            State =


                        };
                    }

                }
            }

        }

    }


    public class Parser
    {
        public State Parse(string inputFilePath)
        {
            using (var reader = new StreamReader(inputFilePath))
            {
                var citiesCount = uint.Parse(reader.ReadLine());
                var placesCount = uint.Parse(reader.ReadLine());

                var placesCitiesIdentifiers = new uint[placesCount];
                for (int i = 0; i < placesCount; i++)
                {
                    placesCitiesIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var airportsIdentifiers = new uint[citiesCount];
                for (int i = 0; i < citiesCount; i++)
                {
                    airportsIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var vansCount = uint.Parse(reader.ReadLine());
                var vansPlaceIdentifiers = new uint[vansCount];
                for (int i = 0; i < vansCount; i++)
                {
                    vansPlaceIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var planesCount = uint.Parse(reader.ReadLine());
                var planesLocationsIdentifiers = new uint[planesCount];
                for (int i = 0; i < planesCount; i++)
                {
                    planesLocationsIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var packagesCount = uint.Parse(reader.ReadLine());
                var packagesLocationIdentifiers = new uint[packagesCount];
                var packagesDestinatioIdentifiers = new uint[packagesCount];
                for (int i = 0; i < packagesCount; i++)
                {
                    var parsedLine = reader.ReadLine().Split(new[] { ' ' })
                        .Select(uint.Parse)
                        .ToArray();
                    packagesLocationIdentifiers[i] = parsedLine[0];
                    packagesDestinatioIdentifiers[i] = parsedLine[1];
                }

                return new State(placesCitiesIdentifiers,
                    airportsIdentifiers,
                    vansPlaceIdentifiers,
                    planesLocationsIdentifiers,
                    packagesLocationIdentifiers,
                    packagesDestinatioIdentifiers);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
