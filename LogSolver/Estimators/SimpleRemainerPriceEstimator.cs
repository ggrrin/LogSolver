using System.Linq;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.Estimators
{
    public class SimpleRemainerPriceEstimator : IRemainerPriceEstimator<State>
    {
        public int CalculateEstimate(State state)
        {
            return state.Packages.Count(p => !p.IsInDestination) * 2;
        }
    }
}