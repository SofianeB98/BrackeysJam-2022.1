using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMAction : ScriptableObject
{
    public abstract void ResetAction(FSMController fsmController);
    public abstract void Act(FSMController fsmController);
}
