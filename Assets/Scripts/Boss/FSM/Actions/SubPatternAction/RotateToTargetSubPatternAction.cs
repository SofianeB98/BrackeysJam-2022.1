using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RotateToTargetSubPatternAction", menuName = "FSM/FSM Pattern Actions/New RotateToTargetSubPatternAction", order = 0)]
public class RotateToTargetSubPatternAction : SubPatternAction
{
    [SerializeField] private float m_SpeedToRotate = 90.0f;
    [SerializeField] private float m_TimeToReach = 1.0f;

    private Quaternion m_TargetRot;
    
    private void OnValidate()
    {
        if (m_TimeToReach <= Mathf.Epsilon)
            m_TimeToReach = 0.01f;
    }

    public override void OnEnter(FSMController fsmController)
    {
        var dir = fsmController.Boss.Target.position - fsmController.Boss.transform.position;
        dir.y = 0;
        m_TargetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        float rotationDelta = Time.deltaTime * m_SpeedToRotate / m_TimeToReach;
        
        fsmController.Boss.transform.rotation = Quaternion.RotateTowards(fsmController.Boss.transform.rotation, m_TargetRot, rotationDelta);
        
        if (Mathf.Abs(Quaternion.Angle(fsmController.Boss.transform.rotation, m_TargetRot)) <= 1f)
        {
            return SubPatternActionState.ENDED;
        }
        
        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        
    }
}
