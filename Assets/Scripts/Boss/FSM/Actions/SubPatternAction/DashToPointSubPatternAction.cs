using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashToPointSubPatternAction", menuName = "FSM/FSM Pattern Actions/New DashToPointSubPatternAction", order = 0)]
public class DashToPointSubPatternAction : SubPatternAction
{
    [Header("Dash Data")] 
    [SerializeField] private Vector3 m_PointToReach = Vector3.zero;
    
    private float m_DashDuration = 1.0f;
    private Vector3 m_StartPos = Vector3.zero;
    private float m_CurrentDuration = 0f;
    private Vector3 m_Direction = Vector3.forward;
    private float m_CurrentLoadDuration = 0f;
    private float m_Distance = 0f;
    private Vector3 m_NewPos = Vector3.zero;
    
    public override void OnEnter(FSMController fsmController)
    {
        var dashData = fsmController.Boss.DashData;
        m_CurrentLoadDuration = Time.time + dashData.DashLoadDuration;
        m_StartPos = fsmController.Boss.transform.position;
        m_Direction = m_PointToReach - m_StartPos;
        m_Direction.y = 0f;
        m_Distance = m_Direction.magnitude;
        m_Direction.Normalize();
        m_DashDuration = m_Distance / dashData.DashSpeed;
        m_CurrentDuration = 0f;
        m_NewPos = m_StartPos;
        fsmController.Boss.transform.rotation = Quaternion.LookRotation(m_Direction, Vector3.up);
        fsmController.Boss.DashVFX.SetActive(true);
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (m_CurrentDuration >= m_DashDuration || m_Distance <= 0.1f)
            return SubPatternActionState.ENDED;
        
        if (m_CurrentLoadDuration > Time.time)
            return SubPatternActionState.PERFORMED;

        m_CurrentDuration += Time.deltaTime;
        m_NewPos += m_Direction * (fsmController.Boss.DashData.DashSpeed * Time.deltaTime);
        if (Vector3.Distance(m_NewPos, m_StartPos) > m_Distance)
        {
            m_NewPos = m_PointToReach;
            m_CurrentDuration = m_DashDuration + 0.1f;
        }
        fsmController.transform.position = m_NewPos;

        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        m_CurrentDuration = 0f;
        fsmController.Boss.DashVFX.SetActive(false);
    }
}
