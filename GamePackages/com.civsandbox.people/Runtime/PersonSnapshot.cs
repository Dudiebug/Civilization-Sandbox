using CivSandbox.Simulation;
using CivSandbox.World;

namespace CivSandbox.People
{
    public readonly struct PersonSnapshot
    {
        public PersonSnapshot(
            StableEntityId id,
            string name,
            WorldPosition position,
            PersonAction action,
            PersonActionReason actionReason,
            SurvivalNeedsSnapshot needs,
            int appearanceVariant)
        {
            Id = id;
            Name = name;
            Position = position;
            Action = action;
            ActionReason = actionReason;
            Needs = needs;
            AppearanceVariant = appearanceVariant;
        }

        public StableEntityId Id { get; }

        public string Name { get; }

        public WorldPosition Position { get; }

        public PersonAction Action { get; }

        public PersonActionReason ActionReason { get; }

        public SurvivalNeedsSnapshot Needs { get; }

        public int AppearanceVariant { get; }
    }
}
