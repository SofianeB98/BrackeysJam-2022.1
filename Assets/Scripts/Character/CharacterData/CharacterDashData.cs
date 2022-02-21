using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DashData", menuName = "Character/New Dash Data", order = 1)]
public class CharacterDashData : ScriptableObject
{
    [Header("Dash")]
    public float DashSpeed = 5.0f;
    public float DashDistance = 1.0f;
    public float DashDuration = 1.0f;
    public float DashReloadDuration = 1.0f;
    public float InvincibleDuration = 1.0f;
    public float DashRecoveryDuration = 1.0f;
    
    private void OnValidate()
    {
        if (DashSpeed > 0.0f && DashDistance > 0.0f)
            DashDuration = DashDistance / DashSpeed;
    }
}
