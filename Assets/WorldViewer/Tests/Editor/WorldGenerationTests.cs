using System;
using System.Collections.Generic;
using System.Diagnostics;
using CivSandbox.Simulation;
using CivSandbox.World;
using NUnit.Framework;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class WorldGenerationTests
    {
        [Test]
        public void FoundingLocationIsDeterministicValidAndNotHardcodedToFirstLandCell()
        {
            GeneratedWorld first = WorldGenerator.Generate(WorldGenerationSettings.Default);
            GeneratedWorld second = WorldGenerator.Generate(WorldGenerationSettings.Default);

            Assert.That(first.TryChooseFoundingCell(out GeneratedWorldCell firstChoice), Is.True);
            Assert.That(second.TryChooseFoundingCell(out GeneratedWorldCell secondChoice), Is.True);
            Assert.That(firstChoice.IsFoundable, Is.True);
            Assert.That(first.IsFoundingSite(firstChoice), Is.True);
            Assert.That(secondChoice.Id, Is.EqualTo(firstChoice.Id));
            Assert.That(first.TryFindFirstFoundableCell(out GeneratedWorldCell firstLand), Is.True);
            Assert.That(firstChoice.Id, Is.Not.EqualTo(firstLand.Id));
        }

        [Test]
        public void SameSettingsProduceSameWorldAndStableRegionIds()
        {
            WorldGenerationSettings settings = WorldGenerationSettings.Default;
            GeneratedWorld first = WorldGenerator.Generate(settings);
            GeneratedWorld second = WorldGenerator.Generate(settings);

            Assert.That(second.Checksum, Is.EqualTo(first.Checksum));
            Assert.That(second.WorldId, Is.EqualTo(first.WorldId));
            Assert.That(second.CellCount, Is.EqualTo(first.CellCount));

            for (int index = 0; index < first.CellCount; index++)
            {
                Assert.That(second[index].Id, Is.EqualTo(first[index].Id));
                Assert.That(second[index].Biome, Is.EqualTo(first[index].Biome));
                Assert.That(second[index].ElevationPermille, Is.EqualTo(first[index].ElevationPermille));
                Assert.That(second[index].Resources, Is.EqualTo(first[index].Resources));
            }
        }

        [Test]
        public void DefaultWorldDistributesEveryBuild02SourceResourceDeterministically()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            Array kinds = Enum.GetValues(typeof(WorldResourceKind));
            var populatedCells = new int[kinds.Length];
            for (int index = 0; index < world.CellCount; index++)
            {
                GeneratedWorldCell cell = world[index];
                foreach (WorldResourceKind kind in kinds)
                {
                    if (cell.Resources.Amount(kind) > 0)
                    {
                        populatedCells[(int)kind]++;
                    }
                }

                if (cell.IsWater)
                {
                    Assert.That(cell.Resources.FreshWaterPermille, Is.Zero, "Ocean water must not masquerade as fresh water.");
                }
            }

            foreach (WorldResourceKind kind in kinds)
            {
                Assert.That(populatedCells[(int)kind], Is.GreaterThan(0), $"The default world did not contain {kind}.");
            }
        }

        [TestCase(WorldSizePreset.Small, 96, 96)]
        [TestCase(WorldSizePreset.Standard, 384, 384)]
        [TestCase(WorldSizePreset.Large, 1024, 1024)]
        public void SizePresetsProduceDeclaredBoundedDimensions(WorldSizePreset preset, int columns, int rows)
        {
            WorldGenerationSettings baseline = WorldGenerationSettings.Default;
            var settings = new WorldGenerationSettings(
                baseline.Seed.Value,
                preset,
                baseline.LandPercent,
                baseline.TemperaturePercent,
                baseline.RainfallPercent,
                baseline.MountainPercent,
                baseline.ForestPercent,
                baseline.ResourcePercent);

            GeneratedWorld world = WorldGenerator.Generate(settings);
            Assert.That(world.Columns, Is.EqualTo(columns));
            Assert.That(world.Rows, Is.EqualTo(rows));
            Assert.That(world.CellCount, Is.EqualTo(columns * rows));
            Assert.That(world.TryFindFirstFoundableCell(out _), Is.True);
        }

        [Test]
        public void EveryExposedControlChangesCanonicalWorldState()
        {
            WorldGenerationSettings baseline = WorldGenerationSettings.Default;
            ulong baselineChecksum = WorldGenerator.Generate(baseline).Checksum;
            var variants = new[]
            {
                Copy(baseline, seed: baseline.Seed.Value + 1UL),
                Copy(baseline, size: WorldSizePreset.Small),
                Copy(baseline, land: baseline.LandPercent + 1),
                Copy(baseline, temperature: baseline.TemperaturePercent + 1),
                Copy(baseline, rainfall: baseline.RainfallPercent + 1),
                Copy(baseline, mountains: baseline.MountainPercent + 1),
                Copy(baseline, forests: baseline.ForestPercent + 1),
                Copy(baseline, resources: baseline.ResourcePercent + 1)
            };

            foreach (WorldGenerationSettings variant in variants)
            {
                Assert.That(WorldGenerator.Generate(variant).Checksum, Is.Not.EqualTo(baselineChecksum));
            }
        }

        [Test]
        public void DefaultWorldContainsWaterLandAndUniqueRegionIdentity()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            int water = 0;
            int foundable = 0;
            var identities = new HashSet<StableEntityId>();
            var biomes = new HashSet<WorldBiome>();
            for (int index = 0; index < world.CellCount; index++)
            {
                GeneratedWorldCell cell = world[index];
                if (cell.IsWater) water++;
                if (cell.IsFoundable) foundable++;
                biomes.Add(cell.Biome);
                Assert.That(identities.Add(cell.Id), Is.True);
                Assert.That(cell.Id.World, Is.EqualTo(world.WorldId));
                Assert.That(cell.ElevationPermille, Is.InRange(0, 1000));
                Assert.That(cell.MoisturePermille, Is.InRange(0, 1000));
                Assert.That(cell.TemperaturePermille, Is.InRange(0, 1000));
                Assert.That(cell.ResourcePermille, Is.InRange(0, 1000));
            }

            Assert.That(water, Is.GreaterThan(0));
            Assert.That(foundable, Is.GreaterThan(0));
            Assert.That(biomes.Count, Is.GreaterThanOrEqualTo(4), "The default preview should not collapse into one land color and ocean.");
            Assert.That(water + foundable, Is.LessThanOrEqualTo(world.CellCount));
        }

        [Test]
        public void OutOfRangeControlsAreRejected()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new WorldGenerationSettings(1UL, WorldSizePreset.Small, -1, 50, 50, 50, 50, 50));
            Assert.Throws<ArgumentOutOfRangeException>(() => new WorldGenerationSettings(1UL, WorldSizePreset.Small, 50, 50, 50, 50, 50, 101));
        }

        [Test]
        public void LargeWorldGenerationStaysInsidePrototypeBudget()
        {
            WorldGenerationSettings baseline = WorldGenerationSettings.Default;
            WorldGenerationSettings large = Copy(baseline, size: WorldSizePreset.Large);
            var stopwatch = Stopwatch.StartNew();
            GeneratedWorld world = WorldGenerator.Generate(large);
            stopwatch.Stop();

            Assert.That(world.CellCount, Is.EqualTo(1024 * 1024));
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(15000),
                "Max prototype world generation exceeded the provisional fifteen-second editor budget.");
        }

        private static WorldGenerationSettings Copy(
            WorldGenerationSettings source,
            ulong? seed = null,
            WorldSizePreset? size = null,
            int? land = null,
            int? temperature = null,
            int? rainfall = null,
            int? mountains = null,
            int? forests = null,
            int? resources = null)
        {
            return new WorldGenerationSettings(
                seed ?? source.Seed.Value,
                size ?? source.Size,
                land ?? source.LandPercent,
                temperature ?? source.TemperaturePercent,
                rainfall ?? source.RainfallPercent,
                mountains ?? source.MountainPercent,
                forests ?? source.ForestPercent,
                resources ?? source.ResourcePercent);
        }
    }
}
