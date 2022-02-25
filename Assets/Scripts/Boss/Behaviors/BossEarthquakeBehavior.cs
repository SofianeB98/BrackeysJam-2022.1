using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthquakeBehavior : BossBehavior
{
    [Header("Earthquake")] 
    [SerializeField] private BossEarthquakeData m_BossEarthquakeData;

    public override void Detect(BossBehaviorManager bbm)
    {
        if (m_BossEarthquakeData == null)
        {
            Debug.LogWarning("Earthquake data is null, please fill it !");
            return;
        }
        
        var cols = Physics.OverlapSphere(transform.position + (transform.rotation * m_BossEarthquakeData.DetectionPositionOffset),
            m_BossEarthquakeData.DetectionRadius, m_AffectedLayer);

        foreach (var c in cols)
        {
            if (!c.TryGetComponent(out Health hp))
                continue;
            
            hp.ReduceHealth(m_BossEarthquakeData.Damage);
            break;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!m_ShowGizmos || m_BossEarthquakeData == null)
            return;

        Gizmos.color = m_DebugColor;
        Gizmos.DrawWireSphere(transform.position + (transform.rotation * m_BossEarthquakeData.DetectionPositionOffset), m_BossEarthquakeData.DetectionRadius);
    }
}
