using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestFSMAction", menuName = "FSM/FSM Actions/New Test FSM Action", order = 0)]
public class TestFSMAction : FSMAction
{
    public override void Act(FSMController fsmController)
    {
        Debug.Log(nameof(TestFSMAction) + " action !");
    }
}
