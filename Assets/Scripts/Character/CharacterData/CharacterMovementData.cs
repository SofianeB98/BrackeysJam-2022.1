using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Character/New Movement Data", order = 0)]
public class CharacterMovementData : ScriptableObject
{
    [Header("Acceleration")]
    public float Speed = 5.0f;
    [Space(2)]
    public float AccelSpeed = 1.0f;
    public float AccelDuration = 1.0f;
    [Space(2)]
    public float DecelSpeed = 1.0f;
    public float DecelDuration = 1.0f;

    [Header("Super Speed")] 
    public float SuperSpeed = 25.0f;
    public float SuperSpeedUnlockTimer = 1.0f;
    public float DecelSuperSpeed = 5.0f;
}
