using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RotateToTargetSubPatternAction", menuName = "FSM/FSM Pattern Actions/New RotateToTargetSubPatternAction", order = 0)]
public class RotateToTargetSubPatternAction : SubPatternAction
{
    [SerializeField] private float m_TimeToReach = 1.0f;
    [SerializeField] private int m_RepeatCount = 1;
    [SerializeField] private float m_DelayBeforeRepeat = 0f;

    private Vector3 m_TargetPosition = Vector3.zero;
    private float m_DegreesToRotate = 90.0f;
    private float m_CurrentDegree = 0f;
    private int m_CurrentCount = 0;
    private float m_CurrentDelayBeforeRepeat = 0f;

    private void OnValidate()
    {
        if (m_TimeToReach <= Mathf.Epsilon)
            m_TimeToReach = 0.01f;
    }

    public override void OnEnter(FSMController fsmController)
    {
        m_CurrentCount = 0;
        m_CurrentDegree = 0f;
        m_CurrentDelayBeforeRepeat = 0f;

        m_TargetPosition = fsmController.Boss.Target.position;
        m_DegreesToRotate = Quaternion.LookRotation((m_TargetPosition - fsmController.Boss.transform.position).normalized, Vector3.up).eulerAngles.y;
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (m_CurrentCount >= m_RepeatCount)
            return SubPatternActionState.ENDED;

        if (m_CurrentDelayBeforeRepeat > Time.time)
            return SubPatternActionState.PERFORMED;
        
        float rotationDelta = Time.deltaTime * m_DegreesToRotate / m_TimeToReach;
        m_CurrentDegree = Mathf.MoveTowards(m_CurrentDegree, m_DegreesToRotate, rotationDelta);
        
        fsmController.Boss.transform.Rotate(Vector3.up, rotationDelta);
        
        if (Mathf.Approximately(m_CurrentDegree, m_DegreesToRotate))
        {
            m_CurrentCount++;
            m_CurrentDegree = 0f;
            m_CurrentDelayBeforeRepeat = Time.time + m_DelayBeforeRepeat;
        }
        
        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        m_CurrentCount = 0;
        m_CurrentDegree = 0f;
    }
}
