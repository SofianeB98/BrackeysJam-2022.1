using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEventTrigger : MonoBehaviour
{
    [SerializeField] private CharacterAbility m_CharacterAbility;
    [SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private CharacterAiming m_CharacterAiming;

    private void Awake()
    {
        if (m_CharacterAbility == null)
            m_CharacterAbility = GetComponentInParent<CharacterAbility>();
        
        if (m_CharacterMovement == null)
            m_CharacterMovement = GetComponentInParent<CharacterMovement>();
        
        if (m_CharacterAiming == null)
            m_CharacterAiming = GetComponentInParent<CharacterAiming>();
    }

    public void DetectMeleeCollision()
    {
        m_CharacterAbility.DetectMeleeCollision();
    }

    public void TriggerExitComboDelay()
    {
        m_CharacterAbility.TriggerExitComboDelay();
    }

    public void TriggerEndCombo()
    {
        m_CharacterAbility.TriggerEndCombo();
    }

    public void TriggerAskedNextAttack()
    {
        m_CharacterAbility.TriggerAskedNextAttack();
    }
    
    public void LaunchProjectile()
    {
        m_CharacterAbility.LaunchProjectile();
    }
}
