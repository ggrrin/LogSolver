using System.Collections.Generic;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class AggregatedAction : IAction<State>
    {
        private readonly IList<IAction<State>> actions;
        public string Name => "Aggregate";
        public int ActionCost { get; }

        public AggregatedAction(IList<IAction<State>> actions )
        {
            this.actions = actions;
            ActionCost = actions.Sum(a => a.ActionCost);
        }

        public State PerformAction(State parentState)
        {
            foreach (var action in actions)
            {
                parentState = action.PerformAction(parentState);
            }

            return parentState;
        }
    }
}