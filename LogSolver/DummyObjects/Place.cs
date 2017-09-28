using System;
using System.Collections.Generic;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.DummyObjects
{
    public struct Place : IDummyObject<Place>
    {
        private readonly State state;
        public uint Identifier { get; }
        public bool IsAirport => State.AirportsIdentifiers.Contains(Identifier);

        public Place(uint identifier, State state)
        {
            this.state = state;
            this.Identifier = identifier;
        }

        public City City => new City(State.PlacesCitiesIdentifiers[(int)Identifier], state);

        public IEnumerable<Plane> Planes
        {
            get
            {
                for (int i = 0; i < state.PlanesLocationsIdentifiers.Count; i++)
                {
                    if (state.PlanesLocationsIdentifiers[i] == Identifier)
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
                    if (state.VansPlaceIdentifiers[(int)i] == Identifier)
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
                    if (state.PackagesLocationIdentifiers[(int)i] == Identifier)
                        yield return new Package(i, state);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Place && Equals((Place)obj);
        }

        public bool Equals(Place other) => Identifier == other.Identifier; 

        public override int GetHashCode() => (int)Identifier;

        public static bool operator ==(Place p1, Place p2) => p1.Equals(p2);

        public static bool operator !=(Place p1, Place p2) => !(p1 == p2);
        public override string ToString() => $"{nameof(Place)} {Identifier}|{City.Identifier}";

    }
}