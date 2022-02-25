using System;
using UnityEngine;


    public abstract class BossBehavior : MonoBehaviour
    {
        [SerializeField] protected LayerMask m_AffectedLayer;
        
        [Header("Debug", order = 999)] 
        [SerializeField] protected bool m_ShowGizmos = true;
        [SerializeField] protected Color m_DebugColor = Color.red;
        
        public abstract void Detect(BossBehaviorManager bbm);
    }
