using System.Collections.Generic;
using System.IO;
using System.Linq;
using LogSolver.HelperDummyObjects;

namespace LogSolver.DefaultStructuresImplementation
{
    public static class StreamReaderExtensions
    {
        public static string ReadLineSkippingComments(this StreamReader reader)
        {
            string line = null;
            do
            {
                line = reader.ReadLine();
            } while (line != null && line.StartsWith("%"));
            return line;
        }
    }

    public class Parser
    {
        public Parser(string inputFilePath)
        {
            InputFilePath = inputFilePath;
        }

        public string InputFilePath { get; }



        public State Parse()
        {
            using (var reader = new StreamReader(InputFilePath))
            {
                var citiesCount = uint.Parse(reader.ReadLineSkippingComments());
                var placesCount = uint.Parse(reader.ReadLineSkippingComments());

                var placesCitiesIdentifiers = new uint[placesCount];
                for (int i = 0; i < placesCount; i++)
                {
                    placesCitiesIdentifiers[i] = uint.Parse(reader.ReadLineSkippingComments());
                }


                var airportsIdentifiers = new uint[citiesCount];
                for (int i = 0; i < citiesCount; i++)
                {
                    airportsIdentifiers[i] = uint.Parse(reader.ReadLineSkippingComments());
                }


                var vansCount = uint.Parse(reader.ReadLineSkippingComments());
                var vansPlaceIdentifiers = new uint[vansCount];
                for (int i = 0; i < vansCount; i++)
                {
                    vansPlaceIdentifiers[i] = uint.Parse(reader.ReadLineSkippingComments());
                }


                var planesCount = uint.Parse(reader.ReadLineSkippingComments());
                var planesLocationsIdentifiers = new uint[planesCount];
                for (int i = 0; i < planesCount; i++)
                {
                    planesLocationsIdentifiers[i] = uint.Parse(reader.ReadLineSkippingComments());
                }


                var packagesCount = uint.Parse(reader.ReadLineSkippingComments());
                var packagesLocationIdentifiers = new uint[packagesCount];
                var packagesDestinatioIdentifiers = new uint[packagesCount];
                for (int i = 0; i < packagesCount; i++)
                {
                    var parsedLine = reader.ReadLineSkippingComments().Split(new[] { ' ' })
                        .Select(uint.Parse)
                        .ToArray();
                    packagesLocationIdentifiers[i] = parsedLine[0];
                    packagesDestinatioIdentifiers[i] = parsedLine[1];
                }

                return State.CreateInitState(placesCitiesIdentifiers,
                    vansPlaceIdentifiers,
                    planesLocationsIdentifiers,
                    packagesLocationIdentifiers,
                    airportsIdentifiers,
                    packagesDestinatioIdentifiers);
            }
        }
    }
}