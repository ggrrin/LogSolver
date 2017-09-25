using System;
using System.Collections.Generic;

namespace LogSolver
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

        public Place Place => new Place(state.PlacesCitiesIdentifiers[(int)Identifier], state);
        public bool IsFull => Packages.Count >= 4;

        public bool Equals(Van other) => Identifier == other.Identifier;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Van && Equals((Van)obj);
        }

        public override int GetHashCode() => (int)Identifier;

        public static bool operator ==(Van v1, Van v2) => v1.Equals(v2);

        public static bool operator !=(Van v1, Van v2) => !(v1 == v2);
    }
}