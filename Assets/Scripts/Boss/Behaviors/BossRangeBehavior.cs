using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossRangeBehavior : MonoBehaviour
{
    [Header("Range Parameters")]
    [SerializeField] protected Projectile m_LineWavePrefab;
    [SerializeField] protected float m_PercentChanceNotReal = 10.0f;
    [SerializeField] protected Vector3 m_PositionOffset = Vector3.zero;

    public void Detect(Transform t, float damage)
    {
        if (!t.TryGetComponent(out Health hp))
            return;
        
        hp .ReduceHealth(damage);
    }
    
    public abstract void TriggerRange();
}
