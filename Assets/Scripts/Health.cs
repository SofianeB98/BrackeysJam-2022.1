using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent DeathEvent = null;
    
    [SerializeField] private float m_StartingHealth = 100f;
    private float m_CurrentHealth;

    private bool m_IsDead = false;
    
    private void Awake()
    {
        m_CurrentHealth = m_StartingHealth;
    }

    public virtual void ReduceHealth(float amount)
    {
        if (m_IsDead)
            return;
        
        m_CurrentHealth -= amount;
        if (m_CurrentHealth <= 0)
            Death();
    }

    private void Death()
    {
        m_IsDead = true;
        DeathEvent?.Invoke();
    }
}
