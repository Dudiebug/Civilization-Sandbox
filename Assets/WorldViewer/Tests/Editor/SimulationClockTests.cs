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
            Assert.That(SimulationClock.CalendarSecondsPerRealSecond, Is.EqualTo(5));
            for (int tick = 0; tick < 20; tick++)
            {
                clock.AdvanceFixedWallTick(SimulationSpeed.Normal);
            }

            Assert.That(clock.Time, Is.EqualTo(new WorldTime(5)),
                "One real second at Normal must advance the calendar exactly five seconds.");

            for (int tick = 0; tick < 20; tick++)
            {
                clock.AdvanceFixedWallTick(SimulationSpeed.Double);
            }

            Assert.That(clock.Time, Is.EqualTo(new WorldTime(15)));
            Assert.That(clock.AdvanceFixedWallTick(SimulationSpeed.Paused), Is.Zero);
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
