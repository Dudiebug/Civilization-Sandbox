using System;

namespace CivSandbox.Simulation
{
    public readonly struct WorldTime : IEquatable<WorldTime>, IComparable<WorldTime>
    {
        public WorldTime(long seconds)
        {
            Seconds = seconds;
        }

        public long Seconds { get; }

        public int CompareTo(WorldTime other) => Seconds.CompareTo(other.Seconds);

        public bool Equals(WorldTime other) => Seconds == other.Seconds;

        public override bool Equals(object obj) => obj is WorldTime other && Equals(other);

        public override int GetHashCode() => Seconds.GetHashCode();

        public static bool operator ==(WorldTime left, WorldTime right) => left.Equals(right);

        public static bool operator !=(WorldTime left, WorldTime right) => !left.Equals(right);

        public override string ToString() => $"{Seconds}s";
    }
}
