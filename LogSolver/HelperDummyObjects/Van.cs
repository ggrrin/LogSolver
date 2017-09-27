using System;
using System.Collections.Generic;
using LogSolver.DefaultStructuresImplementation;

namespace LogSolver.HelperDummyObjects
{
    public struct Van : IEquatable<Van>
    {
        public uint Identifier { get; }
        private readonly State state;

        public HashSet<uint> Packages => state.VansLoads[(int)Identifier];

        public Van(uint identifier, State state)
        {
            this.Identifier = identifier;
            this.state = state;
        }

        public const int maxVanLoad = 4;
        public Place Place => new Place(state.VansPlaceIdentifiers[(int)Identifier], state);
        public bool IsFull => Packages.Count >= maxVanLoad;
        public int FreeStorageCount => maxVanLoad - Packages.Count; 

        public bool Equals(Van other) => Identifier == other.Identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Van && Equals((Van)obj);
        }

        public override int GetHashCode() => (int)Identifier;

        public static bool operator ==(Van v1, Van v2) => v1.Equals(v2);

        public static bool operator !=(Van v1, Van v2) => !(v1 == v2);

        public override string ToString() => $"{nameof(Van)} {Identifier} at {Place}";
    }
}