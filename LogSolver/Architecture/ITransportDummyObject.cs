using System.Collections.Generic;
using LogSolver.DummyObjects;

namespace LogSolver.Architecture
{
    public interface ITransportDummyObject<T> : IDummyObject<T>
    {
        int MaxLoad { get; }
        bool IsEmpty { get; }
        bool IsPartlyLoaded { get; }
        bool IsFull { get; }
        int FreeStorageCount { get; }
        Place Place { get; }
        HashSet<uint> Packages { get; }
    }
}