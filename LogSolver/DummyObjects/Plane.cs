using System;
using System.Collections.Generic;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.DummyObjects
{
    public struct Plane : ITransportDummyObject<Plane>
    {
        public uint Identifier { get; }
        private readonly State state;


        public Place Place => new Place(state.PlanesLocationsIdentifiers[(int)Identifier], state);
        public HashSet<uint> Packages => state.PlanesLoads[(int)Identifier];
        public bool IsFull
        {
            get
            {
                if (Packages.Count > MaxLoad)
                    throw new InvalidOperationException();
                return Packages.Count == MaxLoad;
            }
        }

        public int FreeStorageCount => MaxLoad - Packages.Count;
        public int MaxLoad => 30;
        public bool IsEmpty => Packages.Count == 0;
        public bool IsPartlyLoaded => Packages.Count > 0 && Packages.Count < MaxLoad;

        public Plane(uint identifier, State state)
        {
            this.Identifier = identifier;
            this.state = state;
        }

        public bool Equals(Plane other) => Identifier == other.Identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Plane && Equals((Plane)obj);
        }

        public override int GetHashCode() => (int)Identifier;

        public static bool operator ==(Plane p1, Plane p2) => p1.Equals(p2);

        public static bool operator !=(Plane p1, Plane p2) => !(p1 == p2);
        public override string ToString() => $"{nameof(Plane)} {Identifier} at {Place}";
    }
}