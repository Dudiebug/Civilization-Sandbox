using System;
using CivSandbox.Simulation;

namespace CivSandbox.World
{
    public static class WorldGenerator
    {
        private const ulong ContinentalStream = 0x636f6e74696e656eUL;
        private const ulong DetailStream = 0x7465727261696e31UL;
        private const ulong MoistureStream = 0x6d6f697374757265UL;
        private const ulong ResourceStream = 0x7265736f75726365UL;
        private const int SeaLevelPermille = 500;
        private static readonly int[] ContinentalScales = { 256, 96, 32 };
        private static readonly int[] ContinentalWeights = { 5, 3, 2 };
        private static readonly int[] DetailScales = { 48, 16, 6 };
        private static readonly int[] DetailWeights = { 2, 2, 1 };
        private static readonly int[] MoistureScales = { 220, 80, 24 };
        private static readonly int[] MoistureWeights = { 5, 3, 2 };

        public static GeneratedWorld Generate(WorldGenerationSettings settings)
        {
            WorldSizeDefinition size = settings.Size.Definition();
            int columns = size.Columns;
            int rows = size.Rows;
            int count = columns * rows;
            int[] continental = CreateFractalField(
                settings.Seed.Value,
                ContinentalStream,
                columns,
                rows,
                ContinentalScales,
                ContinentalWeights);
            int[] detail = CreateFractalField(
                settings.Seed.Value,
                DetailStream,
                columns,
                rows,
                DetailScales,
                DetailWeights);
            int[] moisture = CreateFractalField(
                settings.Seed.Value,
                MoistureStream,
                columns,
                rows,
                MoistureScales,
                MoistureWeights);
            int[] elevations = new int[count];
            int[] temperatures = new int[count];
            int[] moistures = new int[count];
            int[] fertilities = new int[count];
            int[] resources = new int[count];
            WorldBiome[] biomes = new WorldBiome[count];
            ulong worldId = ComputeWorldId(settings);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    int index = row * columns + column;
                    int edgeNorth = Math.Abs(row * 2 - (rows - 1)) * 1000 / Math.Max(1, rows - 1);
                    int distanceToEdge = Math.Min(Math.Min(column, columns - 1 - column), Math.Min(row, rows - 1 - row));
                    int oceanBorder = Math.Max(6, Math.Min(columns, rows) / 14);
                    int edgePenalty = distanceToEdge >= oceanBorder
                        ? 0
                        : (oceanBorder - distanceToEdge) * 650 / oceanBorder;
                    int elevation = (continental[index] * 7 + detail[index] * 3) / 10 +
                                    (settings.LandPercent - 50) * 6 -
                                    edgePenalty;
                    elevation = ClampPermille(elevation);
                    elevations[index] = elevation;

                    int moistureValue = ClampPermille(moisture[index] + (settings.RainfallPercent - 50) * 5);
                    int latitudeWarmth = (1000 - edgeNorth) * 350 / 1000;
                    int elevationCooling = Math.Max(0, elevation - SeaLevelPermille) / 2;
                    int temperature = ClampPermille(settings.TemperaturePercent * 8 + latitudeWarmth - elevationCooling);
                    int fertility = ClampPermille((moistureValue * 3 + temperature * 2) / 5 - Math.Max(0, elevation - 700));
                    int resource = KeyedRandom.Range(
                        settings.Seed.Value,
                        ResourceStream,
                        (ulong)index + 1UL,
                        0,
                        0,
                        0,
                        1001);
                    resource = ClampPermille(resource + (settings.ResourcePercent - 50) * 7 + Math.Max(0, elevation - 650) / 2);

                    moistures[index] = moistureValue;
                    temperatures[index] = temperature;
                    fertilities[index] = fertility;
                    resources[index] = resource;
                    biomes[index] = ClassifyBaseBiome(elevation, moistureValue, temperature, settings);
                }
            }

            MarkCoasts(biomes, columns, rows);
            EnsureFoundableRegion(biomes, elevations, moistures, temperatures, fertilities, columns, rows);

            var resourceProfiles = new WorldResourceProfile[count];
            for (int index = 0; index < count; index++)
            {
                resourceProfiles[index] = CreateResourceProfile(
                    settings,
                    index,
                    biomes[index],
                    elevations[index],
                    moistures[index],
                    temperatures[index],
                    fertilities[index],
                    resources[index]);
                resources[index] = resourceProfiles[index].OpportunityPermille;
            }

