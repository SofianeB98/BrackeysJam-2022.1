using System;

public static class CharacterEvents
{
    public static Action<float> TriggerInvicible;
    public static Action<bool> DashStateUpdate;
    public static Action<bool> UpdateCanMoveEvent;
}
