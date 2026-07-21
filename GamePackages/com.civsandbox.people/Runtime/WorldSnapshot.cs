using System;
using System.Collections;
using System.Collections.Generic;
using CivSandbox.Simulation;
using CivSandbox.World;

namespace CivSandbox.People
{
    public sealed class WorldSnapshot : IReadOnlyList<PersonSnapshot>
    {
        private readonly PersonSnapshot[] people;

        internal WorldSnapshot(WorldSeed seed, WorldTime time, WorldBounds bounds, PersonSnapshot[] people)
        {
            Seed = seed;
            Time = time;
            Bounds = bounds;
            this.people = people ?? throw new ArgumentNullException(nameof(people));
        }

        public WorldSeed Seed { get; }

        public WorldTime Time { get; }

        public WorldBounds Bounds { get; }

        public int Count => people.Length;

        public PersonSnapshot this[int index] => people[index];

        public IEnumerator<PersonSnapshot> GetEnumerator()
        {
            for (int index = 0; index < people.Length; index++)
            {
                yield return people[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
