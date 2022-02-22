using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityState
{
    NONE,
    CAN_NOT_PERFORM_ABILITY,
    MELEE, 
    RANGE,
    ULTIMATE
}

public class CharacterAbility : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterInput m_CharacterInput = null;
    [SerializeField] private CharacterAiming m_CharacterAiming = null;
    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    
    [Header("Melee Ability")] 
    [SerializeField] private CharacterMeleeAbilityData m_MeleeAbilityData;
    
    [Header("Range Ability")]
    [SerializeField] private CharacterRangeAbilityData m_RangeAbilityData;

    private AbilityState m_CurrantAbilityState = AbilityState.NONE;
    
    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterInput == null)
            m_CharacterInput = GetComponent<CharacterInput>();
    }
    
    private void Start()
    {
        if (m_CameraReferential == null)
            m_CameraReferential = Camera.main.transform; 
    }
    
    private void OnEnable()
    {
        m_CharacterInput.RangeAbilityEvent += TriggerRangeAbility;
        m_CharacterInput.MeleeAbilityEvent += TriggerMeleeAbility;
        m_CharacterInput.UltimateAbilityEvent += TriggerUltimateAbility;
    }

    private void OnDisable()
    {
        m_CharacterInput.RangeAbilityEvent -= TriggerRangeAbility;
        m_CharacterInput.MeleeAbilityEvent -= TriggerMeleeAbility;
        m_CharacterInput.UltimateAbilityEvent -= TriggerUltimateAbility;
    }

    private void Update()
    {
        switch (m_CurrantAbilityState)
        {
            case AbilityState.NONE:
            case AbilityState.CAN_NOT_PERFORM_ABILITY:
                return;
            case AbilityState.MELEE:
                // Some stuff
                break;
            case AbilityState.RANGE:
                InvokeRepeating(nameof(RangeAbility), 0.2f, 0.5f);
                break;
            case AbilityState.ULTIMATE:
                break;
        }
    }

    private void RangeAbility()
    {
        
    }
    
    private void CancelCurrentAction()
    {
        switch (m_CurrantAbilityState)
        {
            case AbilityState.NONE:
            case AbilityState.CAN_NOT_PERFORM_ABILITY:
                break;
            case AbilityState.MELEE:
                break;
            case AbilityState.RANGE:
                break;
            case AbilityState.ULTIMATE:
                break;
        }
    }

    #region Callbacks

    private void TriggerCanNotPerformAbility(bool canPerform)
    {
        CancelCurrentAction();
        m_CurrantAbilityState = canPerform ? AbilityState.NONE : AbilityState.CAN_NOT_PERFORM_ABILITY;
    }
    
    private void TriggerMeleeAbility()
    {
        if (m_CurrantAbilityState == AbilityState.CAN_NOT_PERFORM_ABILITY)
            return;
        
        Debug.Log("Melee Ability !!");
        
        CancelCurrentAction();
        m_CurrantAbilityState = AbilityState.MELEE;
    }

    private void TriggerRangeAbility(bool isPerformed)
    {
        if (m_CurrantAbilityState == AbilityState.CAN_NOT_PERFORM_ABILITY)
            return;
        
        Debug.Log("Range Ability !! " + isPerformed);
        
        CancelCurrentAction();
        m_CurrantAbilityState = AbilityState.RANGE;
    }

    private void TriggerUltimateAbility()
    {
        if (m_CurrantAbilityState == AbilityState.CAN_NOT_PERFORM_ABILITY)
            return;
        
        Debug.Log("Ultimate Ability");
        
        CancelCurrentAction();
        m_CurrantAbilityState = AbilityState.ULTIMATE;
    }
    
    #endregion
}
