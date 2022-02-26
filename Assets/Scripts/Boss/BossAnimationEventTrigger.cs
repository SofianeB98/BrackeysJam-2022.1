using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationEventTrigger : MonoBehaviour
{
    [SerializeField] private BossBehaviorManager m_BossBehaviorManager;

    private void Awake()
    {
        if (m_BossBehaviorManager == null)
            m_BossBehaviorManager = GetComponentInParent<BossBehaviorManager>();
    }
    
    #region AnimationCallbacks

    public void TriggerEarthquake()
    {
        m_BossBehaviorManager.TriggerEarthquake();
    }

    public void TriggerMeleeAttack()
    {
        m_BossBehaviorManager.TriggerMeleeAttack();
    }

    public void TriggerSwordCrossSlash(int slashIndex)
    {
        Debug.Log("Slash !! --> " + slashIndex);
        m_BossBehaviorManager.TriggerSwordCrossSlash(slashIndex);
    }
    
    #endregion
    
}
