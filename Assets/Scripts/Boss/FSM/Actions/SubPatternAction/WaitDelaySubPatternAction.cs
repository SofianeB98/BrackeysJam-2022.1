using UnityEngine;

[CreateAssetMenu(fileName = "WaitDelaySubPatternAction", menuName = "FSM/FSM Pattern Actions/New WaitDelaySubPatternAction", order = 0)]
public class WaitDelaySubPatternAction : SubPatternAction
{
    [SerializeField] private float m_DelayToWaitInSeconds = 2.0f;
    private float m_CurrentDelay = 0f;
    
    public override void OnEnter(FSMController fsmController)
    {
        m_CurrentDelay = Time.time + m_DelayToWaitInSeconds;
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (m_CurrentDelay < Time.time)
            return SubPatternActionState.ENDED;
        
        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        m_CurrentDelay = 0f;
    }
}
