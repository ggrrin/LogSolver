using System.Linq;

namespace LogSolver
{
    public class Node
    {
        public State State { get; set; }
        public Action Action { get; set; }
        public int PathPrice { get; set; }
        public uint Depth { get; set; }

        public bool IsGoalState()
        {
            var res = State.Packages.FirstOrDefault(p => !p.IsInDestination);
            return res == default(Package);
        }

    }
}