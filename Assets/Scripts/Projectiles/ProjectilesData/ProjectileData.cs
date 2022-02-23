using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Projectile/New Projectile Data", order = 0)]
public class ProjectileData : ScriptableObject
{
    [Header("Projectile")]
    public float Damage = 25.0f;
    public float DistanceToReach = 20.0f;
    public float Speed = 15.0f;
    public float DetectionRadius = 2.0f;
    public float ProjectileHeight = 2.0f;
    
    [Header("DONT TOUCH THESE FIELDS !")]
    [Tooltip("in seconds")] public float LifeTime = 1.0f;
    
    private void OnValidate()
    {
        if (Speed > 0.0f && DistanceToReach > 0.0f)
            LifeTime = DistanceToReach / Speed;
    }
}
