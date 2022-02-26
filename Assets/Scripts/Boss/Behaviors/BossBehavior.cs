using System;
using UnityEngine;


    public abstract class BossBehavior : MonoBehaviour
    {
        [SerializeField] protected LayerMask m_AffectedLayer;
        
        [Header("Debug", order = 999)] 
        [SerializeField] protected bool m_ShowGizmos = true;
        [SerializeField] protected Color m_DebugColor = Color.red;

        [Header("SFX")] 
        [SerializeField] protected AudioSource m_AudioSource;
        [SerializeField] protected AudioClip m_Sfx;

        private void Awake()
        {
            if (m_AudioSource == null)
                m_AudioSource = GetComponent<AudioSource>();
        }

        public abstract void Detect(BossBehaviorManager bbm);
    }
