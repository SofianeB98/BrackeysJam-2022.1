using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController : MonoBehaviour
{
    [SerializeField] private FSMState m_InitialState;
    [SerializeField] private FSMState m_RemainingState;
    
    private FSMState m_CurrentState;
    private bool m_IsActive = false;

    private void Start()
    {
        m_IsActive = true;
        m_CurrentState = m_InitialState;
    }

    private void Update()
    {
        if (!m_IsActive)
            return;

        if (m_CurrentState == null)
            return;
        
        m_CurrentState.UpdateState(this);
    }

    public void TransitionToState(FSMState targetState)
    {
        if (targetState == null || (m_RemainingState != null && targetState == m_RemainingState))
            return;

        // Exit current state
        
        m_CurrentState = targetState;
        
        // Enter new current state
    }
    
    private void OnDrawGizmos()
    {
        if (m_CurrentState == null)
            return;

        Gizmos.color = m_CurrentState.m_DebugColor;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
