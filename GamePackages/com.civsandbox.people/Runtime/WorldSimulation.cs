using System;
using System.Collections.Generic;
using CivSandbox.Simulation;
using CivSandbox.World;

namespace CivSandbox.People
{
    public sealed class WorldSimulation
    {
        public const int PersonCount = 24;
        public const int ActorFixedTicksPerSecond = 20;
        public const int CarryCapacityUnits = 4;
        public const int GatherDurationTicks = 60;
        public const int ShelterTimberRequired = 60;
        public const int ShelterStoneRequired = 30;
        public const int ShelterWorkRequired = 24000;
        public const int MaximumResourceNodesPerCommodity = 24;
        public const int MaximumResourceCandidatesPerDecision = 24;
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
        private readonly List<ResourceNodeState> resourceNodes = new List<ResourceNodeState>();
        private readonly SimulationClock clock = new SimulationClock();
        private readonly StockpileState stockpile = new StockpileState();
        private GeneratedWorld generatedWorld;
        private GeneratedWorldCell foundingCell;
        private bool isFoundedCamp;
        private ulong nextLocalId;
        private WorldSeed seed;
        private long actorFixedTicks;
        private int shelterTimberCommitted;
        private int shelterStoneCommitted;
        private int shelterWorkCompleted;

        public WorldSimulation(ulong seedValue)
        {
            Bounds = new WorldBounds(-22000, 22000, -22000, 22000);
            Reset(seedValue);
        }

        public WorldSimulation(GeneratedWorld world, GeneratedWorldCell site, ulong seedValue)
        {
            generatedWorld = world ?? throw new ArgumentNullException(nameof(world));
            if (!world.IsFoundingSite(site))
            {
                throw new ArgumentException("A camp simulation requires a valid founding site.", nameof(site));
            }

            foundingCell = site;
            isFoundedCamp = true;
            Bounds = new WorldBounds(
                -site.Column * 1000,
                (world.Columns - 1 - site.Column) * 1000,
                -site.Row * 1000,
                (world.Rows - 1 - site.Row) * 1000);
            Reset(seedValue);
        }

        public WorldBounds Bounds { get; private set; }
        public WorldTime Time => clock.Time;
        public WorldSeed Seed => seed;

        public void Reset(ulong seedValue)
        {
            seed = new WorldSeed(seedValue);
            clock.Reset();
            actorFixedTicks = 0;
            nextLocalId = 1;
            shelterTimberCommitted = 0;
            shelterStoneCommitted = 0;
            shelterWorkCompleted = 0;
            stockpile.Clear();
            resourceNodes.Clear();

            ulong worldKey = KeyedRandom.Mix(seedValue ^ 0x776f726c642d3031UL);
            int givenOffset = KeyedRandom.Range(seedValue, InitialPeopleStream, 0, 0, 0, 0, GivenNames.Length);
            int familyOffset = KeyedRandom.Range(seedValue, InitialPeopleStream, 0, 0, 1, 0, FamilyNames.Length);
            for (int index = 0; index < people.Length; index++)
            {
                ulong localId = nextLocalId++;
                int east = isFoundedCamp
                    ? KeyedRandom.Range(seedValue, InitialPeopleStream, localId, 0, 2, -3500, 3501)
                    : KeyedRandom.Range(
                        seedValue, InitialPeopleStream, localId, 0, 2,
                        Bounds.MinimumEastMillimeters + 2000, Bounds.MaximumEastMillimeters - 1999);
                int north = isFoundedCamp
                    ? KeyedRandom.Range(seedValue, InitialPeopleStream, localId, 0, 3, -3500, 3501)
                    : KeyedRandom.Range(
                        seedValue, InitialPeopleStream, localId, 0, 3,
                        Bounds.MinimumNorthMillimeters + 2000, Bounds.MaximumNorthMillimeters - 1999);
                string name = GivenNames[(givenOffset + index) % GivenNames.Length] + " " +
                              FamilyNames[(familyOffset + index * 7) % FamilyNames.Length];
                int appearance = KeyedRandom.Range(seedValue, InitialPeopleStream, localId, 0, 4, 0, 12);
                var person = new PersonState(
                    new StableEntityId(worldKey, localId),
                    name,
                    new WorldPosition(east, north),
                    appearance)
                {
                    IdleTicksRemaining = KeyedRandom.Range(seedValue, MovementStream, localId, 0, 0, 0, 21),
                    NutritionUnits = KeyedRandom.Range(
                        seedValue, SurvivalNeedsStream, localId, 0, 0, 7800, SurvivalNeedsSnapshot.CapacityUnits + 1),
                    HydrationUnits = KeyedRandom.Range(
                        seedValue, SurvivalNeedsStream, localId, 0, 1, 7000, SurvivalNeedsSnapshot.CapacityUnits + 1),
                    NeedReviewOffsetSeconds = KeyedRandom.Range(
                        seedValue, SurvivalNeedsStream, localId, 0, 2, 0, NeedReviewSeconds)
                };
                people[index] = person;
            }

            if (isFoundedCamp)
            {
                CreateReachableResourceNodes(worldKey);
            }
        }

