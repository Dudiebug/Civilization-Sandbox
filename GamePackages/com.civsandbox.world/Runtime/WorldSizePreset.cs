namespace CivSandbox.World
{
    public enum WorldSizePreset : byte
    {
        Small = 0,
        Standard = 1,
        Large = 2
    }

    public readonly struct WorldSizeDefinition
    {
        public WorldSizeDefinition(int columns, int rows, string label)
        {
            Columns = columns;
            Rows = rows;
            Label = label;
        }

        public int Columns { get; }

        public int Rows { get; }

        public string Label { get; }
    }

    public static class WorldSizePresets
    {
        public static WorldSizeDefinition Definition(this WorldSizePreset preset)
        {
            switch (preset)
            {
                case WorldSizePreset.Small: return new WorldSizeDefinition(96, 96, "Small");
                case WorldSizePreset.Large: return new WorldSizeDefinition(1024, 1024, "Max");
                default: return new WorldSizeDefinition(384, 384, "Standard");
            }
        }
    }
}
