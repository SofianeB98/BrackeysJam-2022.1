using System;
using System.Collections;
using UnityEngine;

public class CharacterHealth : Health
{
    private bool m_IsInvincible = false;
    private float m_InvincibleTime = 0f;

    private void OnEnable()
    {
        CharacterEvents.TriggerInvicible += TriggerInvincible;
        
    }

    private void OnDisable()
    {
        CharacterEvents.TriggerInvicible -= TriggerInvincible;
    }

    private void Update()
    {
        if (!m_IsInvincible)
            return;
        
        if (m_InvincibleTime > Time.time)
            return;
        
        m_IsInvincible = false;
    }

    public override void ReduceHealth(float amount)
    {
        if (m_IsInvincible)
            return;

        base.ReduceHealth(amount);
    }

    public void OnDie()
    {
        CharacterEvents.HeroDieEvent.Invoke();
    }
    
    #region Callbacks

    private void TriggerInvincible(float val)
    {
        m_IsInvincible = true;
        m_InvincibleTime = Time.time + val;
    }

    #endregion
}