        public void AdvanceFixedWallTick(SimulationSpeed speed)
        {
            clock.AdvanceFixedWallTick(speed);
            int actorSteps = speed.Multiplier();
            for (int step = 0; step < actorSteps; step++)
            {
                actorFixedTicks++;
                if (isFoundedCamp)
                {
                    AdvanceCampFixedTick();
                }
                else if (actorFixedTicks % ActorFixedTicksPerSecond == 0)
                {
                    AdvanceLegacyOneSecond(actorFixedTicks / ActorFixedTicksPerSecond);
                }
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
                    person.Reason,
                    needs,
                    person.AppearanceVariant,
                    person.CarriedCommodity,
                    person.CarriedUnits);
            }

            return new WorldSnapshot(seed, clock.Time, Bounds, copy, CreateCampSnapshot());
        }

        public ulong ComputeChecksum()
        {
            CanonicalChecksum checksum = CanonicalChecksum.Create();
            checksum.Add("CIV-BUILD02-FOUNDING-CAMP-v2");
            checksum.Add(seed.Value);
            checksum.Add(clock.Time.Seconds);
            checksum.Add(clock.CalendarSubsecondTicks);
            checksum.Add(actorFixedTicks);
            checksum.Add(nextLocalId);
            checksum.Add((byte)(isFoundedCamp ? 1 : 0));
            checksum.Add(isFoundedCamp ? foundingCell.Id.World : 0UL);
            checksum.Add(isFoundedCamp ? foundingCell.Id.Local : 0UL);
            checksum.Add(Bounds.MinimumEastMillimeters);
            checksum.Add(Bounds.MaximumEastMillimeters);
            checksum.Add(Bounds.MinimumNorthMillimeters);
            checksum.Add(Bounds.MaximumNorthMillimeters);
            checksum.Add(stockpile.WaterUnits);
            checksum.Add(stockpile.FoodUnits);
            checksum.Add(stockpile.TimberUnits);
            checksum.Add(stockpile.StoneUnits);
            checksum.Add(shelterTimberCommitted);
            checksum.Add(shelterStoneCommitted);
            checksum.Add(shelterWorkCompleted);

            for (int index = 0; index < people.Length; index++)
            {
                PersonState person = people[index];
                checksum.Add(person.Id.World);
                checksum.Add(person.Id.Local);
                checksum.Add(person.Name);
                checksum.Add(person.Position.EastMillimeters);
                checksum.Add(person.Position.NorthMillimeters);
                checksum.Add((byte)person.Action);
                checksum.Add((byte)person.Reason);
                checksum.Add(person.AppearanceVariant);
                checksum.Add(person.DecisionOrdinal);
                checksum.Add(person.IdleTicksRemaining);
                checksum.Add(person.MoveStart.EastMillimeters);
                checksum.Add(person.MoveStart.NorthMillimeters);
                checksum.Add(person.MoveTarget.EastMillimeters);
                checksum.Add(person.MoveTarget.NorthMillimeters);
                checksum.Add(person.MoveDurationTicks);
                checksum.Add(person.MoveElapsedTicks);
                checksum.Add(person.NutritionUnits);
                checksum.Add(person.HydrationUnits);
                checksum.Add(person.NeedReviewOffsetSeconds);
                checksum.Add((byte)person.CarriedCommodity);
                checksum.Add(person.CarriedUnits);
                checksum.Add(person.TargetResourceIndex);
                checksum.Add(person.ReservedUnits);
                checksum.Add(person.ActionTicksRemaining);
                checksum.Add((byte)(person.IsMoving ? 1 : 0));
            }

            for (int index = 0; index < resourceNodes.Count; index++)
            {
                ResourceNodeState node = resourceNodes[index];
                checksum.Add(node.Id.World);
                checksum.Add(node.Id.Local);
                checksum.Add(node.SourceCellId.World);
                checksum.Add(node.SourceCellId.Local);
                checksum.Add((byte)node.Commodity);
                checksum.Add(node.Position.EastMillimeters);
                checksum.Add(node.Position.NorthMillimeters);
                checksum.Add(node.AvailableUnits);
                checksum.Add(node.ReservedUnits);
            }

            return checksum.Value;
        }

