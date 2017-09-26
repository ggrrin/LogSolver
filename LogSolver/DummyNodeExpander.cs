using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace LogSolver
{
    public class DummyNodeExpander : INodeExpander<State, Node<State>>
    {
        public INodeFactory<State, Node<State>> NodeFactory { get; }

        public DummyNodeExpander(INodeFactory<State, Node<State>> nodeFactory)
        {
            NodeFactory = nodeFactory;
        }

        public IEnumerable<Node<State>> ExpandNode(Node<State> nodeToExpand)
        {
            var state = nodeToExpand.State;
            //drive everywhere with vans
            foreach (var van in state.Vans)
                foreach (var place in van.Place.City.Places)
                    if (place != van.Place)
                        yield return NodeFactory.CreateNode(nodeToExpand, new DriveAction(van, place));

            //fly everywhere with planes
            foreach (var plane in state.Planes)
                foreach (var city in state.Cities)
                    if (city != plane.Place.City)
                        yield return NodeFactory.CreateNode(nodeToExpand, new FlyAction(city, plane));

            //try to do action with every package
            foreach (var package in state.Packages)
            {
                switch (package.LocationType)
                {
                    case PackageLocationEnum.Van:
                        yield return NodeFactory.CreateNode(nodeToExpand, new UnLoadAction(package));
                        break;

                    case PackageLocationEnum.Plane:
                        yield return NodeFactory.CreateNode(nodeToExpand, new DropOffAction(package));
                        break;

                    case PackageLocationEnum.Store:
                        foreach (var plane in package.Location.Planes.Where(p => !p.IsFull))
                            yield return NodeFactory.CreateNode(nodeToExpand, new PickUpAction(plane, package));

                        foreach (var van in package.Location.Vans.Where(v => !v.IsFull))
                            yield return NodeFactory.CreateNode(nodeToExpand, new LoadAction(package, van));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}