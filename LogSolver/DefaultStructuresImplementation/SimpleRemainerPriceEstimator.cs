using System.Linq;
using System.Runtime.ExceptionServices;

namespace LogSolver.DefaultStructuresImplementation
{
    public class SimpleRemainerPriceEstimator : IRemainerPriceEstimator<State>
    {
        public int CalculateEstimate(State state)
        {
            return state.Packages.Count(p => !p.IsInDestination) * 2;
        }
    }
}