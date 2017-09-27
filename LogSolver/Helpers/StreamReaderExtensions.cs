using System.IO;

namespace LogSolver.Helpers
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
}