using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour
{
    public UnityEvent DeathEvent = null;
    public HealthEvent HealthUpdatedEvent = null;
    
    [SerializeField] private float m_StartingHealth = 100f;

    [Space(5)] 
    [SerializeField] protected AudioSource m_AudioSource;
    [SerializeField] protected AudioClip[] m_HittedClips;
    
    public float StartingHealth
    {
        get { return m_StartingHealth; }
    }
    
    private float m_CurrentHealth;

    public float CurrentHealth
    {
        get { return m_CurrentHealth; }
    }

    private bool m_IsDead = false;
    public bool IsDead => m_IsDead;
    
    private void Awake()
    {
        m_CurrentHealth = m_StartingHealth;
    }

    private void Start()
    {
        HealthUpdatedEvent?.Invoke(this);
    }

    public virtual void ReduceHealth(float amount)
    {
        if (m_IsDead)
            return;
        
        m_CurrentHealth -= amount;
        HealthUpdatedEvent?.Invoke(this);

        if (m_CurrentHealth <= 0)
        {
            Death();
        }
        else if (m_AudioSource != null && m_HittedClips != null && m_HittedClips.Length > 0)
        {
            m_AudioSource.PlayOneShot(m_HittedClips[Random.Range(0, m_HittedClips.Length)]);
        }
        
    }

    private void Death()
    {
        m_IsDead = true;

        Animator anim = GetComponentInChildren<Animator>() ?? GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("IsDeadTrigger");
            anim.SetBool("IsDead", true);
        }
        
        DeathEvent?.Invoke();
    }
}

[Serializable]
public class HealthEvent : UnityEvent<Health>
{
    
}