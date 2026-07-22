using CivSandbox.Simulation;

namespace CivSandbox.World
{
    public readonly struct GeneratedWorldCell
    {
        public GeneratedWorldCell(
            StableEntityId id,
            int column,
            int row,
            int elevationPermille,
            int moisturePermille,
            int temperaturePermille,
            int fertilityPermille,
            int resourcePermille,
            WorldResourceProfile resources,
            WorldBiome biome)
        {
            Id = id;
            Column = column;
            Row = row;
            ElevationPermille = elevationPermille;
            MoisturePermille = moisturePermille;
            TemperaturePermille = temperaturePermille;
            FertilityPermille = fertilityPermille;
            ResourcePermille = resourcePermille;
            Resources = resources;
            Biome = biome;
        }

        public StableEntityId Id { get; }

        public int Column { get; }

        public int Row { get; }

        public int ElevationPermille { get; }

        public int MoisturePermille { get; }

        public int TemperaturePermille { get; }

        public int FertilityPermille { get; }

        public int ResourcePermille { get; }

        public WorldResourceProfile Resources { get; }

        public WorldBiome Biome { get; }

        public bool IsWater => Biome == WorldBiome.Ocean;

        public bool IsFoundable => Biome == WorldBiome.Coast ||
                                   Biome == WorldBiome.Grassland ||
                                   Biome == WorldBiome.Woodland ||
                                   Biome == WorldBiome.Dryland;
    }
}
