using System;
using System.Linq;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class UnLoadAction : ActionBase
    {
        private readonly Package package;
        private readonly Van van;

        public UnLoadAction(Package package, Van van) : base("UnLoad", 2)
        {
            if(van.IsEmpty || package.LocationType != PackageLocationEnum.Van || !van.Packages.Contains(package.Identifier))
                throw new ArgumentException();
            this.package = package;
            this.van = van;
        }

        public UnLoadAction() : base(String.Empty, 2)
        {

        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingUnLoadPackage(package, van);
        }

        public override string ToString() => $"{base.ToString()} {package}";
        public override string Dump()
        {
            return $"unload {van.Identifier} {package.Identifier}";
        }
    }
}