        private CampSnapshot CreateCampSnapshot()
        {
            if (!isFoundedCamp) return CampSnapshot.Empty;
            var nodes = new ResourceNodeSnapshot[resourceNodes.Count];
            for (int index = 0; index < resourceNodes.Count; index++)
            {
                ResourceNodeState node = resourceNodes[index];
                nodes[index] = new ResourceNodeSnapshot(
                    node.Id,
                    node.SourceCellId,
                    node.Position,
                    node.Commodity,
                    node.AvailableUnits,
                    node.ReservedUnits);
            }

            return new CampSnapshot(
                true,
                foundingCell.Id,
                stockpile.CreateSnapshot(),
                new ShelterSnapshot(
                    ShelterTimberRequired,
                    ShelterStoneRequired,
                    shelterTimberCommitted,
                    shelterStoneCommitted,
                    shelterWorkCompleted,
                    ShelterWorkRequired),
                nodes);
        }

        private void AdvanceCampFixedTick()
        {
            if (actorFixedTicks % ActorFixedTicksPerSecond == 0)
            {
                long actorSecond = actorFixedTicks / ActorFixedTicksPerSecond;
                for (int index = 0; index < people.Length; index++)
                {
                    AdvanceNeeds(people[index], actorSecond);
                }
            }

            for (int index = 0; index < people.Length; index++)
            {
                AdvanceCampPerson(people[index]);
            }

            TryCommitShelterMaterials();
        }

        private void AdvanceCampPerson(PersonState person)
        {
            switch (person.Action)
            {
                case PersonAction.SeekingResource:
                    if (AdvanceMove(person)) BeginGathering(person);
                    return;
                case PersonAction.Gathering:
                    AdvanceGathering(person);
                    return;
                case PersonAction.Hauling:
                    if (AdvanceMove(person)) DepositCarriedResource(person);
                    return;
                case PersonAction.Drinking:
                case PersonAction.Eating:
                    AdvanceConsumption(person);
                    return;
                case PersonAction.BuildingShelter:
                    AdvanceBuilding(person);
                    return;
                case PersonAction.Waiting:
                    if (person.IdleTicksRemaining > 0)
                    {
                        person.IdleTicksRemaining--;
                        return;
                    }

                    DecideCampWork(person);
                    return;
                default:
                    person.Action = PersonAction.Waiting;
                    person.Reason = PersonActionReason.RestingBetweenWalks;
                    return;
            }
        }

