namespace CivSandbox.Simulation
{
    /// <summary>
    /// Owns authoritative Build 01 world time and fixed-wall-tick speed semantics.
    /// Wall-time accumulation remains outside the authoritative simulation.
    /// </summary>
    public sealed class SimulationClock
    {
        public const int FixedWallTicksPerSecond = 20;
        private long worldSeconds;

        public WorldTime Time => new WorldTime(worldSeconds);

        public int AdvanceFixedWallTick(SimulationSpeed speed)
        {
            int elapsedGameSeconds = speed.GameSecondsPerFixedTick();
            worldSeconds = checked(worldSeconds + elapsedGameSeconds);
            return elapsedGameSeconds;
        }

        public void Reset()
        {
            worldSeconds = 0;
        }
    }
}
