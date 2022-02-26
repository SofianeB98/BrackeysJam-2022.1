using System;

public static class CharacterEvents
{
    // Ability Events
    public static Action<float> TriggerInvicible;
    public static Action<bool> DashStateUpdate;
    public static Action<bool> UpdateCanMoveEvent;

    public static Action<int> ProjectileAdded;
    public static Action<int> ProjectileRemoved;

}