        private void DecideCampWork(PersonState person)
        {
            person.DecisionOrdinal++;
            if (TryBeginConsumption(person, CampCommodity.Water) || TryBeginConsumption(person, CampCommodity.Food))
            {
                return;
            }

            if (shelterTimberCommitted >= ShelterTimberRequired && shelterStoneCommitted >= ShelterStoneRequired)
            {
                if (shelterWorkCompleted < ShelterWorkRequired)
                {
                    person.Action = PersonAction.BuildingShelter;
                    person.Reason = PersonActionReason.ShelterMaterialsReady;
                    BeginMove(person, default);
                }
                else
                {
                    person.Action = PersonAction.Waiting;
                    person.Reason = PersonActionReason.ShelterComplete;
                    person.IdleTicksRemaining = 40;
                }

                return;
            }

            int start = (int)((person.Id.Local + person.DecisionOrdinal) % 4UL);
            for (int offset = 0; offset < 4; offset++)
            {
                CampCommodity commodity = (CampCommodity)(1 + (start + offset) % 4);
                if (NeedsMore(commodity) && TryReserveNearestResource(person, commodity))
                {
                    return;
                }
            }

            person.Action = PersonAction.Waiting;
            person.IdleTicksRemaining = 40;
            person.Reason = MissingResourceReason();
        }

        private bool TryBeginConsumption(PersonState person, CampCommodity commodity)
        {
            bool needsSupply = commodity == CampCommodity.Water
                ? person.HydrationUnits <= 6000
                : person.NutritionUnits <= 6000;
            if (!needsSupply || stockpile.Amount(commodity) <= 0)
            {
                return false;
            }

            person.Action = commodity == CampCommodity.Water ? PersonAction.Drinking : PersonAction.Eating;
            person.Reason = commodity == CampCommodity.Water
                ? PersonActionReason.RestoringHydration
                : PersonActionReason.RestoringNutrition;
            person.ActionTicksRemaining = ActorFixedTicksPerSecond;
            BeginMove(person, default);
            return true;
        }

        private void AdvanceConsumption(PersonState person)
        {
            if (person.IsMoving && !AdvanceMove(person)) return;
            if (person.ActionTicksRemaining > 0)
            {
                person.ActionTicksRemaining--;
                return;
            }

            CampCommodity commodity = person.Action == PersonAction.Drinking
                ? CampCommodity.Water
                : CampCommodity.Food;
            if (stockpile.TryTake(commodity, 1))
            {
                if (commodity == CampCommodity.Water)
                {
                    person.HydrationUnits = Math.Min(SurvivalNeedsSnapshot.CapacityUnits, person.HydrationUnits + 1200);
                }
                else
                {
                    person.NutritionUnits = Math.Min(SurvivalNeedsSnapshot.CapacityUnits, person.NutritionUnits + 900);
                }
            }

            SetBriefWait(person);
        }

        private bool NeedsMore(CampCommodity commodity)
        {
            int target;
            switch (commodity)
            {
                case CampCommodity.Water: target = 48; break;
                case CampCommodity.Food: target = 48; break;
                case CampCommodity.Timber: target = shelterTimberCommitted >= ShelterTimberRequired ? 0 : ShelterTimberRequired; break;
                case CampCommodity.Stone: target = shelterStoneCommitted >= ShelterStoneRequired ? 0 : ShelterStoneRequired; break;
                default: return false;
            }

            if (target == 0) return false;
            return stockpile.Amount(commodity) + PipelineUnits(commodity) < target;
        }

        private int PipelineUnits(CampCommodity commodity)
        {
            int total = 0;
            for (int index = 0; index < people.Length; index++)
            {
                PersonState person = people[index];
                if (person.CarriedCommodity == commodity) total += person.CarriedUnits;
                if (person.TargetResourceIndex >= 0 &&
                    resourceNodes[person.TargetResourceIndex].Commodity == commodity)
                {
                    total += person.ReservedUnits;
                }
            }

            return total;
        }

