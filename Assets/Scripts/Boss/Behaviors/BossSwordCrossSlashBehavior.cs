using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordCrossSlashBehavior : BossBehavior
{
    [Header("Sword Cross Slash")] 
    [SerializeField] private BossSwordCrossSlashData m_BossSwordCrossSlashData;
    
    public override void Detect(BossBehaviorManager bbm)
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!m_ShowGizmos || m_BossSwordCrossSlashData == null)
            return;
        
        
    }
}
