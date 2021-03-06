using System;
using UnityEngine;
using UnityEngine.AI;

public class BossBehaviorManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private NavMeshAgent m_Agent;
    
    public Animator Animator
    {
        get { return m_Animator; }
    }

    public NavMeshAgent Agent
    {
        get { return m_Agent; }
    }
    
    [Header("Behaviors")] 
    [SerializeField] private BossEarthquakeBehavior m_BossEarthquakeBehavior;
    [SerializeField] private BossMeleeAtkBehavior m_BossMeleeAtkBehavior;
    [SerializeField] private BossSwordCrossSlashBehavior m_BossSwordCrossSlashBehavior;
    [SerializeField] private BossLineWaveBehavior m_BossLineWaveBehavior;
    [SerializeField] private BossSwordWaveBehavior m_BossSwordWaveBehavior;
    [SerializeField] private BossDashData m_DashData;
    public BossDashData DashData => m_DashData;

    [Header("Sword Wave Parameters")]
    [SerializeField] private Projectile m_SwordWavePrefab;
    
    [Header("Target")] 
    [SerializeField] private Transform m_Target;
    public Transform Target => m_Target;

    [Header("Stats")]
    [SerializeField] private Health m_Health;
    [SerializeField] private float damage = 20.0f;

    [Header("FX")] 
    public GameObject MeleeVFX;

    public GameObject DashVFX;
    
    public Health Health
    {
        get { return m_Health; }
    }
    
    private void Awake()
    {
        if (m_Animator == null)
            m_Animator = GetComponentInChildren<Animator>();
        
        if (m_Health == null)
            m_Health = GetComponent<Health>();

        if (m_Agent == null)
            m_Agent = GetComponent<NavMeshAgent>();
        
        if (m_BossEarthquakeBehavior == null)
            m_BossEarthquakeBehavior = GetComponent<BossEarthquakeBehavior>();
        
        if (m_BossMeleeAtkBehavior == null)
            m_BossMeleeAtkBehavior = GetComponent<BossMeleeAtkBehavior>();
        
        if (m_BossSwordCrossSlashBehavior == null) 
            m_BossSwordCrossSlashBehavior = GetComponent<BossSwordCrossSlashBehavior>();
        
        if (m_BossLineWaveBehavior == null)
            m_BossLineWaveBehavior = GetComponent<BossLineWaveBehavior>();
        
        if (m_BossSwordWaveBehavior == null) 
            m_BossSwordWaveBehavior = GetComponent<BossSwordWaveBehavior>();
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

    public void OnBossDie()
    {
        BossEvents.BossDieEvent.Invoke();
    }
    
    #region AnimationCallbacks

    public void TriggerEarthquake()
    {
        m_BossEarthquakeBehavior.Detect(this);
    }

    public void TriggerMeleeAttack()
    {
        m_BossMeleeAtkBehavior.Detect(this);
    }

    public int SlashIndex = 0;
    public void TriggerSwordCrossSlash(int slashIndex)
    {
        SlashIndex = slashIndex;
        m_BossSwordCrossSlashBehavior.Detect(this);
    }

    public void TriggerSwordWave()
    {
        m_BossSwordWaveBehavior.TriggerRange();
    }
    
    public void TriggerLineWave()
    {
        m_BossLineWaveBehavior.TriggerRange();
    }

    public void SetActiveMeleeVFX()
    {
        MeleeVFX.SetActive(!MeleeVFX.activeSelf);
    }
    
    #endregion
}