using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeData", menuName = "Character/Ability/New Melee Data", order = 0)]
public class CharacterMeleeData : ScriptableObject
{
    public float Damage = 10.0f;
}
