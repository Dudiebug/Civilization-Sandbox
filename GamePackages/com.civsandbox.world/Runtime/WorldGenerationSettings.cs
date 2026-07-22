using System;

namespace CivSandbox.World
{
    public readonly struct WorldGenerationSettings : IEquatable<WorldGenerationSettings>
    {
        public const int CurrentGenerationVersion = 4;

        public WorldGenerationSettings(
            ulong seed,
            WorldSizePreset size,
            int landPercent,
            int temperaturePercent,
            int rainfallPercent,
            int mountainPercent,
            int forestPercent,
            int resourcePercent)
        {
            if (!Enum.IsDefined(typeof(WorldSizePreset), size))
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            ValidatePercent(landPercent, nameof(landPercent));
            ValidatePercent(temperaturePercent, nameof(temperaturePercent));
            ValidatePercent(rainfallPercent, nameof(rainfallPercent));
            ValidatePercent(mountainPercent, nameof(mountainPercent));
            ValidatePercent(forestPercent, nameof(forestPercent));
            ValidatePercent(resourcePercent, nameof(resourcePercent));

            Seed = new WorldSeed(seed);
            Size = size;
            LandPercent = landPercent;
            TemperaturePercent = temperaturePercent;
            RainfallPercent = rainfallPercent;
            MountainPercent = mountainPercent;
            ForestPercent = forestPercent;
            ResourcePercent = resourcePercent;
        }

        public static WorldGenerationSettings Default => new WorldGenerationSettings(
            170601UL,
            WorldSizePreset.Standard,
            52,
            50,
            52,
            45,
            55,
            50);

        public int GenerationVersion => CurrentGenerationVersion;

        public WorldSeed Seed { get; }

        public WorldSizePreset Size { get; }

        public int LandPercent { get; }

        public int TemperaturePercent { get; }

        public int RainfallPercent { get; }

        public int MountainPercent { get; }

        public int ForestPercent { get; }

        public int ResourcePercent { get; }

        public bool Equals(WorldGenerationSettings other)
        {
            return Seed.Equals(other.Seed) &&
                   Size == other.Size &&
                   LandPercent == other.LandPercent &&
                   TemperaturePercent == other.TemperaturePercent &&
                   RainfallPercent == other.RainfallPercent &&
                   MountainPercent == other.MountainPercent &&
                   ForestPercent == other.ForestPercent &&
                   ResourcePercent == other.ResourcePercent;
        }

        public override bool Equals(object obj) => obj is WorldGenerationSettings other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Seed.GetHashCode();
                hash = hash * 397 ^ (int)Size;
                hash = hash * 397 ^ LandPercent;
                hash = hash * 397 ^ TemperaturePercent;
                hash = hash * 397 ^ RainfallPercent;
                hash = hash * 397 ^ MountainPercent;
                hash = hash * 397 ^ ForestPercent;
                return hash * 397 ^ ResourcePercent;
            }
        }

        private static void ValidatePercent(int value, string parameterName)
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(parameterName, "World-generation controls use an inclusive 0-100 range.");
            }
        }
    }
}
