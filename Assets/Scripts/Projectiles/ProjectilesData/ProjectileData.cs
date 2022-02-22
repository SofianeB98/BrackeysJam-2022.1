using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Projectile/New Projectile Data", order = 0)]
public class ProjectileData : ScriptableObject
{
    public float Damage = 25.0f;
    public float LifeTime = 1.0f;
    public float Speed = 15.0f;
    public float DetectionRadius = 2.0f;
    public float ProjectileHeight = 2.0f;
}
