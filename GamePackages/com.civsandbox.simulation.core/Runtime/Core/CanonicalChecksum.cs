using System.Text;

namespace CivSandbox.Simulation
{
    public struct CanonicalChecksum
    {
        private const ulong Offset = 14695981039346656037UL;
        private const ulong Prime = 1099511628211UL;
        private ulong value;

        public static CanonicalChecksum Create()
        {
            return new CanonicalChecksum { value = Offset };
        }

        public ulong Value => value;

        public void Add(byte item)
        {
            unchecked
            {
                value = (value ^ item) * Prime;
            }
        }

        public void Add(int item) => Add((long)item);

        public void Add(long item) => Add((ulong)item);

        public void Add(ulong item)
        {
            for (int shift = 0; shift < 64; shift += 8)
            {
                Add((byte)(item >> shift));
            }
        }

        public void Add(string item)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(item ?? string.Empty);
            Add(bytes.Length);
            for (int index = 0; index < bytes.Length; index++)
            {
                Add(bytes[index]);
            }
        }
    }
}
