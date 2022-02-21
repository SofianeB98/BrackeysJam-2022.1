using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Character/New Movement Data", order = 0)]
public class CharacterMovementData : ScriptableObject
{
    [Header("Base Speed")]
    [Tooltip("in Unity Unit/s")] public float Speed = 5.0f;
    [Tooltip("in seconds")] public float AccelDuration = 1.0f;
    [Tooltip("in seconds")] public float DecelDuration = 1.0f;

    [Header("Super Speed")] 
    [Tooltip("in Unity Unit/s")] public float SuperSpeed = 25.0f;
    [Tooltip("in seconds")] public float SuperSpeedUnlockTimer = 1.0f;
    [Tooltip("in seconds")] public float DecelSuperSpeedDuration = 5.0f;
}
