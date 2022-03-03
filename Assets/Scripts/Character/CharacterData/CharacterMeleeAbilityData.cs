using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAbilityData", menuName = "Character/Ability/New Melee Ability Data", order = 0)]
public class CharacterMeleeAbilityData : ScriptableObject
{
    [Header("Melee")]
    public float Damage = 10.0f;
    public float AdditionnalDamageComboPercent = 15f;
    public float DelayBeforeExitCombo = 0.5f;
    public float DelayAfterEndCombo = 0.5f;

    [Header("Movement")]
    public float SpeedMovement;
    public float MoveTime;
    
    [Header("Detection")] 
    public float DetectionRadius = 0.25f;
    public Vector3 DetectionPositionOffset = Vector3.forward;
}
