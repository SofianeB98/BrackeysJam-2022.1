using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeAbilityData", menuName = "Character/Ability/New Range Ability Data", order = 0)]
public class CharacterRangeAbilityData : ScriptableObject
{
    public Vector3 LaunchPositionOffset = Vector3.zero;
    public float DelayBetweenShoot = 0.5f;
}