        private bool TryReserveNearestResource(PersonState person, CampCommodity commodity)
        {
            int bestIndex = -1;
            long bestDistance = long.MaxValue;
            int inspected = 0;
            for (int index = 0; index < resourceNodes.Count && inspected < MaximumResourceCandidatesPerDecision; index++)
            {
                ResourceNodeState node = resourceNodes[index];
                if (node.Commodity != commodity) continue;
                inspected++;
                if (node.AvailableUnits - node.ReservedUnits <= 0) continue;
                long east = (long)node.Position.EastMillimeters - person.Position.EastMillimeters;
                long north = (long)node.Position.NorthMillimeters - person.Position.NorthMillimeters;
                long distance = east * east + north * north;
                if (distance < bestDistance || distance == bestDistance && node.Id.Local < resourceNodes[bestIndex].Id.Local)
                {
                    bestDistance = distance;
                    bestIndex = index;
                }
            }

            if (bestIndex < 0) return false;
            ResourceNodeState chosen = resourceNodes[bestIndex];
            int reserved = Math.Min(CarryCapacityUnits, chosen.AvailableUnits - chosen.ReservedUnits);
            chosen.ReservedUnits += reserved;
            person.TargetResourceIndex = bestIndex;
            person.ReservedUnits = reserved;
            person.Action = PersonAction.SeekingResource;
            person.Reason = NeedReason(commodity);
            BeginMove(person, chosen.Position);
            return true;
        }

        private void BeginGathering(PersonState person)
        {
            person.Action = PersonAction.Gathering;
            person.Reason = PersonActionReason.GatheringReservedSource;
            person.ActionTicksRemaining = GatherDurationTicks;
        }

        private void AdvanceGathering(PersonState person)
        {
            if (person.ActionTicksRemaining > 0)
            {
                person.ActionTicksRemaining--;
                return;
            }

            if (person.TargetResourceIndex < 0 || person.TargetResourceIndex >= resourceNodes.Count)
            {
                SetBriefWait(person);
                return;
            }

            ResourceNodeState node = resourceNodes[person.TargetResourceIndex];
            int gathered = Math.Min(person.ReservedUnits, node.AvailableUnits);
            node.AvailableUnits -= gathered;
            node.ReservedUnits = Math.Max(0, node.ReservedUnits - person.ReservedUnits);
            person.ReservedUnits = 0;
            person.TargetResourceIndex = -1;
            if (gathered <= 0)
            {
                person.Reason = PersonActionReason.ResourceSourceDepleted;
                SetBriefWait(person, false);
                return;
            }

            person.CarriedCommodity = node.Commodity;
            person.CarriedUnits = gathered;
            person.Action = PersonAction.Hauling;
            person.Reason = PersonActionReason.CarryingToSharedStockpile;
            BeginMove(person, default);
        }

        private void DepositCarriedResource(PersonState person)
        {
            stockpile.Add(person.CarriedCommodity, person.CarriedUnits);
            person.CarriedCommodity = CampCommodity.None;
            person.CarriedUnits = 0;
            SetBriefWait(person);
        }

        private void TryCommitShelterMaterials()
        {
            if (shelterTimberCommitted >= ShelterTimberRequired && shelterStoneCommitted >= ShelterStoneRequired)
            {
                return;
            }

            if (stockpile.TimberUnits < ShelterTimberRequired || stockpile.StoneUnits < ShelterStoneRequired)
            {
                return;
            }

            stockpile.TryTake(CampCommodity.Timber, ShelterTimberRequired);
            stockpile.TryTake(CampCommodity.Stone, ShelterStoneRequired);
            shelterTimberCommitted = ShelterTimberRequired;
            shelterStoneCommitted = ShelterStoneRequired;
        }

        private void AdvanceBuilding(PersonState person)
        {
            if (person.IsMoving && !AdvanceMove(person)) return;
            if (shelterWorkCompleted < ShelterWorkRequired)
            {
                shelterWorkCompleted++;
                return;
            }

            person.Reason = PersonActionReason.ShelterComplete;
            SetBriefWait(person, false);
        }

        private void BeginMove(PersonState person, WorldPosition target)
        {
            person.MoveStart = person.Position;
            person.MoveTarget = target;
            person.MoveElapsedTicks = 0;
            int eastDistance = Math.Abs(target.EastMillimeters - person.Position.EastMillimeters);
            int northDistance = Math.Abs(target.NorthMillimeters - person.Position.NorthMillimeters);
            int distance = Math.Max(eastDistance, northDistance);
            const int speedMillimetersPerFixedTick = 70;
            person.MoveDurationTicks = Math.Max(1, (distance + speedMillimetersPerFixedTick - 1) / speedMillimetersPerFixedTick);
            person.IsMoving = distance > 0;
        }

