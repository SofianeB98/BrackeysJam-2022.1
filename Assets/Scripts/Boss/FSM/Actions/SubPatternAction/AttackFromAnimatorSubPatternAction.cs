using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackFromAnimatorSubPatternAction", menuName = "FSM/FSM Pattern Actions/New AttackFromAnimatorSubPatternAction", order = 0)]
public class AttackFromAnimatorSubPatternAction : SubPatternAction
{
    [SerializeField] private string m_AnimStateToTrigger = "Attack";
    [SerializeField] private float m_OffsetDuration = -0.1f;
    [SerializeField] private bool m_EnableSwordTrail = true;


    private float m_AnimDuration = 0f;
    private bool m_DurationSetted = false;
    private float m_Delay = 0f;
    private float m_ElapsedDuration = 0f;
    
    public override void OnEnter(FSMController fsmController)
    {
        m_DurationSetted = false;
        m_Delay = Time.time + 0.5f;
        m_ElapsedDuration = 0f;
        m_AnimDuration = Mathf.Infinity;
        
        fsmController.Boss.Animator.Play(m_AnimStateToTrigger);
        if (m_EnableSwordTrail)
            fsmController.Boss.MeleeVFX.SetActive(true);
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (!m_DurationSetted && Time.time > m_Delay)
        {
            if(fsmController.Boss.Animator.GetCurrentAnimatorStateInfo(0).IsName(m_AnimStateToTrigger))
            {
                m_DurationSetted = true;
                m_AnimDuration = fsmController.Boss.Animator.GetCurrentAnimatorStateInfo(0).length + m_OffsetDuration;
            }
        }

        if (m_ElapsedDuration >= m_AnimDuration)
        {
            Debug.Log("ENDED !!");
            fsmController.Boss.MeleeVFX.SetActive(false);
            return SubPatternActionState.ENDED;
        }
        
        m_ElapsedDuration += Time.deltaTime;
        
        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        m_DurationSetted = false;
        m_Delay = 0f;
        m_ElapsedDuration = 0f;
        m_AnimDuration = Mathf.Infinity;
    }
}
