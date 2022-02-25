using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController : MonoBehaviour
{
    [Header("FSM")]
    [SerializeField] private FSMState m_InitialState;
    [SerializeField] private FSMState m_RemainingState;
    private FSMState m_CurrentState;
    public FSMState CurrentState => m_CurrentState;
    private bool m_IsActive = false;

    [Header("IA Behavior")]
    [SerializeField] private BossBehaviorManager m_Boss;
    public BossBehaviorManager Boss => m_Boss;
    
    private void Start()
    {
        m_IsActive = true;
        TransitionToState(m_InitialState);
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

        if (m_CurrentState != null)
            m_CurrentState.TriggerTransition = false;
        
        m_CurrentState = targetState;
        m_CurrentState.OnEnterState(this);
    }
    
    private void OnDrawGizmos()
    {
        if (m_CurrentState == null)
            return;

        Gizmos.color = m_CurrentState.m_DebugColor;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
