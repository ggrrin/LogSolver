using System;
using System.Collections.Generic;
using System.Linq;
using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public class AggregatedAction : ActionBase 
    {
        private readonly IList<IAction<State>> actions;

        public AggregatedAction(IList<IAction<State>> actions) : base("Aggregate",actions.Sum(a => a.ActionCost))
        {
            this.actions = actions;
        }

        public override State PerformAction(State parentState)
        {
            foreach (var action in actions)
            {
                parentState = action.PerformAction(parentState);
            }

            return parentState;
        }

        public override string ToString()
        {
            return $"{Name}[{ActionCost}]: [{string.Join(Environment.NewLine + '\t', actions)}]";
        }

        public override string Dump() => string.Join(Environment.NewLine, actions.Select(a => a.Dump()));
    }
}