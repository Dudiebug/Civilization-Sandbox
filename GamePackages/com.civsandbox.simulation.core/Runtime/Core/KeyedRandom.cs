namespace CivSandbox.Simulation
{
    public static class KeyedRandom
    {
        public static ulong Sample(ulong worldSeed, ulong stream, ulong stableId, ulong occurrence, ulong lane)
        {
            unchecked
            {
                ulong value = worldSeed ^ 0x9e3779b97f4a7c15UL;
                value = Mix(value + stream * 0xbf58476d1ce4e5b9UL);
                value = Mix(value + stableId * 0x94d049bb133111ebUL);
                value = Mix(value + occurrence * 0xd6e8feb86659fd93UL);
                return Mix(value + lane * 0xa0761d6478bd642fUL);
            }
        }

        public static int Range(ulong worldSeed, ulong stream, ulong stableId, ulong occurrence, ulong lane, int minimumInclusive, int maximumExclusive)
        {
            if (maximumExclusive <= minimumInclusive)
            {
                return minimumInclusive;
            }

            uint range = (uint)(maximumExclusive - minimumInclusive);
            uint value = (uint)(Sample(worldSeed, stream, stableId, occurrence, lane) >> 32);
            ulong scaled = (ulong)value * range;
            return minimumInclusive + (int)(scaled >> 32);
        }

        public static ulong Mix(ulong value)
        {
            unchecked
            {
                value += 0x9e3779b97f4a7c15UL;
                value = (value ^ (value >> 30)) * 0xbf58476d1ce4e5b9UL;
                value = (value ^ (value >> 27)) * 0x94d049bb133111ebUL;
                return value ^ (value >> 31);
            }
        }
    }
}
