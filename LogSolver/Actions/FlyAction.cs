using System;
using LogSolver.DummyObjects;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class FlyAction: ActionBase
    {
        private readonly City city;
        private readonly Plane plane;

        public FlyAction(City city, Plane plane) : base("Fly", 1000)
        {
            if(plane.Place.City == city)
                throw new ArgumentException();
            this.city = city;
            this.plane = plane;
        }

        public FlyAction() : base(String.Empty, 1000)
        {
            

        }

        public override State PerformAction(State parentState)
        {
            return parentState.CloneChangingPlaneLocation(plane, city);
        }

        public override string ToString() => $"{base.ToString()} {plane} to {city} with [{string.Join(" ,", plane.Packages)}]";
        public override string Dump()
        {
            return $"fly {plane.Identifier} {city.Airport.Identifier}";
        }
    }
}