        private static bool AdvanceMove(PersonState person)
        {
            if (!person.IsMoving) return true;
            person.MoveElapsedTicks++;
            if (person.MoveElapsedTicks >= person.MoveDurationTicks)
            {
                person.Position = person.MoveTarget;
                person.IsMoving = false;
                return true;
            }

            long elapsed = person.MoveElapsedTicks;
            long duration = person.MoveDurationTicks;
            int east = person.MoveStart.EastMillimeters +
                       (int)(((long)person.MoveTarget.EastMillimeters - person.MoveStart.EastMillimeters) * elapsed / duration);
            int north = person.MoveStart.NorthMillimeters +
                        (int)(((long)person.MoveTarget.NorthMillimeters - person.MoveStart.NorthMillimeters) * elapsed / duration);
            person.Position = new WorldPosition(east, north);
            return false;
        }

        private static void SetBriefWait(PersonState person, bool resetReason = true)
        {
            person.Action = PersonAction.Waiting;
            if (resetReason) person.Reason = PersonActionReason.RestingBetweenWalks;
            person.IdleTicksRemaining = 10;
            person.IsMoving = false;
        }

        private static PersonActionReason NeedReason(CampCommodity commodity)
        {
            switch (commodity)
            {
                case CampCommodity.Water: return PersonActionReason.WaterStockLow;
                case CampCommodity.Food: return PersonActionReason.FoodStockLow;
                case CampCommodity.Timber: return PersonActionReason.TimberNeededForShelter;
                default: return PersonActionReason.StoneNeededForShelter;
            }
        }

        private PersonActionReason MissingResourceReason()
        {
            if (NeedsMore(CampCommodity.Water) && !HasAvailable(CampCommodity.Water)) return PersonActionReason.WaterUnavailable;
            if (NeedsMore(CampCommodity.Food) && !HasAvailable(CampCommodity.Food)) return PersonActionReason.FoodUnavailable;
            return PersonActionReason.ResourceSourceDepleted;
        }

        private bool HasAvailable(CampCommodity commodity)
        {
            for (int index = 0; index < resourceNodes.Count; index++)
            {
                ResourceNodeState node = resourceNodes[index];
                if (node.Commodity == commodity && node.AvailableUnits - node.ReservedUnits > 0) return true;
            }

            return false;
        }

        private static void AdvanceNeeds(PersonState person, long actorSecond)
        {
            if (actorSecond % NeedReviewSeconds != person.NeedReviewOffsetSeconds) return;
            person.NutritionUnits = Math.Max(0, person.NutritionUnits - NutritionLossPerReview);
            person.HydrationUnits = Math.Max(0, person.HydrationUnits - HydrationLossPerReview);
        }

        private void AdvanceLegacyOneSecond(long actorSecond)
        {
            for (int index = 0; index < people.Length; index++)
            {
                PersonState person = people[index];
                AdvanceNeeds(person, actorSecond);
                AdvanceLegacyPerson(person);
            }
        }

        private void AdvanceLegacyPerson(PersonState person)
        {
            if (person.Action == PersonAction.Waiting)
            {
                if (person.IdleTicksRemaining > 0)
                {
                    person.IdleTicksRemaining--;
                    return;
                }

                ulong occurrence = person.DecisionOrdinal++;
                int margin = 1500;
                int targetEast = KeyedRandom.Range(
                    seed.Value, MovementStream, person.Id.Local, occurrence, 1,
                    Bounds.MinimumEastMillimeters + margin, Bounds.MaximumEastMillimeters - margin + 1);
                int targetNorth = KeyedRandom.Range(
                    seed.Value, MovementStream, person.Id.Local, occurrence, 2,
                    Bounds.MinimumNorthMillimeters + margin, Bounds.MaximumNorthMillimeters - margin + 1);
                person.Action = PersonAction.Walking;
                person.Reason = PersonActionReason.ExploringBoundedArea;
                BeginMoveLegacy(person, new WorldPosition(targetEast, targetNorth), occurrence);
                return;
            }

            person.MoveElapsedTicks++;
            if (person.MoveElapsedTicks >= person.MoveDurationTicks)
            {
                person.Position = person.MoveTarget;
                person.Action = PersonAction.Waiting;
                person.Reason = PersonActionReason.RestingBetweenWalks;
                person.IdleTicksRemaining = KeyedRandom.Range(
                    seed.Value, MovementStream, person.Id.Local, person.DecisionOrdinal, 4, 30, 151);
                return;
            }

            long elapsed = person.MoveElapsedTicks;
            long duration = person.MoveDurationTicks;
            person.Position = new WorldPosition(
                person.MoveStart.EastMillimeters +
                (int)(((long)person.MoveTarget.EastMillimeters - person.MoveStart.EastMillimeters) * elapsed / duration),
                person.MoveStart.NorthMillimeters +
                (int)(((long)person.MoveTarget.NorthMillimeters - person.MoveStart.NorthMillimeters) * elapsed / duration));
        }

