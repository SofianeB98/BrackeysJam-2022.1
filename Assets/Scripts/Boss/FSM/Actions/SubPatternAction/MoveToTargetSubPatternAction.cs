using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToTargetSubPatternAction", menuName = "FSM/FSM Pattern Actions/New MoveToTargetSubPatternAction", order = 0)]
public class MoveToTargetSubPatternAction : SubPatternAction
{
    [SerializeField] private int m_UpdateDestinationEveryNFrame = 15;
    [SerializeField] private float m_DistanceThreshold = 1.0f;
    
    public override void OnEnter(FSMController fsmController)
    {
        fsmController.Boss.Agent.enabled = true;
        fsmController.Boss.Agent.isStopped = false;
        fsmController.Boss.Agent.SetDestination(fsmController.Boss.Target.position);
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (Vector3.Distance(fsmController.Boss.transform.position, fsmController.Boss.Target.position) <= m_DistanceThreshold)
            return SubPatternActionState.ENDED;
        
        if (Time.frameCount % m_UpdateDestinationEveryNFrame == 0)
            fsmController.Boss.Agent.SetDestination(fsmController.Boss.Target.position);

        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        fsmController.Boss.Agent.isStopped = true;
        fsmController.Boss.Agent.ResetPath();
        fsmController.Boss.Agent.enabled = false;
    }
}
