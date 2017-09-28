using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            //if there are no packages in city do not generate drive
            //do not genereate flights where they are not needed
            //do not drive with empty van if there is no package to take
            var state = nodeToExpand.State;

            foreach (var node in TryLoadUnLoadPackages(nodeToExpand))
                yield return node;

            foreach (var node in TryDriveVans(nodeToExpand))
                yield return node;


            foreach (var node in TryFlyPlanes(nodeToExpand))
                yield return node;
        }

        protected (List<T>, List<T>, List<T>) ExtractTransportByLoads<T>(IEnumerable<T> transports) where T : ITransportDummyObject<T>
        {
            var emptyTransport = new List<T>();
            var partlyLoadedTransport = new List<T>();
            var fullTransports = new List<T>();
            foreach (var transport in transports)
            {
                if (transport.IsEmpty)
                    emptyTransport.Add(transport);
                else if (transport.IsPartlyLoaded)
                    partlyLoadedTransport.Add(transport);
                else if (transport.IsFull)
                    fullTransports.Add(transport);
            }
            return (emptyTransport, partlyLoadedTransport, fullTransports);
        }


        protected virtual IEnumerable<TNode> TryFlyPlanes(TNode nodeToExpand)
        {
            var state = nodeToExpand.State;
            var (emptyPlanes, partlyLoadedPlanes, fullPlanes) = ExtractTransportByLoads(state.Planes);

            foreach (var plane in emptyPlanes)
                foreach (var city in GetCitiesWithPackagesOnTheAirport(plane))
                    yield return NodeFactory.CreateNode(nodeToExpand, new FlyAction(city, plane));

            foreach (var plane in partlyLoadedPlanes)
                foreach (var city in GetCitiesWithPackagesOnTheAirport(plane).Concat(GetTargetCitiesForPackages(plane)))
                    yield return NodeFactory.CreateNode(nodeToExpand, new FlyAction(city, plane));

            foreach (var plane in fullPlanes)
                foreach (var city in GetTargetCitiesForPackages(plane))
                    yield return NodeFactory.CreateNode(nodeToExpand, new FlyAction(city, plane));

            IEnumerable<City> GetCitiesWithPackagesOnTheAirport(Plane plane)
            {
                return state.Cities.Where(c => c.Airport.Packages.Any(
                    //packages which are waiting for transport to another city
                    p => !p.IsInDestinationCity))
                .Where(c => c != plane.Place.City);
            }

            IEnumerable<City> GetTargetCitiesForPackages(Plane plane)
            {
                return plane.Packages.Select(pId => new Package(pId, state).Destination.City)
                .Where(c => c != plane.Place.City);
            }
        }

        protected virtual IEnumerable<TNode> TryDriveVans(TNode nodeToExpand)
        {
            var state = nodeToExpand.State;
            var (emptyVans, partlyLoadedVans, fullVans) = ExtractTransportByLoads(state.Vans);

            //with each van try to go to places with packages
            foreach (var van in emptyVans)
                foreach (var place in GetPlacesWithPackagesInCity(van))
                    yield return NodeFactory.CreateNode(nodeToExpand, new DriveAction(van, place));

            //with each van try to go to places with packages or destination of packages
            foreach (var van in partlyLoadedVans)
                foreach (var place in GetPlacesWithPackagesInCity(van).Concat(GetTargetPlacesForPackages(van)))
                    yield return NodeFactory.CreateNode(nodeToExpand, new DriveAction(van, place));

            //with each van try to go to each destination for package
            foreach (var van in fullVans)
                foreach (var destination in GetTargetPlacesForPackages(van))
                    yield return NodeFactory.CreateNode(nodeToExpand, new DriveAction(van, destination));

            IEnumerable<Place> GetPlacesWithPackagesInCity(Van van)
            {
                return van.Place.City.Places.Where(place => place != van.Place &&
                place.Packages.Any(
                    //packages which are not delivered and which are not waiting in the airport for transport
                    p => !p.IsInDestination && (p.IsInDestinationCity || !State.AirportsIdentifiers.Contains(p.Location.Identifier))));
            }

            IEnumerable<Place> GetTargetPlacesForPackages(Van van)
            {
                return van.Packages.Select(pId =>
                {
                    var package = new Package(pId, state);
                    return package.IsInDestinationCity ? package.Destination : package.Location.City.Airport;
                })
                .Where(p => p != van.Place);
            }
        }

        protected virtual IEnumerable<TNode> TryLoadUnLoadPackages(TNode nodeToExpand)
        {
            var loadActions = new List<IAction<State>>();
            var state = nodeToExpand.State;
            TryUnloadPlane(state, loadActions);
            TryUnloadVan(state, loadActions);
            TryLoadEveryVan(state, loadActions);
            TryLoadEveryPlane(state, loadActions);

            if (loadActions.Any())
                if (loadActions.Count == 1)
                    yield return NodeFactory.CreateNode(nodeToExpand, loadActions[0]);
                else
                    yield return NodeFactory.CreateNode(nodeToExpand, new AggregatedAction(loadActions));
        }

        protected virtual void TryUnloadVan(State state, List<IAction<State>> loadActions)
        {
            foreach (var van in state.Vans.Where(v => v.Packages.Any()))
            {
                var packages = van.Packages.Select(pId => new Package(pId, state))
                    .Where(p => p.IsInDestination || (!p.IsInDestinationCity && van.Place.IsAirport))
                    .ToArray();

                if (packages.Any())
                    if (packages.Length == 1)
                        loadActions.Add(new UnLoadAction(packages[0]));
                    else
                        loadActions.Add(new BatchedUnLoadAction(packages));
            }
        }

        protected virtual void TryUnloadPlane(State state, List<IAction<State>> loadActions)
        {
            foreach (var plane in state.Planes.Where(p => p.Packages.Any()))
            {
                var packages = plane.Packages.Select(pId => new Package(pId, state))
                    .Where(p => p.IsInDestinationCity)
                    .ToArray();
                if (packages.Any())
                    if (packages.Length == 1)
                        loadActions.Add(new DropOffAction(packages[0]));
                    else
                        loadActions.Add(new BatchedDropOffAction(packages));
            }
        }

        protected virtual void TryLoadEveryPlane(State state, List<IAction<State>> loadActions)
        {
            //try fully load every plane
            foreach (var airport in state.Airports.Where(p => p.Planes.Any(pl => !pl.IsFull)))
            {
                var queue = new Queue<Package>(airport.Packages
                    .Where(p => !p.IsInDestination && p.LocationType == PackageLocationEnum.Store &&
                           p.Location.City != p.Destination.City));

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

                    if (load.Any())
                        if (load.Count == 1)
                            loadActions.Add(new PickUpAction(plane, load[0]));
                        else
                            loadActions.Add(new BatchedPickUpAction(plane, load.ToArray()));
                }
            }
        }

        protected virtual void TryLoadEveryVan(State state, List<IAction<State>> loadActions)
        {
            //try fully load every van
            foreach (var place in state.Places.Where(p => p.Vans.Any(v => !v.IsFull)))
            {
                var queue =
                    new Queue<Package>(place.Packages.Where(
                            p => !p.IsInDestination && p.LocationType == PackageLocationEnum.Store
                            //do not drive out packages that are correctly on the airport
                            && (p.Location.City == p.Destination.City || !p.Location.IsAirport)
                            ));
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

                    if (load.Any())
                        if (load.Count == 1)
                            loadActions.Add(new LoadAction(load[0], van));
                        else
                            loadActions.Add(new BatchedLoadAction(load.ToArray(), van));
                }
            }
        }
    }
}