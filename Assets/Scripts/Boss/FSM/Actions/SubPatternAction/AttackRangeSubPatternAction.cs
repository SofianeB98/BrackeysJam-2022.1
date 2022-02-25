using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeSubPatternAction : SubPatternAction
{
    [SerializeField] private AIProjectileParameter[] m_ProjectileParameters;
    [SerializeField] private int m_RepeatCount = 1;
    [SerializeField] private float m_DelayBeforeRepeat = 0f;
    
    private int m_CurrentCount = 0;
    private float m_CurrentDelayBeforeRepeat = 0f;
    
    public override void OnEnter(FSMController fsmController)
    {
        m_CurrentCount = 0;
        m_CurrentDelayBeforeRepeat = 0f;
    }

    public override SubPatternActionState Execute(FSMController fsmController)
    {
        if (m_CurrentCount >= m_RepeatCount)
            return SubPatternActionState.ENDED;

        if (m_CurrentDelayBeforeRepeat > Time.time)
            return SubPatternActionState.PERFORMED;

        foreach (var p in m_ProjectileParameters)
        {
            var pos = fsmController.Boss.transform.position;
            var fwrd = fsmController.Boss.transform.forward;

            var dir = Quaternion.Euler(0f, p.LaunchAngleFromForward, 0f) * fwrd;
            var rot = Quaternion.LookRotation(dir.normalized, Vector3.up);

            var proj = Instantiate(p.ProjectilePrefab, pos, rot);
            proj.CollisionDetectedEvent += fsmController.Boss.ProjectileCollideWithSomething;
        }

        m_CurrentCount++;
        m_CurrentDelayBeforeRepeat = Time.time + m_DelayBeforeRepeat;
        
        return SubPatternActionState.PERFORMED;
    }

    public override void OnEnd(FSMController fsmController)
    {
        m_CurrentCount = 0;
        m_CurrentDelayBeforeRepeat = 0f;
    }
}

[Serializable]
public struct AIProjectileParameter
{
    public Projectile ProjectilePrefab;
    [Tooltip("This is the angle that we add from the forward of the entity")] public float LaunchAngleFromForward;
}