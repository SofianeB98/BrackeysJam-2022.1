using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "Character/New Movement Data", order = 0)]
public class CharacterMovementData : ScriptableObject
{
    [Header("Movement")]
    public AnimationCurve AccelerationCurve;
    public float Speed = 5.0f;
    
    [Header("Dash")]
    public AnimationCurve AccelerationdDashCurve;
    public float DashSpeed = 10.0f;
}
