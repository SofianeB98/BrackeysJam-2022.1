using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossMeleeAtkData", menuName = "Boss/Behaviors Data/New BossMeleeAtkData", order = 0)]
public class BossMeleeAtkData : ScriptableObject
{
    [Header("Stats")] 
    public float Damage = 5f;
}
