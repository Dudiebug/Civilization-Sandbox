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
        public static int Multiplier(this SimulationSpeed speed)
        {
            int value = (int)speed;
            return value < 0 ? 0 : value;
        }

        public static string Label(this SimulationSpeed speed)
        {
            return speed == SimulationSpeed.Paused ? "PAUSED" : $"{(int)speed}x";
        }
    }
}
