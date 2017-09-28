using System;

namespace LogSolver.Architecture
{
    public interface IDummyObject<T> : IEquatable<T>
    {
        uint Identifier { get; }
    }
}