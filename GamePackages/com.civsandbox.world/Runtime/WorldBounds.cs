namespace CivSandbox.World
{
    public readonly struct WorldBounds
    {
        public WorldBounds(int minimumEastMillimeters, int maximumEastMillimeters, int minimumNorthMillimeters, int maximumNorthMillimeters)
        {
            MinimumEastMillimeters = minimumEastMillimeters;
            MaximumEastMillimeters = maximumEastMillimeters;
            MinimumNorthMillimeters = minimumNorthMillimeters;
            MaximumNorthMillimeters = maximumNorthMillimeters;
        }

        public int MinimumEastMillimeters { get; }

        public int MaximumEastMillimeters { get; }

        public int MinimumNorthMillimeters { get; }

        public int MaximumNorthMillimeters { get; }

        public bool Contains(WorldPosition position)
        {
            return position.EastMillimeters >= MinimumEastMillimeters &&
                   position.EastMillimeters <= MaximumEastMillimeters &&
                   position.NorthMillimeters >= MinimumNorthMillimeters &&
                   position.NorthMillimeters <= MaximumNorthMillimeters;
        }
    }
}
