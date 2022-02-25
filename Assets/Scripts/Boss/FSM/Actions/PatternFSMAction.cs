using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatternFSMAction", menuName = "FSM/FSM Actions/New PatternFSMAction", order = 0)]
public class PatternFSMAction : FSMAction
{
    [SerializeField] private SubPatternAction[] m_SubPatternActions;
    private SubPatternAction m_CurrentAction = null;
    private int m_CurrentIndex = 0;

    public override void ResetAction(FSMController fsmController)
    {
        m_CurrentIndex = 0;
        m_CurrentAction = null;
    }

    public override void Act(FSMController fsmController)
    {
        if (fsmController.CurrentState.TriggerTransition) return;
        
        if (m_CurrentAction == null)
        {
            m_CurrentAction = m_SubPatternActions[m_CurrentIndex];
            m_CurrentAction.OnEnter(fsmController);
            return;
        }

        var state = m_CurrentAction.Execute(fsmController);
        if (state == SubPatternActionState.ENDED)
        {
            m_CurrentAction.OnEnd(fsmController);
            m_CurrentAction = null;
            m_CurrentIndex++;

            if (m_CurrentIndex >= m_SubPatternActions.Length)
            {
                fsmController.CurrentState.TriggerTransition = true;
                
            }
        }
    }
}
