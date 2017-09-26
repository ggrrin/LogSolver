using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.DefaultStructuresImplementation;

namespace LogSolver.HelperDummyObjects
{
    public struct City : IEquatable<City>
    {
        private readonly State state;
        public uint Identifier { get; }

        public City(uint identifier, State state)
        {
            this.state = state;
            this.Identifier = identifier;
        }

        public IEnumerable<Place> Places
        {
            get
            {
                for (uint i = 0; i < state.PlacesCitiesIdentifiers.Count; i++)
                {
                    if (state.PlacesCitiesIdentifiers[(int)i] == Identifier)
                        yield return new Place(i, state);
                }
            }
        }

        public IEnumerable<Van> Vans => Places.SelectMany(p => p.Vans);
        public IEnumerable<Plane> Planes => Places.SelectMany(p => p.Planes);
        public IEnumerable<Package> Packages => Places.SelectMany(p => p.Packages);

        public bool Equals(City other) => Identifier == other.Identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is City && Equals((City)obj);
        }

        public override int GetHashCode() => (int)Identifier;

        public static bool operator ==(City c1, City c2) => c1.Equals(c2);

        public static bool operator !=(City c1, City c2) => !(c1 == c2);

        public override string ToString() => $"{nameof(City)} {Identifier}";
    }
}