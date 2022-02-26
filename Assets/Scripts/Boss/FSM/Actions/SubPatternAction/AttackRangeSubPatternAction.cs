using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackRangeSubPatternAction",
    menuName = "FSM/FSM Pattern Actions/New AttackRangeSubPatternAction", order = 0)]
public class AttackRangeSubPatternAction : SubPatternAction
{
    [SerializeField] private AIProjectileParameter[] m_ProjectileParameters;
    [SerializeField] private int m_RepeatCount = 1;
    [SerializeField] private float m_DelayBeforeRepeat = 0f;

    private int m_CurrentCount = 0;
    private float m_CurrentDelayBeforeRepeat = 0f;

    private int m_CurrentIndex = 0;
    private float m_CurrentTimer = 0;

    public override void OnEnter(FSMController fsmController)
    {
        m_CurrentCount = 0;
        m_CurrentDelayBeforeRepeat = 0f;
        m_CurrentIndex = 0;
        m_CurrentTimer = 0f;
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (m_CurrentCount >= m_RepeatCount)
            return SubPatternActionState.ENDED;

        if (m_CurrentDelayBeforeRepeat > Time.time)
            return SubPatternActionState.PERFORMED;

        var p = m_ProjectileParameters[m_CurrentIndex];
        if (m_CurrentTimer < p.DelayBeforeLaunch)
        {
            m_CurrentTimer += Time.deltaTime;
            return SubPatternActionState.PERFORMED;
        }
        
        var pos = fsmController.Boss.transform.position;
        var fwrd = fsmController.Boss.transform.forward;

        var dir = Quaternion.Euler(0f, p.LaunchAngleFromForward, 0f) * fwrd;
        var rot = Quaternion.LookRotation(dir.normalized, Vector3.up);

        var proj = Instantiate(p.ProjectilePrefab, pos, rot);
        proj.CollisionDetectedEvent += fsmController.Boss.ProjectileCollideWithSomething;

        m_CurrentIndex++;
        m_CurrentTimer = 0f;
        
        if (m_CurrentIndex >= m_ProjectileParameters.Length)
        {
            m_CurrentIndex = 0;
            m_CurrentCount++;
            m_CurrentDelayBeforeRepeat = Time.time + m_DelayBeforeRepeat;
        }
        
        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        m_CurrentCount = 0;
        m_CurrentDelayBeforeRepeat = 0f;
        m_CurrentTimer = 0f;
        m_CurrentIndex = 0;
    }
}

[Serializable]
public struct AIProjectileParameter
{
    public Projectile ProjectilePrefab;

    [Tooltip("This is the angle that we add from the forward of the entity")]
    public float LaunchAngleFromForward;

    public float DelayBeforeLaunch;
}