            var cells = new GeneratedWorldCell[count];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    int index = row * columns + column;
                    cells[index] = new GeneratedWorldCell(
                        new StableEntityId(worldId, (ulong)index + 1UL),
                        column,
                        row,
                        elevations[index],
                        moistures[index],
                        temperatures[index],
                        fertilities[index],
                        resources[index],
                        resourceProfiles[index],
                        biomes[index]);
                }
            }

            return new GeneratedWorld(settings, worldId, columns, rows, cells);
        }

        private static ulong ComputeWorldId(WorldGenerationSettings settings)
        {
            CanonicalChecksum checksum = CanonicalChecksum.Create();
            checksum.Add("CIV-GENERATED-WORLD-IDENTITY-v1");
            checksum.Add(settings.GenerationVersion);
            checksum.Add(settings.Seed.Value);
            checksum.Add((int)settings.Size);
            checksum.Add(settings.LandPercent);
            checksum.Add(settings.TemperaturePercent);
            checksum.Add(settings.RainfallPercent);
            checksum.Add(settings.MountainPercent);
            checksum.Add(settings.ForestPercent);
            checksum.Add(settings.ResourcePercent);
            return checksum.Value == 0UL ? 1UL : checksum.Value;
        }

        private static WorldResourceProfile CreateResourceProfile(
            WorldGenerationSettings settings,
            int index,
            WorldBiome biome,
            int elevation,
            int moisture,
            int temperature,
            int fertility,
            int geologicalRichness)
        {
            bool water = biome == WorldBiome.Ocean;
            int landTemperature = temperature >= 260 && temperature <= 850 ? 900 : 350;
            int freshWaterSuitability = !water && moisture >= 560
                ? ClampPermille(480 + (moisture - 560) * 2 + Math.Max(0, elevation - 520) / 2)
                : 80;
            int stapleFoodSuitability = water
                ? 0
                : ClampPermille((fertility * 3 + landTemperature) / 4);
            int proteinFoodSuitability = water || biome == WorldBiome.Coast
                ? 900
                : biome == WorldBiome.Woodland ? 720 : biome == WorldBiome.Grassland ? 560 : 260;
            int timberSuitability = biome == WorldBiome.Woodland
                ? 1000
                : biome == WorldBiome.Grassland ? 280 : biome == WorldBiome.Coast ? 180 : 0;
            int stoneSuitability = biome == WorldBiome.Highland
                ? 1000
                : water ? 0 : ClampPermille(220 + Math.Max(0, elevation - 570) * 2);
            int claySuitability = water
                ? 0
                : biome == WorldBiome.Coast ? 900 : ClampPermille((moisture + (1000 - elevation)) / 2);
            int fiberSuitability = water
                ? 0
                : ClampPermille((moisture * 3 + fertility) / 4 + (biome == WorldBiome.Coast ? 180 : 0));
            int ironSuitability = water
                ? 0
                : ClampPermille(geologicalRichness / 2 + Math.Max(0, elevation - 580) * 2);
            int coalSuitability = water
                ? 0
                : ClampPermille(geologicalRichness / 2 +
                    (biome == WorldBiome.Highland ? 320 : biome == WorldBiome.Woodland ? 170 : 0));
            int medicinalSuitability = water
                ? 0
                : ClampPermille((moisture + fertility) / 2 + (biome == WorldBiome.Woodland ? 260 : 0));

            return new WorldResourceProfile(
                Deposit(settings, index, WorldResourceKind.FreshWater, freshWaterSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.StapleFood, stapleFoodSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.ProteinFood, proteinFoodSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.Timber, timberSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.Stone, stoneSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.Clay, claySuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.Fiber, fiberSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.IronOre, ironSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.Coal, coalSuitability, geologicalRichness),
                Deposit(settings, index, WorldResourceKind.MedicinalInputs, medicinalSuitability, geologicalRichness));
        }

        private static int Deposit(
            WorldGenerationSettings settings,
            int index,
            WorldResourceKind kind,
            int suitability,
            int geologicalRichness)
        {
            int localVariation = KeyedRandom.Range(
                settings.Seed.Value,
                ResourceStream,
                (ulong)index + 1UL,
                (ulong)kind + 1UL,
                0,
                0,
                1001);
            int score = (suitability * 3 + geologicalRichness + localVariation) / 5 +
                        (settings.ResourcePercent - 50) * 6;
            return score <= 450 ? 0 : ClampPermille((score - 450) * 1000 / 550);
        }

        private static int[] CreateFractalField(
            ulong seed,
            ulong stream,
            int columns,
            int rows,
            int[] scales,
            int[] weights)
        {
            if (scales == null || weights == null || scales.Length != weights.Length || scales.Length == 0)
            {
                throw new ArgumentException("Fractal field scales and weights must be non-empty and paired.");
            }

            int count = columns * rows;
            var result = new int[count];
            int totalWeight = 0;

            for (int octave = 0; octave < scales.Length; octave++)
            {
                int scale = Math.Max(1, scales[octave]);
                int weight = Math.Max(1, weights[octave]);
                totalWeight += weight;
                int latticeColumns = columns / scale + 2;
                int latticeRows = rows / scale + 2;
                var lattice = new int[latticeColumns * latticeRows];
                for (int latticeRow = 0; latticeRow < latticeRows; latticeRow++)
                {
                    for (int latticeColumn = 0; latticeColumn < latticeColumns; latticeColumn++)
                    {
                        ulong latticeId = ((ulong)(uint)latticeRow << 32) | (uint)latticeColumn;
                        lattice[latticeRow * latticeColumns + latticeColumn] = KeyedRandom.Range(
                            seed,
                            stream,
                            latticeId + 1UL,
                            (ulong)octave,
                            0,
                            0,
                            1001);
                    }
                }

                for (int row = 0; row < rows; row++)
                {
                    int latticeRow = row / scale;
                    int northFraction = row % scale * 1000 / scale;
                    for (int column = 0; column < columns; column++)
                    {
                        int latticeColumn = column / scale;
                        int eastFraction = column % scale * 1000 / scale;
                        int southWest = lattice[latticeRow * latticeColumns + latticeColumn];
                        int southEast = lattice[latticeRow * latticeColumns + latticeColumn + 1];
                        int northWest = lattice[(latticeRow + 1) * latticeColumns + latticeColumn];
                        int northEast = lattice[(latticeRow + 1) * latticeColumns + latticeColumn + 1];
                        int south = LerpPermille(southWest, southEast, eastFraction);
                        int north = LerpPermille(northWest, northEast, eastFraction);
                        result[row * columns + column] += LerpPermille(south, north, northFraction) * weight;
                    }
                }
            }

            for (int index = 0; index < result.Length; index++)
            {
                result[index] /= totalWeight;
            }

            return result;
        }

        private static int LerpPermille(int from, int to, int fractionPermille)
        {
            return from + (to - from) * fractionPermille / 1000;
        }

        private static WorldBiome ClassifyBaseBiome(
            int elevation,
            int moisture,
            int temperature,
            WorldGenerationSettings settings)
        {
            if (elevation < SeaLevelPermille) return WorldBiome.Ocean;
            int mountainThreshold = 760 - settings.MountainPercent * 2;
            if (elevation >= mountainThreshold) return WorldBiome.Highland;
            if (temperature < 250) return WorldBiome.Snow;
            int forestThreshold = 760 - settings.ForestPercent * 4;
            if (moisture >= forestThreshold && temperature >= 300) return WorldBiome.Woodland;
            if (moisture < 300) return WorldBiome.Dryland;
            return WorldBiome.Grassland;
        }

        private static void MarkCoasts(WorldBiome[] biomes, int columns, int rows)
        {
            var coast = new bool[biomes.Length];
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    int index = row * columns + column;
                    if (biomes[index] == WorldBiome.Ocean || biomes[index] == WorldBiome.Highland || biomes[index] == WorldBiome.Snow)
                    {
                        continue;
                    }

                    coast[index] = IsOcean(biomes, columns, rows, column - 1, row) ||
                                   IsOcean(biomes, columns, rows, column + 1, row) ||
                                   IsOcean(biomes, columns, rows, column, row - 1) ||
                                   IsOcean(biomes, columns, rows, column, row + 1);
                }
            }

            for (int index = 0; index < coast.Length; index++)
            {
                if (coast[index]) biomes[index] = WorldBiome.Coast;
            }
        }

        private static bool IsOcean(WorldBiome[] biomes, int columns, int rows, int column, int row)
        {
            return column >= 0 && column < columns && row >= 0 && row < rows && biomes[row * columns + column] == WorldBiome.Ocean;
        }

        private static void EnsureFoundableRegion(
            WorldBiome[] biomes,
            int[] elevations,
            int[] moistures,
            int[] temperatures,
            int[] fertilities,
            int columns,
            int rows)
        {
            for (int index = 0; index < biomes.Length; index++)
            {
                if (IsFoundable(biomes[index])) return;
            }

            int center = rows / 2 * columns + columns / 2;
            elevations[center] = Math.Max(SeaLevelPermille + 20, elevations[center]);
            moistures[center] = Math.Max(450, moistures[center]);
            temperatures[center] = Math.Max(400, temperatures[center]);
            fertilities[center] = Math.Max(500, fertilities[center]);
            biomes[center] = WorldBiome.Grassland;
        }

        private static bool IsFoundable(WorldBiome biome)
        {
            return biome == WorldBiome.Coast ||
                   biome == WorldBiome.Grassland ||
                   biome == WorldBiome.Woodland ||
                   biome == WorldBiome.Dryland;
        }

        private static int ClampPermille(int value)
        {
            if (value < 0) return 0;
            return value > 1000 ? 1000 : value;
        }
    }
}
