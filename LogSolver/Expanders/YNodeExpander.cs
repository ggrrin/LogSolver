using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Actions;
using LogSolver.Architecture;
using LogSolver.DummyObjects;
using LogSolver.Helpers;
using LogSolver.Structures;

namespace LogSolver.Expanders
{
    public class YNodeExpander<TNode> : INodeExpander<State, TNode> where TNode : INode<State>

    {
        public INodeFactory<State, TNode> NodeFactory { get; }

        public YNodeExpander(INodeFactory<State, TNode> nodeFactory)
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

            foreach (var node in LoadStorages(nodeToExpand))
                yield return node;
        }

        public IEnumerable<TNode> LoadStorages(TNode nodeToExpand)
        {
            var state = nodeToExpand.State;
            //unload drop off only if needed
            foreach (var package in state.Packages)
            {
                if (package.IsInDestination)
                {
                    switch (package.LocationType)
                    {
                        case PackageLocationEnum.Van:
                            if (package.Location == package.Destination)
                                yield return NodeFactory.CreateNode(nodeToExpand, new UnLoadAction(package));
                            break;
                    }
                }
                else
                {
                    switch (package.LocationType)//TODO nefunguje !!!
                    {
                        case PackageLocationEnum.Plane:
                            if (package.Location.City == package.Destination.City)
                                yield return NodeFactory.CreateNode(nodeToExpand, new DropOffAction(package));
                            break;
                    }
                }
            }

            //try to fully load every vehicle
            List<IAction<State>> loadActions = new List<IAction<State>>();
            TryLoadEveryVan(state, loadActions);
            TryLoadEveryPlane(state, loadActions);
            yield return NodeFactory.CreateNode(nodeToExpand, new AggregatedAction(loadActions));
        }

        private static void TryLoadEveryPlane(State state, List<IAction<State>> loadActions)
        {
            foreach (var airport in state.Airports.Where(p => p.Planes.Any(pl => !pl.IsFull)))
            {
                var queue = new Queue<Package>(airport.Packages
                    .Where(
                        p =>
                            !p.IsInDestination && p.Location.City != p.Destination.City &&
                            p.LocationType == PackageLocationEnum.Store));

                foreach (var plane in airport.Planes.Where(p => !p.IsFull))
                {
                    if (!queue.Any())
                        break;

                    var load = new List<Package>();
                    for (int i = 0; i < plane.FreeStorageCount; i++)
                    {
                        if (!queue.Any())
                            break;

                        load.Add(queue.Dequeue());
                    }

                    loadActions.Add(new BatchedPickUpAction(plane, load.ToArray()));
                }
            }
        }

        private static void TryLoadEveryVan(State state, List<IAction<State>> loadActions)
        {
            //try fully load every van
            foreach (var place in state.Places.Where(p => p.Vans.Any(v => !v.IsFull)))
            {
                var queue =
                    new Queue<Package>(
                        place.Packages.Where(p => !p.IsInDestination && p.LocationType == PackageLocationEnum.Store));
                foreach (var van in place.Vans.Where(v => !v.IsFull))
                {
                    if (!queue.Any())
                        break;

                    var load = new List<Package>();
                    for (int i = 0; i < van.FreeStorageCount; i++)
                    {
                        if (!queue.Any())
                            break;

                        load.Add(queue.Dequeue());
                    }

                    loadActions.Add(new BatchedLoadAction(load.ToArray(), van));
                }
            }
        }
    }
}