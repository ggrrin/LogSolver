using LogSolver.Architecture;
using LogSolver.Structures;

namespace LogSolver.Actions
{
    public abstract class ActionBase :IAction<State>
    {
        private static uint IdCounter;
        public uint Id { get; }
        public string Name { get; }
        public int ActionCost { get; }

        protected ActionBase(string name, int actionCost)
        {
            Name = name;
            ActionCost = actionCost;
            Id = IdCounter++;
            if (Id == 346293266 || Id == 359592869)
            {
                

            }
        }

        public abstract State PerformAction(State parentState);

        public override string ToString() => $"{Name}[{ActionCost}]:";
        public abstract string Dump();
    }
}