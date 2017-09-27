using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public abstract class ActionBase :IAction<State>
    {
        public string Name { get; }
        public int ActionCost { get; }

        protected ActionBase(string name, int actionCost)
        {
            Name = name;
            ActionCost = actionCost;
        }

        public abstract State PerformAction(State parentState);

        public override string ToString() => $"{Name}[{ActionCost}]:";
    }
}