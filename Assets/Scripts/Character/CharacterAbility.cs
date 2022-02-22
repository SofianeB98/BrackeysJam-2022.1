using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CharacterInput m_CharacterInput = null;
    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    
    [Header("Melee Ability")] 
    [SerializeField] private CharacterMeleeData m_MeleeData;
    
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

    #region Callbacks

    private void TriggerMeleeAbility()
    {
        Debug.Log("Melee Ability !!");
    }

    private void TriggerRangeAbility(bool isPerformed)
    {
        Debug.Log("Range Ability !! " + isPerformed);
    }

    private void TriggerUltimateAbility()
    {
        Debug.Log("Ultimate Ability");
    }
    
    #endregion
}
