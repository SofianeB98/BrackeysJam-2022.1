using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossEarthquakeData", menuName = "Boss/Behaviors Data/New BossEarthquakeData", order = 0)]
public class BossEarthquakeData : ScriptableObject
{
    [Header("Stats")]
    public float Damage = 15.0f;

    [Header("Detection")] 
    public float DetectionRadius = 5.0f;
    public Vector3 DetectionPositionOffset = Vector3.zero;

    [Header("FX")] 
    public GameObject VFX;
    public Vector3 PositionOffset;
}
