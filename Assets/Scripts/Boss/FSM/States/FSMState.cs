using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FSMState", menuName = "FSM/New FSM State", order = 0)]
public class FSMState : ScriptableObject
{
    [Header("FSM")]
    [SerializeField] private FSMAction[] m_Actions = new FSMAction[0];
    [SerializeField] private FSMTransition[] m_Transitions = new FSMTransition[0];
    
    [Header("Debug")]
    public Color m_DebugColor = Color.red;

    public void UpdateState(FSMController fsmController)
    {
        ExecuteActions(fsmController);
        CheckTransitions(fsmController);
    }
    
    private void ExecuteActions(FSMController fsmController)
    {
        foreach (var a in m_Actions)
        {
            a.Act(fsmController);
        }
    }

    private void CheckTransitions(FSMController fsmController)
    {
        foreach (var t in m_Transitions)
        {
            fsmController.TransitionToState(t.Condition.Decide(fsmController) ? t.TrueState : t.FalseState);
        }
    }
}
