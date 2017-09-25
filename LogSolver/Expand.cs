using System;
using System.Collections.Generic;
using System.Linq;

namespace LogSolver
{
    public class Expand
    {
        public IEnumerable<Node> ExpandNode(Node nodeToExpand)
        {
            var state = nodeToExpand.State;
            //drive everywhere with vans
            foreach (var van in state.Vans)
            {
                foreach (var place in van.Place.City.Places)
                {
                    if (place != van.Place)
                    {
                        yield return new Node
                        {
                            Action = Action.Drive,
                            State = state.CloneChangingVanLocation(van, place),
                            Depth = nodeToExpand.Depth + 1,
                            PathPrice = nodeToExpand.PathPrice + 17
                        };
                    }
                }
            }

            //fly everywhere with planes
            foreach (var plane in state.Planes)
            {
                foreach (var city in state.Cities)
                {
                    if (city != plane.Place.City)
                    {
                        yield return new Node
                        {
                            Action = Action.Fly,
                            State = state.CloneChangingPlaneLocation(plane, city),
                            Depth = nodeToExpand.Depth + 1,
                            PathPrice = nodeToExpand.PathPrice + 1000
                        };
                    }

                }
            }


            //try to do action with every package
            foreach (var package in state.Packages)
            {
                switch (package.LocationType)
                {
                    case PackageLocationEnum.Van:
                        yield return new Node
                        {
                            Action = Action.Unload,
                            State = state.CloneChangingUnLoadPackage(package),
                            PathPrice = nodeToExpand.PathPrice + 2,
                            Depth = nodeToExpand.Depth + 1
                        };
                        break;

                    case PackageLocationEnum.Plane:
                        yield return new Node
                        {
                            Action = Action.DropOff,
                            State = state.CloneChangingDropOffPackage(package),
                            PathPrice = nodeToExpand.PathPrice + 11,
                            Depth = nodeToExpand.Depth + 1
                        };
                        break;

                    case PackageLocationEnum.Store:
                        foreach (var plane in package.Location.Planes.Where(p => !p.IsFull))
                        {
                            yield return new Node
                            {
                                Action = Action.PickUp,
                                State = state.CloneChangingPickUpPackage(package, plane),
                                PathPrice = nodeToExpand.PathPrice + 14,
                                Depth = nodeToExpand.Depth + 1
                            };
                        }

                        foreach (var van in package.Location.Vans.Where(v => !v.IsFull))
                        {
                            yield return new Node
                            {
                                Action = Action.Load,
                                State = state.CloneChangingLoadPackage(package, van),
                                PathPrice = nodeToExpand.PathPrice + 2,
                                Depth = nodeToExpand.Depth + 1
                            };
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

    }
}