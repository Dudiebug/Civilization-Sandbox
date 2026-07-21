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
                Assert.That(reset[index].AppearanceVariant, Is.EqualTo(first[index].AppearanceVariant));
                Assert.That(reset[index].Clothing.Upper, Is.EqualTo(first[index].Clothing.Upper));
                Assert.That(reset[index].Clothing.Lower, Is.EqualTo(first[index].Clothing.Lower));
                Assert.That(reset[index].Clothing.Outer, Is.EqualTo(first[index].Clothing.Outer));
                Assert.That(reset[index].Clothing.Headwear, Is.EqualTo(first[index].Clothing.Headwear));
                Assert.That(reset[index].Clothing.Footwear, Is.EqualTo(first[index].Clothing.Footwear));
                Assert.That(reset[index].Clothing.UpperColor, Is.EqualTo(first[index].Clothing.UpperColor));
                Assert.That(reset[index].Clothing.LowerColor, Is.EqualTo(first[index].Clothing.LowerColor));
                Assert.That(reset[index].Clothing.OuterColor, Is.EqualTo(first[index].Clothing.OuterColor));
            }
        }

        [Test]
        public void SeededCompanyUsesAValidDiverseEarlyModernWardrobe()
        {
            WorldSnapshot snapshot = new WorldSimulation(170601UL).CreateSnapshot();
            var combinations = new HashSet<string>();

            for (int index = 0; index < snapshot.Count; index++)
            {
                ClothingAppearance clothing = snapshot[index].Clothing;
                Assert.That((int)clothing.Upper, Is.InRange(0, 5));
                Assert.That((int)clothing.Lower, Is.InRange(0, 3));
                Assert.That((int)clothing.Outer, Is.InRange(0, 2));
                Assert.That((int)clothing.Headwear, Is.InRange(0, 4));
                Assert.That((int)clothing.Footwear, Is.InRange(0, 1));
                Assert.That(clothing.UpperColor, Is.LessThan(6));
                Assert.That(clothing.LowerColor, Is.LessThan(6));
                Assert.That(clothing.OuterColor, Is.LessThan(6));
                combinations.Add($"{clothing.Upper}:{clothing.Lower}:{clothing.Outer}:{clothing.Headwear}:{clothing.Footwear}:{clothing.UpperColor}:{clothing.LowerColor}");
            }

            Assert.That(combinations.Count, Is.GreaterThanOrEqualTo(18));
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
            }

            var otherSeed = new WorldSimulation(43UL);
            Assert.That(otherSeed.ComputeChecksum(), Is.Not.EqualTo(simulation.ComputeChecksum()));
        }
    }
}
