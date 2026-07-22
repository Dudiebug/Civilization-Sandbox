using CivSandbox.Simulation;
using CivSandbox.World;
using NUnit.Framework;

namespace CivSandbox.People.Tests
{
    public sealed class FoundingCampInteractionTests
    {
        [Test]
        public void FoundersGatherHaulCommitMaterialsAndBuildShelterDeterministically()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            Assert.That(world.TryChooseFoundingCell(out GeneratedWorldCell site), Is.True);
            var first = new WorldSimulation(world, site, 8181UL);
            var second = new WorldSimulation(world, site, 8181UL);
            WorldSnapshot initial = first.CreateSnapshot();

            Assert.That(initial.Camp.IsFounded, Is.True);
            Assert.That(initial.Camp.Count, Is.GreaterThan(0));
            Assert.That(initial.Camp.Remaining(CampCommodity.Water), Is.GreaterThan(0));
            Assert.That(initial.Camp.Remaining(CampCommodity.Food), Is.GreaterThan(0));
            Assert.That(initial.Camp.Remaining(CampCommodity.Timber), Is.GreaterThanOrEqualTo(WorldSimulation.ShelterTimberRequired));
            Assert.That(initial.Camp.Remaining(CampCommodity.Stone), Is.GreaterThanOrEqualTo(WorldSimulation.ShelterStoneRequired));

            int initialTimber = initial.Camp.Remaining(CampCommodity.Timber);
            int initialStone = initial.Camp.Remaining(CampCommodity.Stone);
            for (int tick = 0; tick < 30000 && !first.CreateSnapshot().Camp.Shelter.IsComplete; tick++)
            {
                first.AdvanceFixedWallTick(SimulationSpeed.Normal);
                second.AdvanceFixedWallTick(SimulationSpeed.Normal);
            }

            WorldSnapshot completed = first.CreateSnapshot();
            Assert.That(completed.Camp.Shelter.IsComplete, Is.True,
                "The canonical founding site should complete the first shelter from reachable material.");
            Assert.That(completed.Camp.Shelter.TimberCommitted, Is.EqualTo(WorldSimulation.ShelterTimberRequired));
            Assert.That(completed.Camp.Shelter.StoneCommitted, Is.EqualTo(WorldSimulation.ShelterStoneRequired));
            Assert.That(
                completed.Camp.Remaining(CampCommodity.Timber) + completed.Camp.Stockpile.TimberUnits +
                completed.Camp.Shelter.TimberCommitted + Carried(completed, CampCommodity.Timber),
                Is.EqualTo(initialTimber));
            Assert.That(
                completed.Camp.Remaining(CampCommodity.Stone) + completed.Camp.Stockpile.StoneUnits +
                completed.Camp.Shelter.StoneCommitted + Carried(completed, CampCommodity.Stone),
                Is.EqualTo(initialStone));
            Assert.That(second.ComputeChecksum(), Is.EqualTo(first.ComputeChecksum()));
        }

        [Test]
        public void ResourceReservationsAreBoundedAndNeverExceedAvailableUnits()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            Assert.That(world.TryChooseFoundingCell(out GeneratedWorldCell site), Is.True);
            var simulation = new WorldSimulation(world, site, 9191UL);

            for (int tick = 0; tick < 300; tick++)
            {
                simulation.AdvanceFixedWallTick(SimulationSpeed.Normal);
                WorldSnapshot snapshot = simulation.CreateSnapshot();
                Assert.That(snapshot.Camp.Count, Is.LessThanOrEqualTo(4 * WorldSimulation.MaximumResourceNodesPerCommodity));
                for (int index = 0; index < snapshot.Camp.Count; index++)
                {
                    ResourceNodeSnapshot node = snapshot.Camp[index];
                    Assert.That(node.ReservedUnits, Is.GreaterThanOrEqualTo(0));
                    Assert.That(node.ReservedUnits, Is.LessThanOrEqualTo(node.AvailableUnits));
                }
            }
        }

        private static int Carried(WorldSnapshot snapshot, CampCommodity commodity)
        {
            int total = 0;
            for (int index = 0; index < snapshot.Count; index++)
            {
                if (snapshot[index].CarriedCommodity == commodity) total += snapshot[index].CarriedUnits;
            }

            return total;
        }
    }
}
