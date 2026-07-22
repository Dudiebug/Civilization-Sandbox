using CivSandbox.Simulation;
using NUnit.Framework;

namespace CivSandbox.People.Tests
{
    public sealed class MovementAndClockTests
    {
        [Test]
        public void IdenticalWorldsReachIdenticalMovementCheckpoint()
        {
            var first = new WorldSimulation(170601UL);
            var second = new WorldSimulation(170601UL);

            for (int tick = 0; tick < 400; tick++)
            {
                first.AdvanceFixedWallTick(SimulationSpeed.Normal);
                second.AdvanceFixedWallTick(SimulationSpeed.Normal);
            }

            Assert.That(second.Time, Is.EqualTo(first.Time));
            Assert.That(second.ComputeChecksum(), Is.EqualTo(first.ComputeChecksum()));
        }

        [Test]
        public void LegalSpeedSchedulesMatchAtEqualWorldTime()
        {
            var normal = new WorldSimulation(9917UL);
            var veryFast = new WorldSimulation(9917UL);

            for (int tick = 0; tick < 100; tick++)
            {
                normal.AdvanceFixedWallTick(SimulationSpeed.Normal);
            }

            for (int tick = 0; tick < 10; tick++)
            {
                veryFast.AdvanceFixedWallTick(SimulationSpeed.VeryFast);
            }

            Assert.That(normal.Time.Seconds, Is.EqualTo(25L));
            Assert.That(veryFast.Time, Is.EqualTo(normal.Time));
            Assert.That(veryFast.ComputeChecksum(), Is.EqualTo(normal.ComputeChecksum()));
        }

        [Test]
        public void CalendarRunsFiveTimesRealTimeWithoutMultiplyingNormalActorWork()
        {
            var simulation = new WorldSimulation(9917UL);
            WorldSnapshot initial = simulation.CreateSnapshot();

            for (int tick = 0; tick < WorldSimulation.ActorFixedTicksPerSecond; tick++)
            {
                simulation.AdvanceFixedWallTick(SimulationSpeed.Normal);
            }

            WorldSnapshot later = simulation.CreateSnapshot();
            Assert.That(later.Time.Seconds, Is.EqualTo(5L));
            Assert.That(
                System.Math.Abs(later[0].Position.EastMillimeters - initial[0].Position.EastMillimeters),
                Is.LessThan(100),
                "A five-second calendar advance must not silently apply five seconds of ordinary walking.");
        }

        [Test]
        public void PauseDoesNotAdvanceAuthoritativeState()
        {
            var simulation = new WorldSimulation(83UL);
            ulong before = simulation.ComputeChecksum();

            for (int tick = 0; tick < 100; tick++)
            {
                simulation.AdvanceFixedWallTick(SimulationSpeed.Paused);
            }

            Assert.That(simulation.Time.Seconds, Is.Zero);
            Assert.That(simulation.ComputeChecksum(), Is.EqualTo(before));
        }

        [Test]
        public void PeopleRemainInsideBoundsDuringLongRun()
        {
            var simulation = new WorldSimulation(7123UL);
            for (int tick = 0; tick < 1000; tick++)
            {
                simulation.AdvanceFixedWallTick(SimulationSpeed.VeryFast);
            }

            WorldSnapshot snapshot = simulation.CreateSnapshot();
            for (int index = 0; index < snapshot.Count; index++)
            {
                Assert.That(snapshot.Bounds.Contains(snapshot[index].Position), Is.True, snapshot[index].Name);
            }
        }

        [Test]
        public void SnapshotReadsDoNotAffectStateAndOldSnapshotsStayDetached()
        {
            var observed = new WorldSimulation(501UL);
            var unobserved = new WorldSimulation(501UL);
            WorldSnapshot retained = observed.CreateSnapshot();
            var originalFirstPosition = retained[0].Position;

            for (int tick = 0; tick < 250; tick++)
            {
                observed.AdvanceFixedWallTick(SimulationSpeed.Double);
                observed.CreateSnapshot();
                unobserved.AdvanceFixedWallTick(SimulationSpeed.Double);
            }

            Assert.That(observed.ComputeChecksum(), Is.EqualTo(unobserved.ComputeChecksum()));
            Assert.That(retained.Time.Seconds, Is.Zero);
            Assert.That(retained[0].Position, Is.EqualTo(originalFirstPosition));
        }
    }
}
