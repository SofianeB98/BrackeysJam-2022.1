using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashToPointSubPatternAction : SubPatternAction
{
    [Header("Dash Data")] 
    [SerializeField] private Vector3 m_PointToReach = Vector3.zero;
    [Tooltip("in Unity Unit/s"), SerializeField] private float m_DashSpeed = 5.0f;
    [Tooltip("in seconds"), SerializeField] private float m_DashLoadDuration = 1.0f;
    [Tooltip("in seconds"), SerializeField] private float m_DashDuration = 1.0f;
    
    public override void OnEnter(FSMController fsmController)
    {
        
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        
    }
}
