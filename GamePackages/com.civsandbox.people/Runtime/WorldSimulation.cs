using System;
using CivSandbox.Simulation;
using CivSandbox.World;

namespace CivSandbox.People
{
    public sealed class WorldSimulation
    {
        public const int PersonCount = 24;
        public const ulong InitialPeopleStream = 0x70656f706c653031UL;
        public const ulong MovementStream = 0x6d6f76656d656e74UL;
        public const ulong SurvivalNeedsStream = 0x737572766976616cUL;
        public const int NeedReviewSeconds = 60;
        public const int NutritionLossPerReview = 5;
        public const int HydrationLossPerReview = 10;
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
        private readonly SimulationClock clock = new SimulationClock();
        private ulong nextLocalId;
        private WorldSeed seed;

        public WorldSimulation(ulong seedValue)
        {
            Reset(seedValue);
        }

        public WorldBounds Bounds { get; } = new WorldBounds(-22000, 22000, -22000, 22000);

        public WorldTime Time => clock.Time;

        public WorldSeed Seed => seed;

        public void Reset(ulong seedValue)
        {
            seed = new WorldSeed(seedValue);
            clock.Reset();
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
                var person = new PersonState(new StableEntityId(worldKey, localId), name, new WorldPosition(east, north), appearance)
                {
                    IdleSecondsRemaining = KeyedRandom.Range(seedValue, MovementStream, localId, 0, 0, 0, 46),
                    NutritionUnits = KeyedRandom.Range(seedValue, SurvivalNeedsStream, localId, 0, 0, 7800, SurvivalNeedsSnapshot.CapacityUnits + 1),
                    HydrationUnits = KeyedRandom.Range(seedValue, SurvivalNeedsStream, localId, 0, 1, 7000, SurvivalNeedsSnapshot.CapacityUnits + 1),
                    NeedReviewOffsetSeconds = KeyedRandom.Range(seedValue, SurvivalNeedsStream, localId, 0, 2, 0, NeedReviewSeconds)
                };
                people[index] = person;
            }
        }

        public void AdvanceFixedWallTick(SimulationSpeed speed)
        {
            int seconds = clock.AdvanceFixedWallTick(speed);
            if (seconds <= 0)
            {
                return;
            }

            long firstAdvancedSecond = clock.Time.Seconds - seconds + 1L;
            for (int step = 0; step < seconds; step++)
            {
                AdvanceOneGameSecond(firstAdvancedSecond + step);
            }
        }

        public WorldSnapshot CreateSnapshot()
        {
            var copy = new PersonSnapshot[people.Length];
            for (int index = 0; index < people.Length; index++)
            {
                PersonState person = people[index];
                var needs = new SurvivalNeedsSnapshot(person.NutritionUnits, person.HydrationUnits);
                copy[index] = new PersonSnapshot(
                    person.Id,
                    person.Name,
                    person.Position,
                    person.Action,
                    DetermineActionReason(person, needs),
                    needs,
                    person.AppearanceVariant);
            }

            return new WorldSnapshot(seed, clock.Time, Bounds, copy);
        }

        public ulong ComputeChecksum()
        {
            CanonicalChecksum checksum = CanonicalChecksum.Create();
            checksum.Add("CIV-BUILD02-WORLD-v1");
            checksum.Add(seed.Value);
            checksum.Add(clock.Time.Seconds);
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
                checksum.Add(person.NutritionUnits);
                checksum.Add(person.HydrationUnits);
                checksum.Add(person.NeedReviewOffsetSeconds);
            }

            return checksum.Value;
        }

        private void AdvanceOneGameSecond(long gameSecond)
        {
            for (int index = 0; index < people.Length; index++)
            {
                PersonState person = people[index];
                AdvanceNeeds(person, gameSecond);
                AdvancePerson(person);
            }
        }

        private static void AdvanceNeeds(PersonState person, long gameSecond)
        {
            if (gameSecond % NeedReviewSeconds != person.NeedReviewOffsetSeconds)
            {
                return;
            }

            person.NutritionUnits = Math.Max(0, person.NutritionUnits - NutritionLossPerReview);
            person.HydrationUnits = Math.Max(0, person.HydrationUnits - HydrationLossPerReview);
        }

        private static PersonActionReason DetermineActionReason(PersonState person, SurvivalNeedsSnapshot needs)
        {
            if (needs.HydrationUrgency >= NeedUrgency.Urgent)
            {
                return PersonActionReason.WaterUnavailable;
            }

            if (needs.NutritionUrgency >= NeedUrgency.Urgent)
            {
                return PersonActionReason.FoodUnavailable;
            }

            return person.Action == PersonAction.Walking
                ? PersonActionReason.ExploringBoundedArea
                : PersonActionReason.RestingBetweenWalks;
        }

        private void AdvancePerson(PersonState person)
        {
            if (person.Action == PersonAction.Waiting)
            {
                if (person.IdleSecondsRemaining > 0)
                {
                    person.IdleSecondsRemaining--;
                    return;
                }

                BeginMove(person);
                return;
            }

            person.MoveElapsedSeconds++;
            if (person.MoveElapsedSeconds >= person.MoveDurationSeconds)
            {
                person.Position = person.MoveTarget;
                person.Action = PersonAction.Waiting;
                person.IdleSecondsRemaining = KeyedRandom.Range(
                    seed.Value,
                    MovementStream,
                    person.Id.Local,
                    person.DecisionOrdinal,
                    4,
                    30,
                    151);
                return;
            }

            long elapsed = person.MoveElapsedSeconds;
            long duration = person.MoveDurationSeconds;
            int east = person.MoveStart.EastMillimeters + (int)(((long)person.MoveTarget.EastMillimeters - person.MoveStart.EastMillimeters) * elapsed / duration);
            int north = person.MoveStart.NorthMillimeters + (int)(((long)person.MoveTarget.NorthMillimeters - person.MoveStart.NorthMillimeters) * elapsed / duration);
            person.Position = new WorldPosition(east, north);
        }

        private void BeginMove(PersonState person)
        {
            ulong occurrence = person.DecisionOrdinal;
            person.DecisionOrdinal++;
            int margin = 1500;
            int targetEast = KeyedRandom.Range(
                seed.Value,
                MovementStream,
                person.Id.Local,
                occurrence,
                1,
                Bounds.MinimumEastMillimeters + margin,
                Bounds.MaximumEastMillimeters - margin + 1);
            int targetNorth = KeyedRandom.Range(
                seed.Value,
                MovementStream,
                person.Id.Local,
                occurrence,
                2,
                Bounds.MinimumNorthMillimeters + margin,
                Bounds.MaximumNorthMillimeters - margin + 1);
            int speedMillimetersPerGameSecond = KeyedRandom.Range(
                seed.Value,
                MovementStream,
                person.Id.Local,
                occurrence,
                3,
                16,
                25);

            person.MoveStart = person.Position;
            person.MoveTarget = new WorldPosition(targetEast, targetNorth);
            person.MoveElapsedSeconds = 0;
            int eastDistance = Math.Abs(targetEast - person.Position.EastMillimeters);
            int northDistance = Math.Abs(targetNorth - person.Position.NorthMillimeters);
            int distance = Math.Max(eastDistance, northDistance);
            person.MoveDurationSeconds = Math.Max(1, (distance + speedMillimetersPerGameSecond - 1) / speedMillimetersPerGameSecond);
            person.Action = PersonAction.Walking;
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
            public int NutritionUnits { get; set; }
            public int HydrationUnits { get; set; }
            public int NeedReviewOffsetSeconds { get; set; }
        }
    }
}
