namespace CivSandbox.People
{
    public enum PersonAction : byte
    {
        Waiting = 0,
        Walking = 1,
        SeekingResource = 2,
        Gathering = 3,
        Hauling = 4,
        Drinking = 5,
        Eating = 6,
        BuildingShelter = 7
    }

    public enum PersonActionReason : byte
    {
        RestingBetweenWalks = 0,
        ExploringBoundedArea = 1,
        WaterUnavailable = 2,
        FoodUnavailable = 3,
        WaterStockLow = 4,
        FoodStockLow = 5,
        TimberNeededForShelter = 6,
        StoneNeededForShelter = 7,
        GatheringReservedSource = 8,
        CarryingToSharedStockpile = 9,
        RestoringHydration = 10,
        RestoringNutrition = 11,
        ShelterMaterialsReady = 12,
        ShelterComplete = 13,
        ResourceSourceDepleted = 14
    }
}
