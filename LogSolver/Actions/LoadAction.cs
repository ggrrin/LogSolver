using System;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class LoadAction : ActionBase
    {
        private readonly Package package;
        private readonly Van van;

        public LoadAction(Package package, Van van) : base("Load", 2)
        {
            if(van.IsFull || package.LocationType != PackageLocationEnum.Store)
                throw new ArgumentException();

            this.package = package;
            this.van = van;
        }

        public LoadAction() : base(String.Empty, 2)
        {
            

        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingLoadPackage(package, van);
        }


        public override string ToString() => $"{base.ToString()} {package} to {van}";
        public override string Dump()
        {
            return $"load {van.Identifier} {package.Identifier}";
        }
    }
}