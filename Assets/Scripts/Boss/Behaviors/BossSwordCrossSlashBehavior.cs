using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwordCrossSlashBehavior : BossBehavior
{
    [Header("Sword Cross Slash")] 
    [SerializeField] private BossSwordCrossSlashData m_BossSwordCrossSlashData;
    
    [Header("Gizmo")] 
    [SerializeField] private Mesh m_CubeMesh;

    private bool m_SfxPlayed = false;
    
    public override void Detect(BossBehaviorManager bbm)
    {
        int idx = bbm.SlashIndex;
        if (idx >= 2)
        {
            Debug.LogWarning("Please, set the slash index as 0 or 1");
            return;
        }

        if (!m_SfxPlayed)
        {
            m_AudioSource?.PlayOneShot(m_Sfx);
            m_SfxPlayed = true;
        }

        if (idx == 1)
            m_SfxPlayed = false;

        if (m_BossSwordCrossSlashData.SlashFxParameters != null &&
            m_BossSwordCrossSlashData.SlashFxParameters.Length > 1)
        {
            var fx = m_BossSwordCrossSlashData.SlashFxParameters[idx];
            var go = Instantiate(fx.VFX, transform.position + (transform.rotation * fx.PositionOffset), transform.rotation);
            Destroy(go, 2.0f);
        }
        
        var slash = m_BossSwordCrossSlashData.SlashDetectionParameters[idx];
        var cols = Physics.OverlapBox(transform.position + (transform.rotation * slash.m_OffsetPosition), slash.m_SlashBoxDetection * 0.5f, transform.rotation, m_AffectedLayer);
        foreach (var c in cols)
        {
            if (!c.TryGetComponent(out Health hp))
                continue;
            
            hp.ReduceHealth(idx == 0 ? m_BossSwordCrossSlashData.DamageFirstSlash : m_BossSwordCrossSlashData.DamageSecondSlash);
            break;
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_ShowGizmos || m_BossSwordCrossSlashData == null)
            return;
        
        foreach (var slash in m_BossSwordCrossSlashData.SlashDetectionParameters)
        {
            Gizmos.color = m_DebugColor;
            if (m_CubeMesh != null)
                Gizmos.DrawWireMesh(m_CubeMesh, transform.position + (transform.rotation * slash.m_OffsetPosition), transform.rotation, slash.m_SlashBoxDetection);

        }
    }
}
