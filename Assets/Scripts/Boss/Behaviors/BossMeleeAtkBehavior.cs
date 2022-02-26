using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAtkBehavior : BossBehavior
{
    [Header("Melee Atk")] 
    [SerializeField] private BossMeleeAtkData m_BossMeleeAtkData;
    
    public override void Detect(BossBehaviorManager bbm)
    {
        var cols = Physics.OverlapSphere(transform.position + (transform.rotation * m_BossMeleeAtkData.DetectionPositionOffset),
            m_BossMeleeAtkData.DetectionRadius, m_AffectedLayer);

        m_AudioSource?.PlayOneShot(m_Sfx);
        
        foreach (var c in cols)
        {
            if (!c.TryGetComponent(out Health hp))
                continue;
            
            hp.ReduceHealth(m_BossMeleeAtkData.Damage);
            break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_ShowGizmos || m_BossMeleeAtkData == null)
            return;

        Gizmos.color = m_DebugColor;
        Gizmos.DrawWireSphere(transform.position+ (transform.rotation * m_BossMeleeAtkData.DetectionPositionOffset), m_BossMeleeAtkData.DetectionRadius);
    }
}
