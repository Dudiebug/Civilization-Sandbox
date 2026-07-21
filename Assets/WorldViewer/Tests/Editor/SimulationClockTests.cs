using CivSandbox.Simulation;
using NUnit.Framework;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class SimulationClockTests
    {
        [Test]
        public void ClockOwnsFixedRateAndSpeedStepSemantics()
        {
            var clock = new SimulationClock();

            Assert.That(SimulationClock.FixedWallTicksPerSecond, Is.EqualTo(20));
            Assert.That(clock.AdvanceFixedWallTick(SimulationSpeed.Paused), Is.Zero);
            Assert.That(clock.AdvanceFixedWallTick(SimulationSpeed.Normal), Is.EqualTo(3));
            Assert.That(clock.AdvanceFixedWallTick(SimulationSpeed.Double), Is.EqualTo(6));
            Assert.That(clock.AdvanceFixedWallTick(SimulationSpeed.Fast), Is.EqualTo(15));
            Assert.That(clock.AdvanceFixedWallTick(SimulationSpeed.VeryFast), Is.EqualTo(30));
            Assert.That(clock.Time, Is.EqualTo(new WorldTime(54)));
        }

        [Test]
        public void ResetReturnsAuthoritativeTimeToEpoch()
        {
            var clock = new SimulationClock();
            clock.AdvanceFixedWallTick(SimulationSpeed.VeryFast);

            clock.Reset();

            Assert.That(clock.Time, Is.EqualTo(new WorldTime(0)));
        }
    }
}
