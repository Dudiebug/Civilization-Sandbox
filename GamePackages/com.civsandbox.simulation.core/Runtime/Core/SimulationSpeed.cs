namespace CivSandbox.Simulation
{
    public enum SimulationSpeed
    {
        Paused = 0,
        Normal = 1,
        Double = 2,
        Fast = 5,
        VeryFast = 10
    }

    public static class SimulationSpeedExtensions
    {
        public static int GameSecondsPerFixedTick(this SimulationSpeed speed)
        {
            switch (speed)
            {
                case SimulationSpeed.Paused:
                    return 0;
                case SimulationSpeed.Normal:
                    return 3;
                case SimulationSpeed.Double:
                    return 6;
                case SimulationSpeed.Fast:
                    return 15;
                case SimulationSpeed.VeryFast:
                    return 30;
                default:
                    return 0;
            }
        }

        public static string Label(this SimulationSpeed speed)
        {
            return speed == SimulationSpeed.Paused ? "PAUSED" : $"{(int)speed}x";
        }
    }
}
