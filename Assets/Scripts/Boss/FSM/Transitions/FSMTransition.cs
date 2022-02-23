using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FSMTransition
{
    public FSMDecision Condition;
    [Tooltip("if condition return true, this state will be applied")] public FSMState TrueState;
    [Tooltip("if condition return false, this state will be applied")] public FSMState FalseState;
}
