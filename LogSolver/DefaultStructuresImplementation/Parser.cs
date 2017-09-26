using System.Collections.Generic;
using System.IO;
using System.Linq;
using LogSolver.HelperDummyObjects;

namespace LogSolver.DefaultStructuresImplementation
{
    public class Parser
    {
        public State Parse(string inputFilePath)
        {
            using (var reader = new StreamReader(inputFilePath))
            {
                var citiesCount = uint.Parse(reader.ReadLine());
                var placesCount = uint.Parse(reader.ReadLine());

                var placesCitiesIdentifiers = new uint[placesCount];
                for (int i = 0; i < placesCount; i++)
                {
                    placesCitiesIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var airportsIdentifiers = new uint[citiesCount];
                for (int i = 0; i < citiesCount; i++)
                {
                    airportsIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var vansCount = uint.Parse(reader.ReadLine());
                var vansPlaceIdentifiers = new uint[vansCount];
                for (int i = 0; i < vansCount; i++)
                {
                    vansPlaceIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var planesCount = uint.Parse(reader.ReadLine());
                var planesLocationsIdentifiers = new uint[planesCount];
                for (int i = 0; i < planesCount; i++)
                {
                    planesLocationsIdentifiers[i] = uint.Parse(reader.ReadLine());
                }


                var packagesCount = uint.Parse(reader.ReadLine());
                var packagesLocationIdentifiers = new uint[packagesCount];
                var packagesDestinatioIdentifiers = new uint[packagesCount];
                for (int i = 0; i < packagesCount; i++)
                {
                    var parsedLine = reader.ReadLine().Split(new[] { ' ' })
                        .Select(uint.Parse)
                        .ToArray();
                    packagesLocationIdentifiers[i] = parsedLine[0];
                    packagesDestinatioIdentifiers[i] = parsedLine[1];
                }

                return new State(placesCitiesIdentifiers,
                    airportsIdentifiers,
                    vansPlaceIdentifiers,
                    planesLocationsIdentifiers,
                    packagesLocationIdentifiers,
                    packagesDestinatioIdentifiers,
                    Enumerable.Repeat(PackageLocationEnum.Store, (int)packagesCount).ToArray(),
                    Enumerable.Repeat(new HashSet<uint>(), (int)vansCount).ToArray(),
                    Enumerable.Repeat(new HashSet<uint>(), (int)planesCount).ToArray());
            }
        }
    }
}