using CivSandbox.Simulation;
using CivSandbox.World;

namespace CivSandbox.People
{
    public readonly struct PersonSnapshot
    {
        public PersonSnapshot(StableEntityId id, string name, WorldPosition position, PersonAction action, int appearanceVariant, ClothingAppearance clothing)
        {
            Id = id;
            Name = name;
            Position = position;
            Action = action;
            AppearanceVariant = appearanceVariant;
            Clothing = clothing;
        }

        public StableEntityId Id { get; }

        public string Name { get; }

        public WorldPosition Position { get; }

        public PersonAction Action { get; }

        public int AppearanceVariant { get; }

        public ClothingAppearance Clothing { get; }
    }
}
