namespace CivSandbox.Simulation
{
    /// <summary>
    /// Owns authoritative Build 01 world time and fixed-wall-tick speed semantics.
    /// Wall-time accumulation remains outside the authoritative simulation.
    /// </summary>
    public sealed class SimulationClock
    {
        public const int FixedWallTicksPerSecond = 20;
        public const int CalendarSecondsPerRealSecond = 5;
        private long worldSeconds;
        private int calendarSubsecondTicks;

        public WorldTime Time => new WorldTime(worldSeconds);

        public int CalendarSubsecondTicks => calendarSubsecondTicks;

        public int AdvanceFixedWallTick(SimulationSpeed speed)
        {
            int previousWholeSeconds = calendarSubsecondTicks / FixedWallTicksPerSecond;
            calendarSubsecondTicks = checked(
                calendarSubsecondTicks + speed.Multiplier() * CalendarSecondsPerRealSecond);
            int elapsedGameSeconds = calendarSubsecondTicks / FixedWallTicksPerSecond - previousWholeSeconds;
            calendarSubsecondTicks %= FixedWallTicksPerSecond;
            worldSeconds = checked(worldSeconds + elapsedGameSeconds);
            return elapsedGameSeconds;
        }

        public void Reset()
        {
            worldSeconds = 0;
            calendarSubsecondTicks = 0;
        }
    }
}
