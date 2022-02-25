using System;
using UnityEngine;

public class BossBehaviorManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Animator m_Animator;
    public Animator Animator
    {
        get { return m_Animator; }
    }

    [Header("Behaviors")] 
    [SerializeField] private BossEarthquakeBehavior m_BossEarthquakeBehavior;
    [SerializeField] private BossMeleeAtkBehavior m_BossMeleeAtkBehavior;
    [SerializeField] private BossSwordCrossSlashBehavior m_BossSwordCrossSlashBehavior;
    
    [Header("Target")] 
    [SerializeField] private Transform m_Target;
    public Transform Target => m_Target;

    [Header("Stats")]
    [SerializeField] private Health m_Health;
    [SerializeField] private float damage = 20.0f;
    
    
    public Health Health
    {
        get { return m_Health; }
    }
    
    private void Awake()
    {
        if (m_Health == null)
            m_Health = GetComponent<Health>();

        if (m_BossEarthquakeBehavior == null)
            m_BossEarthquakeBehavior = GetComponent<BossEarthquakeBehavior>();
        
        if (m_BossMeleeAtkBehavior == null)
            m_BossMeleeAtkBehavior = GetComponent<BossMeleeAtkBehavior>();
        
        if (m_BossSwordCrossSlashBehavior == null) 
            m_BossSwordCrossSlashBehavior = GetComponent<BossSwordCrossSlashBehavior>();
    }

    public void SetAnimatorTrigger(string triggerName)
    {
        m_Animator.SetTrigger(triggerName);
    }

    public void SetAnimatorBool(string boolName, bool val)
    {
        m_Animator.SetBool(boolName, val);
    }

    public void SetAnimatorFloat(string floatName, float val)
    {
        m_Animator.SetFloat(floatName, val);
    }

    #region Callbacks
    
    public void ProjectileCollideWithSomething(Transform oth, float projectileDamage)
    {
        if (!oth.TryGetComponent(out Health hp))
            return;

        hp.ReduceHealth(projectileDamage);
    }
    
    #endregion

    #region AnimationCallbacks

    public void TriggerEarthquake()
    {
        m_BossEarthquakeBehavior.Detect(this);
    }

    public void TriggerMeleeAttack()
    {
        m_BossMeleeAtkBehavior.Detect(this);
    }

    public void TriggerSwordCrossSlash()
    {
        m_BossSwordCrossSlashBehavior.Detect(this);
    }
    
    #endregion
}