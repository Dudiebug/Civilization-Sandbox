using System;
using CivSandbox.Simulation;
using CivSandbox.World;

namespace CivSandbox.People
{
    public sealed class WorldSimulation
    {
        public const int FixedWallTicksPerSecond = 20;
        public const int PersonCount = 24;
        public const ulong InitialPeopleStream = 0x70656f706c653031UL;
        private static readonly string[] GivenNames =
        {
            "Alden", "Beata", "Corren", "Davia", "Edric", "Fenna", "Garran", "Hester",
            "Iven", "Jessamine", "Kester", "Liora", "Marten", "Nella", "Osric", "Petra",
            "Quillan", "Rosam", "Sayer", "Tamsin", "Ulric", "Vessa", "Willem", "Ysabet",
            "Ansel", "Brinna", "Catrin", "Dorian", "Elian", "Frida", "Gideon", "Honora"
        };

        private static readonly string[] FamilyNames =
        {
            "Alder", "Bellweather", "Candlewick", "Dunmere", "Everden", "Farrow", "Gorse",
            "Harrow", "Ives", "Kestrel", "Lark", "Morrow", "Nettle", "Pryce", "Rook", "Thorne"
        };

        private readonly PersonState[] people = new PersonState[PersonCount];
        private ulong nextLocalId;
        private long worldSeconds;
        private WorldSeed seed;

        public WorldSimulation(ulong seedValue)
        {
            Reset(seedValue);
        }

        public WorldBounds Bounds { get; } = new WorldBounds(-22000, 22000, -22000, 22000);

        public WorldTime Time => new WorldTime(worldSeconds);

        public WorldSeed Seed => seed;

        public void Reset(ulong seedValue)
        {
            seed = new WorldSeed(seedValue);
            worldSeconds = 0;
            nextLocalId = 1;
            ulong worldKey = KeyedRandom.Mix(seedValue ^ 0x776f726c642d3031UL);
            int givenOffset = KeyedRandom.Range(seedValue, InitialPeopleStream, 0, 0, 0, 0, GivenNames.Length);
            int familyOffset = KeyedRandom.Range(seedValue, InitialPeopleStream, 0, 0, 1, 0, FamilyNames.Length);

            for (int index = 0; index < people.Length; index++)
            {
                ulong localId = nextLocalId++;
                int east = KeyedRandom.Range(seedValue, InitialPeopleStream, localId, 0, 2, Bounds.MinimumEastMillimeters + 2000, Bounds.MaximumEastMillimeters - 1999);
                int north = KeyedRandom.Range(seedValue, InitialPeopleStream, localId, 0, 3, Bounds.MinimumNorthMillimeters + 2000, Bounds.MaximumNorthMillimeters - 1999);
                string name = GivenNames[(givenOffset + index) % GivenNames.Length] + " " + FamilyNames[(familyOffset + index * 7) % FamilyNames.Length];
                int appearance = KeyedRandom.Range(seedValue, InitialPeopleStream, localId, 0, 4, 0, 12);
                people[index] = new PersonState(new StableEntityId(worldKey, localId), name, new WorldPosition(east, north), appearance);
            }
        }

        public void AdvanceFixedWallTick(SimulationSpeed speed)
        {
            int seconds = speed.GameSecondsPerFixedTick();
            if (seconds <= 0)
            {
                return;
            }

            for (int step = 0; step < seconds; step++)
            {
                AdvanceOneGameSecond();
            }
        }

        public WorldSnapshot CreateSnapshot()
        {
            var copy = new PersonSnapshot[people.Length];
            for (int index = 0; index < people.Length; index++)
            {
                PersonState person = people[index];
                copy[index] = new PersonSnapshot(person.Id, person.Name, person.Position, person.Action, person.AppearanceVariant);
            }

            return new WorldSnapshot(seed, new WorldTime(worldSeconds), Bounds, copy);
        }

        public ulong ComputeChecksum()
        {
            CanonicalChecksum checksum = CanonicalChecksum.Create();
            checksum.Add("CIV-BUILD01-WORLD-v1");
            checksum.Add(seed.Value);
            checksum.Add(worldSeconds);
            checksum.Add(nextLocalId);
            checksum.Add(Bounds.MinimumEastMillimeters);
            checksum.Add(Bounds.MaximumEastMillimeters);
            checksum.Add(Bounds.MinimumNorthMillimeters);
            checksum.Add(Bounds.MaximumNorthMillimeters);

            for (int index = 0; index < people.Length; index++)
            {
                PersonState person = people[index];
                checksum.Add(person.Id.World);
                checksum.Add(person.Id.Local);
                checksum.Add(person.Name);
                checksum.Add(person.Position.EastMillimeters);
                checksum.Add(person.Position.NorthMillimeters);
                checksum.Add((byte)person.Action);
                checksum.Add(person.AppearanceVariant);
                checksum.Add(person.DecisionOrdinal);
                checksum.Add(person.IdleSecondsRemaining);
                checksum.Add(person.MoveStart.EastMillimeters);
                checksum.Add(person.MoveStart.NorthMillimeters);
                checksum.Add(person.MoveTarget.EastMillimeters);
                checksum.Add(person.MoveTarget.NorthMillimeters);
                checksum.Add(person.MoveDurationSeconds);
                checksum.Add(person.MoveElapsedSeconds);
            }

            return checksum.Value;
        }

        private void AdvanceOneGameSecond()
        {
            worldSeconds++;
        }

        private sealed class PersonState
        {
            public PersonState(StableEntityId id, string name, WorldPosition position, int appearanceVariant)
            {
                Id = id;
                Name = name;
                Position = position;
                AppearanceVariant = appearanceVariant;
                Action = PersonAction.Waiting;
                MoveStart = position;
                MoveTarget = position;
            }

            public StableEntityId Id { get; }
            public string Name { get; }
            public int AppearanceVariant { get; }
            public WorldPosition Position { get; set; }
            public PersonAction Action { get; set; }
            public ulong DecisionOrdinal { get; set; }
            public int IdleSecondsRemaining { get; set; }
            public WorldPosition MoveStart { get; set; }
            public WorldPosition MoveTarget { get; set; }
            public int MoveDurationSeconds { get; set; }
            public int MoveElapsedSeconds { get; set; }
        }
    }
}
