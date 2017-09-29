using System;
using System.Collections.Generic;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.DummyObjects
{
    public struct Van : ITransportDummyObject<Van>
    {
        public uint Identifier { get; }
        private readonly State state;

        public HashSet<uint> Packages => state.VansLoads[(int)Identifier];
        public Place Place => new Place(state.VansPlaceIdentifiers[(int)Identifier], state);

        public int MaxLoad => 4;
        public bool IsFull
        {

            get
            {
                if(Packages.Count > MaxLoad )
                    throw new InvalidOperationException();
                return Packages.Count == MaxLoad;
            }
        }

        public bool IsEmpty => Packages.Count == 0;
        public bool IsPartlyLoaded => Packages.Count > 0 && Packages.Count < MaxLoad;
        public int FreeStorageCount => MaxLoad - Packages.Count;

        public Van(uint identifier, State state)
        {
            this.Identifier = identifier;
            this.state = state;
        }

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