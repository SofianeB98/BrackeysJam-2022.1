using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent DeathEvent = null;
    public HealthEvent HealthUpdatedEvent = null;

    [SerializeField] private float m_StartingHealth = 100f;
    
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
            Death();
    }

    private void Death()
    {
        m_IsDead = true;

        Animator anim = GetComponentInChildren<Animator>() ?? GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("IsDead", m_IsDead);
        
        DeathEvent?.Invoke();
    }
}

[Serializable]
public class HealthEvent : UnityEvent<Health>
{
    
}