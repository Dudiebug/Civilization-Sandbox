using System;

namespace CivSandbox.Simulation
{
    public readonly struct StableEntityId : IEquatable<StableEntityId>, IComparable<StableEntityId>
    {
        public StableEntityId(ulong world, ulong local)
        {
            if (local == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(local), "Stable local IDs begin at one.");
            }

            World = world;
            Local = local;
        }

        public ulong World { get; }

        public ulong Local { get; }

        public int CompareTo(StableEntityId other)
        {
            int worldOrder = World.CompareTo(other.World);
            return worldOrder != 0 ? worldOrder : Local.CompareTo(other.Local);
        }

        public bool Equals(StableEntityId other) => World == other.World && Local == other.Local;

        public override bool Equals(object obj) => obj is StableEntityId other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)World * 397) ^ (int)(World >> 32) ^ (int)Local ^ (int)(Local >> 32);
            }
        }

        public static bool operator ==(StableEntityId left, StableEntityId right) => left.Equals(right);

        public static bool operator !=(StableEntityId left, StableEntityId right) => !left.Equals(right);

        public override string ToString() => $"{World:x8}:{Local:D4}";
    }
}
