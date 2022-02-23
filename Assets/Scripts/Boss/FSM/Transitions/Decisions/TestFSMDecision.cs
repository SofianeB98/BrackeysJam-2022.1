using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestFSMDecision", menuName = "FSM/FSM Decisions/New Test FSM Decision", order = 0)]
public class TestFSMDecision : FSMDecision
{
    public override bool Decide(FSMController fsmController)
    {
        Debug.Log("DECISION TEST");
        return true;
    }
}
