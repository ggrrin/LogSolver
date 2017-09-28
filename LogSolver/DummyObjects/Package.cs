using System;
using LogSolver.Architecture;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.DummyObjects
{
    public struct Package : IDummyObject<Package>
    {
        private readonly State state;
        public uint Identifier { get; }

        public PackageLocationEnum LocationType => state.PackagesLocationTypes[(int) Identifier];


        public Place Destination => new Place(State.PackagesDestinatioIdentifiers[(int)Identifier], state);
        public Place Location => new Place(state.PackagesLocationIdentifiers[(int)Identifier], state);
        public bool IsInDestination => Destination == Location;
        public bool IsInDestinationCity => Destination.City == Location.City;
        public bool IsInDestinationStore => Destination == Location && LocationType == PackageLocationEnum.Store;

        public Package(uint identifier, State state)
        {
            this.state = state;
            this.Identifier = identifier;
        }

        public bool Equals(Package other) => Identifier == other.Identifier; 

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Package && Equals((Package)obj);
        }

        public override int GetHashCode() => (int)Identifier;

        public static bool operator ==(Package p1, Package p2) => p1.Equals(p2);

        public static bool operator !=(Package p1, Package p2) => !(p1 == p2);
        public override string ToString() => $"{nameof(Package)} {Identifier} at {Location}";
    }
}