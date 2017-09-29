using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Actions;
using LogSolver.Architecture;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Expanders
{
    public class DummyNodeExpander<TNode> : INodeExpander<State, TNode> where TNode : INode<State,TNode>

    {
        public INodeFactory<State, TNode> NodeFactory { get; }

        public DummyNodeExpander(INodeFactory<State, TNode> nodeFactory)
        {
            NodeFactory = nodeFactory;
        }

        public virtual IEnumerable<TNode> ExpandNode(TNode nodeToExpand)
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