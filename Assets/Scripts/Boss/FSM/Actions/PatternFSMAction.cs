using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFSMAction : FSMAction
{
    [SerializeField] private SubPatternAction[] m_SubPatternActions;
    private SubPatternAction m_CurrentAction = null;
    private int m_CurrentIndex = 0;
    
    public override void Act(FSMController fsmController)
    {
        if (fsmController.CurrentState.TriggerTransition)
            return;
        
        if (m_CurrentAction == null)
        {
            m_CurrentAction = m_SubPatternActions[m_CurrentIndex];
            m_CurrentAction.OnEnter();
            return;
        }

        var state = m_CurrentAction.Execute(fsmController);
        if (state == SubPatternActionState.ENDED)
        {
            m_CurrentAction.OnEnd();
            m_CurrentAction = null;
            m_CurrentIndex++;
        }

        if (m_CurrentIndex >= m_SubPatternActions.Length)
        {
            fsmController.CurrentState.TriggerTransition = true;
        }
    }
}