        private void BeginMoveLegacy(PersonState person, WorldPosition target, ulong occurrence)
        {
            int speedMillimetersPerActorSecond = KeyedRandom.Range(
                seed.Value, MovementStream, person.Id.Local, occurrence, 3, 16, 25);
            person.MoveStart = person.Position;
            person.MoveTarget = target;
            person.MoveElapsedTicks = 0;
            int distance = Math.Max(
                Math.Abs(target.EastMillimeters - person.Position.EastMillimeters),
                Math.Abs(target.NorthMillimeters - person.Position.NorthMillimeters));
            person.MoveDurationTicks = Math.Max(1, (distance + speedMillimetersPerActorSecond - 1) / speedMillimetersPerActorSecond);
        }

        private void CreateReachableResourceNodes(ulong worldKey)
        {
            AddResourceNodes(worldKey, CampCommodity.Water);
            AddResourceNodes(worldKey, CampCommodity.Food);
            AddResourceNodes(worldKey, CampCommodity.Timber);
            AddResourceNodes(worldKey, CampCommodity.Stone);
        }

        private void AddResourceNodes(ulong worldKey, CampCommodity commodity)
        {
            var candidates = new List<ResourceCandidate>();
            int radius = GeneratedWorld.FoundingClearanceCells;
            for (int row = foundingCell.Row - radius; row <= foundingCell.Row + radius; row++)
            {
                for (int column = foundingCell.Column - radius; column <= foundingCell.Column + radius; column++)
                {
                    GeneratedWorldCell cell = generatedWorld.CellAt(column, row);
                    if (cell.IsWater) continue;
                    int abundance = ResourceAmount(cell, commodity);
                    if (abundance <= 0) continue;
                    int east = (column - foundingCell.Column) * 1000;
                    int north = (row - foundingCell.Row) * 1000;
                    int distance = Math.Max(Math.Abs(east), Math.Abs(north));
                    candidates.Add(new ResourceCandidate(cell.Id, new WorldPosition(east, north), abundance, distance));
                }
            }

            candidates.Sort(ResourceCandidate.Compare);
            int count = Math.Min(MaximumResourceNodesPerCommodity, candidates.Count);
            for (int index = 0; index < count; index++)
            {
                ResourceCandidate candidate = candidates[index];
                int capacity = 6 + candidate.AbundancePermille / 25;
                resourceNodes.Add(new ResourceNodeState(
                    new StableEntityId(worldKey, nextLocalId++),
                    candidate.CellId,
                    candidate.Position,
                    commodity,
                    capacity));
            }
        }

        private static int ResourceAmount(GeneratedWorldCell cell, CampCommodity commodity)
        {
            switch (commodity)
            {
                case CampCommodity.Water: return cell.Resources.FreshWaterPermille;
                case CampCommodity.Food: return Math.Max(cell.Resources.StapleFoodPermille, cell.Resources.ProteinFoodPermille);
                case CampCommodity.Timber: return cell.Resources.TimberPermille;
                case CampCommodity.Stone: return cell.Resources.StonePermille;
                default: return 0;
            }
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
                Reason = PersonActionReason.RestingBetweenWalks;
                MoveStart = position;
                MoveTarget = position;
                TargetResourceIndex = -1;
            }

