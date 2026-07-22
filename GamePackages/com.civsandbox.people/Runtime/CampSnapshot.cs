using System;
using System.Collections;
using System.Collections.Generic;
using CivSandbox.Simulation;
using CivSandbox.World;

namespace CivSandbox.People
{
    public enum CampCommodity : byte
    {
        None = 0,
        Water = 1,
        Food = 2,
        Timber = 3,
        Stone = 4
    }

    public readonly struct CampStockpileSnapshot
    {
        public CampStockpileSnapshot(int waterUnits, int foodUnits, int timberUnits, int stoneUnits)
        {
            WaterUnits = waterUnits;
            FoodUnits = foodUnits;
            TimberUnits = timberUnits;
            StoneUnits = stoneUnits;
        }

        public int WaterUnits { get; }
        public int FoodUnits { get; }
        public int TimberUnits { get; }
        public int StoneUnits { get; }

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
    }

    public readonly struct ShelterSnapshot
    {
        public ShelterSnapshot(
            int timberRequired,
            int stoneRequired,
            int timberCommitted,
            int stoneCommitted,
            int workCompleted,
            int workRequired)
        {
            TimberRequired = timberRequired;
            StoneRequired = stoneRequired;
            TimberCommitted = timberCommitted;
            StoneCommitted = stoneCommitted;
            WorkCompleted = workCompleted;
            WorkRequired = workRequired;
        }

        public int TimberRequired { get; }
        public int StoneRequired { get; }
        public int TimberCommitted { get; }
        public int StoneCommitted { get; }
        public int WorkCompleted { get; }
        public int WorkRequired { get; }
        public bool MaterialsCommitted => TimberCommitted >= TimberRequired && StoneCommitted >= StoneRequired;
        public bool IsComplete => MaterialsCommitted && WorkCompleted >= WorkRequired;
        public int ProgressPercent => WorkRequired <= 0 ? 100 : Math.Min(100, WorkCompleted * 100 / WorkRequired);
    }

    public readonly struct ResourceNodeSnapshot
    {
        public ResourceNodeSnapshot(
            StableEntityId id,
            StableEntityId sourceCellId,
            WorldPosition position,
            CampCommodity commodity,
            int availableUnits,
            int reservedUnits)
        {
            Id = id;
            SourceCellId = sourceCellId;
            Position = position;
            Commodity = commodity;
            AvailableUnits = availableUnits;
            ReservedUnits = reservedUnits;
        }

        public StableEntityId Id { get; }
        public StableEntityId SourceCellId { get; }
        public WorldPosition Position { get; }
        public CampCommodity Commodity { get; }
        public int AvailableUnits { get; }
        public int ReservedUnits { get; }
        public int UnreservedUnits => Math.Max(0, AvailableUnits - ReservedUnits);
        public bool IsDepleted => AvailableUnits <= 0;
    }

    public sealed class CampSnapshot : IReadOnlyList<ResourceNodeSnapshot>
    {
        private static readonly ResourceNodeSnapshot[] NoResources = new ResourceNodeSnapshot[0];
        public static CampSnapshot Empty { get; } = new CampSnapshot(
            false,
            default,
            new CampStockpileSnapshot(0, 0, 0, 0),
            new ShelterSnapshot(0, 0, 0, 0, 0, 0),
            NoResources);

        internal CampSnapshot(
            bool isFounded,
            StableEntityId foundingCellId,
            CampStockpileSnapshot stockpile,
            ShelterSnapshot shelter,
            ResourceNodeSnapshot[] resources)
        {
            IsFounded = isFounded;
            FoundingCellId = foundingCellId;
            Stockpile = stockpile;
            Shelter = shelter;
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        private readonly ResourceNodeSnapshot[] resources;
        public bool IsFounded { get; }
        public StableEntityId FoundingCellId { get; }
        public CampStockpileSnapshot Stockpile { get; }
        public ShelterSnapshot Shelter { get; }
        public int Count => resources.Length;
        public ResourceNodeSnapshot this[int index] => resources[index];

        public int Remaining(CampCommodity commodity)
        {
            int total = 0;
            for (int index = 0; index < resources.Length; index++)
            {
                if (resources[index].Commodity == commodity)
                {
                    total += resources[index].AvailableUnits;
                }
            }

            return total;
        }

        public IEnumerator<ResourceNodeSnapshot> GetEnumerator()
        {
            for (int index = 0; index < resources.Length; index++) yield return resources[index];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
