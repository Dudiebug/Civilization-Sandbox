namespace CivSandbox.People
{
    public enum UpperGarment : byte { LinenShirt, LinenShift, WoolWaistcoat, LacedBodice, WorkJacket, LongCoat }
    public enum LowerGarment : byte { KneeBreeches, Petticoat, WorkSkirt, TiedApron }
    public enum OuterGarment : byte { None, HoodedCloak, ShoulderShawl }
    public enum Headwear : byte { None, LinenCap, Bonnet, BroadFeltHat, CockedHat }
    public enum Footwear : byte { BuckleShoes, AnkleBoots }

    public readonly struct ClothingAppearance
    {
        public ClothingAppearance(UpperGarment upper, LowerGarment lower, OuterGarment outer, Headwear headwear, Footwear footwear, byte upperColor, byte lowerColor, byte outerColor)
        {
            Upper = upper;
            Lower = lower;
            Outer = outer;
            Headwear = headwear;
            Footwear = footwear;
            UpperColor = upperColor;
            LowerColor = lowerColor;
            OuterColor = outerColor;
        }

        public UpperGarment Upper { get; }
        public LowerGarment Lower { get; }
        public OuterGarment Outer { get; }
        public Headwear Headwear { get; }
        public Footwear Footwear { get; }
        public byte UpperColor { get; }
        public byte LowerColor { get; }
        public byte OuterColor { get; }
    }
}