            public StableEntityId Id { get; }
            public string Name { get; }
            public int AppearanceVariant { get; }
            public WorldPosition Position { get; set; }
            public PersonAction Action { get; set; }
            public PersonActionReason Reason { get; set; }
            public ulong DecisionOrdinal { get; set; }
            public int IdleTicksRemaining { get; set; }
            public WorldPosition MoveStart { get; set; }
            public WorldPosition MoveTarget { get; set; }
            public int MoveDurationTicks { get; set; }
            public int MoveElapsedTicks { get; set; }
            public bool IsMoving { get; set; }
            public int NutritionUnits { get; set; }
            public int HydrationUnits { get; set; }
            public int NeedReviewOffsetSeconds { get; set; }
            public CampCommodity CarriedCommodity { get; set; }
            public int CarriedUnits { get; set; }
            public int TargetResourceIndex { get; set; }
            public int ReservedUnits { get; set; }
            public int ActionTicksRemaining { get; set; }
        }

        private sealed class ResourceNodeState
        {
            public ResourceNodeState(
                StableEntityId id,
                StableEntityId sourceCellId,
                WorldPosition position,
                CampCommodity commodity,
                int availableUnits)
            {
                Id = id;
                SourceCellId = sourceCellId;
                Position = position;
                Commodity = commodity;
                AvailableUnits = availableUnits;
            }

            public StableEntityId Id { get; }
            public StableEntityId SourceCellId { get; }
            public WorldPosition Position { get; }
            public CampCommodity Commodity { get; }
            public int AvailableUnits { get; set; }
            public int ReservedUnits { get; set; }
        }

        private readonly struct ResourceCandidate
        {
            public ResourceCandidate(StableEntityId cellId, WorldPosition position, int abundancePermille, int distance)
            {
                CellId = cellId;
                Position = position;
                AbundancePermille = abundancePermille;
                Distance = distance;
            }

            public StableEntityId CellId { get; }
            public WorldPosition Position { get; }
            public int AbundancePermille { get; }
            public int Distance { get; }

            public static int Compare(ResourceCandidate left, ResourceCandidate right)
            {
                int distance = left.Distance.CompareTo(right.Distance);
                return distance != 0 ? distance : left.CellId.Local.CompareTo(right.CellId.Local);
            }
        }

        private sealed class StockpileState
        {
            public int WaterUnits { get; private set; }
            public int FoodUnits { get; private set; }
            public int TimberUnits { get; private set; }
            public int StoneUnits { get; private set; }

            public int Amount(CampCommodity commodity)
            {
                switch (commodity)
                {
                    case CampCommodity.Water: return WaterUnits;
                    case CampCommodity.Food: return FoodUnits;
                    case CampCommodity.Timber: return TimberUnits;
                    case CampCommodity.Stone: return StoneUnits;
                    default: return 0;
                }
            }

            public void Add(CampCommodity commodity, int units)
            {
                if (units <= 0) return;
                switch (commodity)
                {
                    case CampCommodity.Water: WaterUnits = checked(WaterUnits + units); break;
                    case CampCommodity.Food: FoodUnits = checked(FoodUnits + units); break;
                    case CampCommodity.Timber: TimberUnits = checked(TimberUnits + units); break;
                    case CampCommodity.Stone: StoneUnits = checked(StoneUnits + units); break;
                }
            }

            public bool TryTake(CampCommodity commodity, int units)
            {
                if (units <= 0 || Amount(commodity) < units) return false;
                switch (commodity)
                {
                    case CampCommodity.Water: WaterUnits -= units; break;
                    case CampCommodity.Food: FoodUnits -= units; break;
                    case CampCommodity.Timber: TimberUnits -= units; break;
                    case CampCommodity.Stone: StoneUnits -= units; break;
                }

                return true;
            }

            public CampStockpileSnapshot CreateSnapshot()
            {
                return new CampStockpileSnapshot(WaterUnits, FoodUnits, TimberUnits, StoneUnits);
            }

            public void Clear()
            {
                WaterUnits = 0;
                FoodUnits = 0;
                TimberUnits = 0;
                StoneUnits = 0;
            }
        }
    }
}
