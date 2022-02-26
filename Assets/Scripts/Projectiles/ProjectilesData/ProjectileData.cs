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
    
    [Header("Detection")]
    public Vector3 DetectionSize = Vector3.one;
    public Vector3 DetectionPointOffset = Vector3.zero;
    
    [Header("DONT TOUCH THESE FIELDS !")]
    [Tooltip("in seconds")] public float LifeTime = 1.0f;
    
    private void OnValidate()
    {
        if (Speed > 0.0f && DistanceToReach > 0.0f)
            LifeTime = Mathf.Abs(DistanceToReach / Speed);
    }
}
