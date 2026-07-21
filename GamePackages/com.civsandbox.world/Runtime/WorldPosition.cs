using System;

namespace CivSandbox.World
{
    public readonly struct WorldPosition : IEquatable<WorldPosition>
    {
        public WorldPosition(int eastMillimeters, int northMillimeters)
        {
            EastMillimeters = eastMillimeters;
            NorthMillimeters = northMillimeters;
        }

        public int EastMillimeters { get; }

        public int NorthMillimeters { get; }

        public bool Equals(WorldPosition other)
        {
            return EastMillimeters == other.EastMillimeters && NorthMillimeters == other.NorthMillimeters;
        }

        public override bool Equals(object obj) => obj is WorldPosition other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (EastMillimeters * 397) ^ NorthMillimeters;
            }
        }

        public override string ToString()
        {
            return $"({EastMillimeters / 1000.0:0.0} m E, {NorthMillimeters / 1000.0:0.0} m N)";
        }
    }
}
