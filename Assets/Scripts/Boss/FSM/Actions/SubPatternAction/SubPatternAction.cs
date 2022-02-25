using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SubPatternActionState
{
    PERFORMED,
    ENDED
}

public abstract class SubPatternAction : ScriptableObject
{
    public abstract void OnEnter(FSMController fsmController);

    public abstract SubPatternActionState Execute(FSMController fsmController);

    public abstract void OnEnd(FSMController fsmController);

}
