using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FSMTransition
{
    public FSMDecision[] Conditions;
    [Tooltip("if condition return true, this state will be applied")] public FSMState TrueState;
    [Tooltip("if condition return false, this state will be applied")] public FSMState FalseState;

    public bool GetConditionsFilled(FSMController fsmController)
    {
        // foreach (var c in Conditions)
        // {
        //     if (!c.Decide(fsmController))
        //         return false;
        // }

        return Conditions.All(x => x.Decide(fsmController));
    }
}
