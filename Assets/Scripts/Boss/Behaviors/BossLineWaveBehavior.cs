using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLineWaveBehavior : BossRangeBehavior
{
    public override void TriggerRange()
    {
        var proj = Instantiate(m_LineWavePrefab, transform.position + (transform.rotation * m_PositionOffset),
            transform.rotation);
        
        var rdm = Random.Range(0.0f, 100.01f);
        if (m_PercentChanceNotReal <= rdm)
        {
            proj.SetNotReal(true);
        }

        proj.CollisionDetectedEvent += Detect;
    }
}
