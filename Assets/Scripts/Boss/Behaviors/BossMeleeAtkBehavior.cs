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
        
    }

    private void OnDrawGizmos()
    {
        if (!m_ShowGizmos || m_BossMeleeAtkData == null)
            return;

        Gizmos.color = m_DebugColor;
    }
}
