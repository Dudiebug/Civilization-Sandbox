using System.Collections.Generic;
using NUnit.Framework;

namespace CivSandbox.People.Tests
{
    public sealed class DeterministicResetTests
    {
        [Test]
        public void SameSeedResetRecreatesIdenticalInitialWorld()
        {
            var simulation = new WorldSimulation(170601UL);
            ulong initial = simulation.ComputeChecksum();
            WorldSnapshot first = simulation.CreateSnapshot();

            simulation.Reset(999UL);
            simulation.Reset(170601UL);

            Assert.That(simulation.ComputeChecksum(), Is.EqualTo(initial));
            WorldSnapshot reset = simulation.CreateSnapshot();
            Assert.That(reset.Count, Is.EqualTo(WorldSimulation.PersonCount));
            for (int index = 0; index < first.Count; index++)
            {
                Assert.That(reset[index].Id, Is.EqualTo(first[index].Id));
                Assert.That(reset[index].Name, Is.EqualTo(first[index].Name));
                Assert.That(reset[index].Position, Is.EqualTo(first[index].Position));
                Assert.That(reset[index].Action, Is.EqualTo(first[index].Action));
                Assert.That(reset[index].ActionReason, Is.EqualTo(first[index].ActionReason));
                Assert.That(reset[index].Needs.NutritionUnits, Is.EqualTo(first[index].Needs.NutritionUnits));
                Assert.That(reset[index].Needs.HydrationUnits, Is.EqualTo(first[index].Needs.HydrationUnits));
            }
        }

        [Test]
        public void SeededPeopleHaveUniqueStableIdsAndBoundedPositions()
        {
            var simulation = new WorldSimulation(42UL);
            WorldSnapshot snapshot = simulation.CreateSnapshot();
            var ids = new HashSet<Simulation.StableEntityId>();

            Assert.That(snapshot.Count, Is.InRange(20, 30));
            for (int index = 0; index < snapshot.Count; index++)
            {
                Assert.That(ids.Add(snapshot[index].Id), Is.True);
                Assert.That(snapshot.Bounds.Contains(snapshot[index].Position), Is.True);
                Assert.That(snapshot[index].Name, Is.Not.Empty);
                Assert.That(snapshot[index].Needs.NutritionUrgency, Is.EqualTo(NeedUrgency.Safe));
                Assert.That(snapshot[index].Needs.HydrationUrgency, Is.EqualTo(NeedUrgency.Safe));
            }

            var otherSeed = new WorldSimulation(43UL);
            Assert.That(otherSeed.ComputeChecksum(), Is.Not.EqualTo(simulation.ComputeChecksum()));
        }
    }
}
