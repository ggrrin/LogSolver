namespace LogSolver
{
    public class InitAction : ActionBase 
    {
        public InitAction(Parser inputParser) : base("Init", 0)
        {
            InputParser = inputParser;
        }

        public Parser InputParser { get; }

        public override State PerformAction(State parentState)
        {
            return InputParser.Parse("TODO");
        }
    }
}