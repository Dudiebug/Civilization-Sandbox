using CivSandbox.Simulation;
using NUnit.Framework;

namespace CivSandbox.People.Tests
{
    public sealed class SurvivalNeedsTests
    {
        [Test]
        public void NeedsDeclineDeterministicallyWithGameTime()
        {
            var first = new WorldSimulation(170601UL);
            var second = new WorldSimulation(170601UL);
            WorldSnapshot initial = first.CreateSnapshot();

            for (int tick = 0; tick < 1200; tick++)
            {
                first.AdvanceFixedWallTick(SimulationSpeed.VeryFast);
                second.AdvanceFixedWallTick(SimulationSpeed.VeryFast);
            }

            WorldSnapshot later = first.CreateSnapshot();
            Assert.That(later[0].Needs.NutritionUnits, Is.LessThan(initial[0].Needs.NutritionUnits));
            Assert.That(later[0].Needs.HydrationUnits, Is.LessThan(initial[0].Needs.HydrationUnits));
            Assert.That(second.ComputeChecksum(), Is.EqualTo(first.ComputeChecksum()));
        }

        [Test]
        public void NeedsClampAtZeroAndBecomeCritical()
        {
            var simulation = new WorldSimulation(170601UL);
            for (int tick = 0; tick < 24000; tick++)
            {
                simulation.AdvanceFixedWallTick(SimulationSpeed.VeryFast);
            }

            WorldSnapshot snapshot = simulation.CreateSnapshot();
            for (int index = 0; index < snapshot.Count; index++)
            {
                Assert.That(snapshot[index].Needs.NutritionUnits, Is.EqualTo(0));
                Assert.That(snapshot[index].Needs.HydrationUnits, Is.EqualTo(0));
                Assert.That(snapshot[index].Needs.HighestUrgency, Is.EqualTo(NeedUrgency.Critical));
            }
        }
    }
}
