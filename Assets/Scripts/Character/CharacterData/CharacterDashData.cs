using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DashData", menuName = "Character/New Dash Data", order = 1)]
public class CharacterDashData : ScriptableObject
{
    [Header("Dash")]
    [Tooltip("in Unity Unit/s")] public float DashSpeed = 5.0f;
    [Tooltip("in Unity Unit")] public float DashDistance = 1.0f;
    [Tooltip("in seconds")] public float DashDuration = 1.0f;
    [Tooltip("in seconds")] public float DashLoadDuration = 1.0f;
    [Tooltip("in seconds")] public float DashRecoveryDuration = 1.0f;
    [Tooltip("in seconds")] public float InvincibleDuration = 1.0f;
    
    private void OnValidate()
    {
        if (DashSpeed > 0.0f && DashDistance > 0.0f)
            DashDuration = DashDistance / DashSpeed;
    }
}
