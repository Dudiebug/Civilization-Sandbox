using System;

namespace CivSandbox.World
{
    public readonly struct WorldSeed : IEquatable<WorldSeed>
    {
        public WorldSeed(ulong value)
        {
            Value = value;
        }

        public ulong Value { get; }

        public bool Equals(WorldSeed other) => Value == other.Value;

        public override bool Equals(object obj) => obj is WorldSeed other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
    }
}
