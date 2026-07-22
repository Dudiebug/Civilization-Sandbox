namespace CivSandbox.People
{
    public enum PersonAction : byte
    {
        Waiting = 0,
        Walking = 1
    }

    public enum PersonActionReason : byte
    {
        RestingBetweenWalks = 0,
        ExploringBoundedArea = 1,
        WaterUnavailable = 2,
        FoodUnavailable = 3
    }
}
