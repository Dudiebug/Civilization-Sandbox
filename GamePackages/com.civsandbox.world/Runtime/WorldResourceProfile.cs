using System;

namespace CivSandbox.World
{
    public enum WorldResourceKind : byte
    {
        FreshWater = 0,
        StapleFood = 1,
        ProteinFood = 2,
        Timber = 3,
        Stone = 4,
        Clay = 5,
        Fiber = 6,
        IronOre = 7,
        Coal = 8,
        MedicinalInputs = 9
    }

    public readonly struct WorldResourceProfile : IEquatable<WorldResourceProfile>
    {
        public WorldResourceProfile(
            int freshWaterPermille,
            int stapleFoodPermille,
            int proteinFoodPermille,
            int timberPermille,
            int stonePermille,
            int clayPermille,
            int fiberPermille,
            int ironOrePermille,
            int coalPermille,
            int medicinalInputsPermille)
        {
            FreshWaterPermille = Validate(freshWaterPermille, nameof(freshWaterPermille));
            StapleFoodPermille = Validate(stapleFoodPermille, nameof(stapleFoodPermille));
            ProteinFoodPermille = Validate(proteinFoodPermille, nameof(proteinFoodPermille));
            TimberPermille = Validate(timberPermille, nameof(timberPermille));
            StonePermille = Validate(stonePermille, nameof(stonePermille));
            ClayPermille = Validate(clayPermille, nameof(clayPermille));
            FiberPermille = Validate(fiberPermille, nameof(fiberPermille));
            IronOrePermille = Validate(ironOrePermille, nameof(ironOrePermille));
            CoalPermille = Validate(coalPermille, nameof(coalPermille));
            MedicinalInputsPermille = Validate(medicinalInputsPermille, nameof(medicinalInputsPermille));
        }

        public int FreshWaterPermille { get; }
        public int StapleFoodPermille { get; }
        public int ProteinFoodPermille { get; }
        public int TimberPermille { get; }
        public int StonePermille { get; }
        public int ClayPermille { get; }
        public int FiberPermille { get; }
        public int IronOrePermille { get; }
        public int CoalPermille { get; }
        public int MedicinalInputsPermille { get; }

        public int OpportunityPermille => Math.Max(
            Math.Max(Math.Max(FreshWaterPermille, StapleFoodPermille), Math.Max(ProteinFoodPermille, TimberPermille)),
            Math.Max(
                Math.Max(Math.Max(StonePermille, ClayPermille), Math.Max(FiberPermille, IronOrePermille)),
                Math.Max(CoalPermille, MedicinalInputsPermille)));

        public int Amount(WorldResourceKind kind)
        {
            switch (kind)
            {
                case WorldResourceKind.FreshWater: return FreshWaterPermille;
                case WorldResourceKind.StapleFood: return StapleFoodPermille;
                case WorldResourceKind.ProteinFood: return ProteinFoodPermille;
                case WorldResourceKind.Timber: return TimberPermille;
                case WorldResourceKind.Stone: return StonePermille;
                case WorldResourceKind.Clay: return ClayPermille;
                case WorldResourceKind.Fiber: return FiberPermille;
                case WorldResourceKind.IronOre: return IronOrePermille;
                case WorldResourceKind.Coal: return CoalPermille;
                case WorldResourceKind.MedicinalInputs: return MedicinalInputsPermille;
                default: throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        public bool Equals(WorldResourceProfile other)
        {
            return FreshWaterPermille == other.FreshWaterPermille &&
                   StapleFoodPermille == other.StapleFoodPermille &&
                   ProteinFoodPermille == other.ProteinFoodPermille &&
                   TimberPermille == other.TimberPermille &&
                   StonePermille == other.StonePermille &&
                   ClayPermille == other.ClayPermille &&
                   FiberPermille == other.FiberPermille &&
                   IronOrePermille == other.IronOrePermille &&
                   CoalPermille == other.CoalPermille &&
                   MedicinalInputsPermille == other.MedicinalInputsPermille;
        }

        public override bool Equals(object obj) => obj is WorldResourceProfile other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = FreshWaterPermille;
                hash = hash * 397 ^ StapleFoodPermille;
                hash = hash * 397 ^ ProteinFoodPermille;
                hash = hash * 397 ^ TimberPermille;
                hash = hash * 397 ^ StonePermille;
                hash = hash * 397 ^ ClayPermille;
                hash = hash * 397 ^ FiberPermille;
                hash = hash * 397 ^ IronOrePermille;
                hash = hash * 397 ^ CoalPermille;
                return hash * 397 ^ MedicinalInputsPermille;
            }
        }

        private static int Validate(int value, string parameterName)
        {
            if (value < 0 || value > 1000)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Resource abundance uses an inclusive 0-1000 range.");
            }

            return value;
        }
    }
}
