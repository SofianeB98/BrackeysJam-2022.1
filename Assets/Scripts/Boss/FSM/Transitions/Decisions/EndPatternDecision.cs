using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EndPatternDecision", menuName = "FSM/FSM Decisions/New End Pattern Decision", order = 0)]
public class EndPatternDecision : FSMDecision
{
    public override bool Decide(FSMController fsmController)
    {
        return fsmController.CurrentState.TriggerTransition;
    }
}
