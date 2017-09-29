using System;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.Estimators
{
    public class CombinePriceEstimator : IRemainerPriceEstimator<State>
    {
        private readonly IRemainerPriceEstimator<State>[] estimators;
        private static readonly Random rnd = new Random(3423);

        public CombinePriceEstimator(params IRemainerPriceEstimator<State>[] estimators)
        {
            this.estimators = estimators;
        }
        public int CalculateEstimate(State state)
        {
            var estimates = estimators.Select(e => e.CalculateEstimate(state)).ToArray();
            var min = estimates.Min();
            var sum = estimates.Sum();
            var avg = (int)estimates.Average();

            return avg + rnd.Next(Math.Abs(sum - avg)) - rnd.Next(Math.Abs(avg - min));
        }
    }
}