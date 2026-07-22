namespace CivSandbox.People
{
    public enum NeedUrgency : byte
    {
        Safe = 0,
        Concern = 1,
        Urgent = 2,
        Critical = 3
    }

    public readonly struct SurvivalNeedsSnapshot
    {
        public const int CapacityUnits = 10000;

        public SurvivalNeedsSnapshot(int nutritionUnits, int hydrationUnits)
        {
            NutritionUnits = Clamp(nutritionUnits);
            HydrationUnits = Clamp(hydrationUnits);
        }

        public int NutritionUnits { get; }

        public int HydrationUnits { get; }

        public int NutritionPercent => (NutritionUnits * 100 + CapacityUnits / 2) / CapacityUnits;

        public int HydrationPercent => (HydrationUnits * 100 + CapacityUnits / 2) / CapacityUnits;

        public NeedUrgency NutritionUrgency => Classify(NutritionUnits);

        public NeedUrgency HydrationUrgency => Classify(HydrationUnits);

        public NeedUrgency HighestUrgency => NutritionUrgency > HydrationUrgency ? NutritionUrgency : HydrationUrgency;

        public static NeedUrgency Classify(int units)
        {
            if (units <= 1500) return NeedUrgency.Critical;
            if (units <= 3500) return NeedUrgency.Urgent;
            if (units <= 6000) return NeedUrgency.Concern;
            return NeedUrgency.Safe;
        }

        private static int Clamp(int value)
        {
            if (value < 0) return 0;
            return value > CapacityUnits ? CapacityUnits : value;
        }
    }
}
