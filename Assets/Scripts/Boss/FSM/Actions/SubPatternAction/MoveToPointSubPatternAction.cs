using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToPointSubPatternAction", menuName = "FSM/FSM Pattern Actions/New MoveToPointSubPatternAction", order = 0)]
public class MoveToPointSubPatternAction : SubPatternAction
{
    [SerializeField] private Vector3 m_PointToReach = Vector3.zero;
    [SerializeField] private float m_DistanceThreshold = 1.0f;
    
    public override void OnEnter(FSMController fsmController)
    {
        fsmController.Boss.Agent.enabled = true;
        fsmController.Boss.Agent.SetDestination(m_PointToReach);
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (Vector3.Distance(fsmController.Boss.transform.position, m_PointToReach) <= m_DistanceThreshold)
            return SubPatternActionState.ENDED;

        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        fsmController.Boss.Agent.ResetPath();
        fsmController.Boss.Agent.enabled = false;
    }
}
