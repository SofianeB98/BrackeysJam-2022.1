using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AbilityState
{
    NONE,
    CAN_NOT_PERFORM_ABILITY,
    MELEE,
    RANGE,
    ULTIMATE
}

public class CharacterAbility : MonoBehaviour
{
    [SerializeField] private Transform m_Boss;

    [Header("Dependencies")] [SerializeField]
    private CharacterInput m_CharacterInput = null;

    [SerializeField] private CharacterAiming m_CharacterAiming = null;
    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    [SerializeField] private Animator m_CharacterAnimator = null;
    [SerializeField] private AudioSource m_AudioSource;

    [Header("Melee Ability")] [SerializeField]
    private CharacterMeleeAbilityData m_MeleeAbilityData;

    //[SerializeField] private Transform m_BottomDetectionPoint;
    //[SerializeField] private Transform m_UpDetectionPoint;
    [SerializeField] private LayerMask m_IgnoreLayer;
    [SerializeField] private float m_RangeAutoLock = 2.0f;
    [SerializeField] private GameObject m_MeleeVFX;
    [SerializeField] private AudioClip m_MeleeSFX;
    [SerializeField] private AudioClip[] m_HitSFX;
    private float m_ExitComboDelay = 0f;
    private float m_CurrentComboPercent = 0f;
    private float m_DelayAfterEndCombo = 0f;
    private bool m_TriggerExitComboDelay = false;
    private bool m_CanTriggerExit = false;

    private bool m_MeleeAbilityAvailable = true;
    //private bool m_AskedNextMeleeAttack = false;

    [Header("Range Ability")] [SerializeField]
    private CharacterRangeAbilityData m_RangeAbilityData;

    [SerializeField] private Projectile m_Projectile;
    private float m_RangeAbilityTimer = 0f;
    private int m_CurrentMunition = 15;

    private AbilityState m_CurrentAbilityState = AbilityState.NONE;
    private readonly int meleeAttackTrigger = Animator.StringToHash("MeleeAttackTrigger");
    private readonly int rangeAttackTrigger = Animator.StringToHash("RangeAttackTrigger");
    private readonly int cancelMeleeTrigger = Animator.StringToHash("CancelMeleeTrigger");
    private readonly int nextMeleeAttackTrigger = Animator.StringToHash("NextMeleeAttackTrigger");

    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterInput == null)
            m_CharacterInput = GetComponent<CharacterInput>();

        if (m_CharacterAnimator == null)
            m_CharacterAnimator = GetComponentInChildren<Animator>();

        if (m_AudioSource == null)
            m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (m_CameraReferential == null)
            m_CameraReferential = Camera.main.transform;
    }

    private void OnEnable()
    {
        m_CharacterInput.RangeAbilityEvent += TriggerRangeAbility;
        m_CharacterInput.MeleeAbilityEvent += TriggerMeleeAbility;
        m_CharacterInput.UltimateAbilityEvent += TriggerUltimateAbility;

        CharacterEvents.DashStateUpdate += TriggerCanNotPerformAbility;
    }

    private void OnDisable()
    {
        m_CharacterInput.RangeAbilityEvent -= TriggerRangeAbility;
        m_CharacterInput.MeleeAbilityEvent -= TriggerMeleeAbility;
        m_CharacterInput.UltimateAbilityEvent -= TriggerUltimateAbility;

        CharacterEvents.DashStateUpdate -= TriggerCanNotPerformAbility;
    }

    private void Update()
    {
        if (!m_CharacterInput.IsActive)
        {
            CancelCurrentAction(AbilityState.CAN_NOT_PERFORM_ABILITY);
            m_CurrentAbilityState = AbilityState.CAN_NOT_PERFORM_ABILITY;
            return;
        }
        
        if (!m_MeleeAbilityAvailable && Time.time > m_DelayAfterEndCombo)
        {
            m_MeleeAbilityAvailable = true;
            m_DelayAfterEndCombo = Mathf.Infinity;
        }

        switch (m_CurrentAbilityState)
        {
            case AbilityState.NONE:
            case AbilityState.CAN_NOT_PERFORM_ABILITY:
                return;
            case AbilityState.MELEE:
                CheckMeleeAbilityExitCombo();
                break;
            case AbilityState.RANGE:
                RangeAbility();
                break;
            case AbilityState.ULTIMATE:
                break;
        }
    }

    private void CheckMeleeAbilityExitCombo()
    {
        if (!m_CanTriggerExit)
        {
            m_TriggerExitComboDelay = false;
            m_ExitComboDelay = Mathf.Infinity;
            return;
        }

        if (!m_TriggerExitComboDelay)
            return;

        if (m_ExitComboDelay < Time.time)
        {
            CancelCurrentAction(AbilityState.NONE);
            m_CurrentAbilityState = AbilityState.NONE;
        }
    }


    private void RangeAbility()
    {
        if (m_RangeAbilityTimer < Time.time && m_CurrentMunition > 0)
        {
            m_CharacterAnimator.SetTrigger(rangeAttackTrigger);
            m_RangeAbilityTimer = Time.time + m_RangeAbilityData.DelayBetweenShoot;
        }
        else if (m_CurrentMunition == 0)
        {
            CancelCurrentAction(AbilityState.NONE);
            m_CurrentAbilityState = AbilityState.NONE;
        }
    }

    private void CancelCurrentAction(AbilityState nextAction)
    {
        if (m_CurrentAbilityState == nextAction)
            return;

        switch (m_CurrentAbilityState)
        {
            case AbilityState.NONE:
            case AbilityState.CAN_NOT_PERFORM_ABILITY:
                break;
            case AbilityState.MELEE:
                m_TriggerExitComboDelay = false;
                m_CanTriggerExit = false;
                //m_AskedNextMeleeAttack = false;

                m_ExitComboDelay = Mathf.Infinity;
                m_CurrentComboPercent = 0f;

                m_CharacterAnimator.ResetTrigger(meleeAttackTrigger);
                m_CharacterAnimator.ResetTrigger(nextMeleeAttackTrigger);
                m_CharacterAnimator.SetTrigger(cancelMeleeTrigger);

                m_MeleeVFX.SetActive(false);
                
                CharacterEvents.UpdateCanMoveEvent?.Invoke(true);
                break;
            case AbilityState.RANGE:
                m_CharacterAnimator.ResetTrigger(rangeAttackTrigger);
                break;
            case AbilityState.ULTIMATE:
                break;
        }
    }

    #region Callbacks

    private void ProjectileCollideWithSomething(Transform oth, float projectileDamage)
    {
        if (!oth.TryGetComponent(out Health hp))
            return;

        hp.ReduceHealth(projectileDamage);
    }

    private void TriggerCanNotPerformAbility(bool cantPerform)
    {
        var next = !cantPerform ? AbilityState.NONE : AbilityState.CAN_NOT_PERFORM_ABILITY;
        CancelCurrentAction(next);
        m_CharacterAnimator.ResetTrigger(meleeAttackTrigger);
        m_CharacterAnimator.ResetTrigger(rangeAttackTrigger);
        m_CharacterAnimator.ResetTrigger(nextMeleeAttackTrigger);
        m_CurrentAbilityState = next;
    }

    private void TriggerMeleeAbility()
    {
        StartCoroutine("TriggerMeleeAbilityCoroutine");
    }

    private IEnumerator TriggerMeleeAbilityCoroutine()
    {
        if (m_CurrentAbilityState == AbilityState.CAN_NOT_PERFORM_ABILITY || !m_MeleeAbilityAvailable)
            yield break;

        float startTime = Time.time;

        while (Time.time < startTime + m_MeleeAbilityData.MoveTime)
        {
            m_CharacterController.Move(transform.forward * m_MeleeAbilityData.SpeedMovement * Time.deltaTime);
            yield return null;
        }

        //Debug.Log("Melee Ability !!");

        CancelCurrentAction(AbilityState.MELEE);

        if (m_CurrentAbilityState == AbilityState.MELEE)
        {
            m_CurrentComboPercent += m_MeleeAbilityData.AdditionnalDamageComboPercent;
            //m_AskedNextMeleeAttack = true;
        }
        else
        {
            m_CurrentComboPercent = 0f;
            //m_AskedNextMeleeAttack = false;
        }

        m_TriggerExitComboDelay = false;
        m_CanTriggerExit = false;
        m_ExitComboDelay = Mathf.Infinity;
        m_CurrentAbilityState = AbilityState.MELEE;
        m_CharacterAnimator.SetTrigger(meleeAttackTrigger);

        if (!m_MeleeVFX.activeSelf)
            m_MeleeVFX.SetActive(true);

        CharacterEvents.UpdateCanMoveEvent?.Invoke(false);
        yield return null;
    }

    private void TriggerRangeAbility(bool isPerformed)
    {
        if (m_CurrentMunition == 0)
            return;

        if (m_CurrentAbilityState == AbilityState.CAN_NOT_PERFORM_ABILITY)
            return;

        Debug.Log("Range Ability !! " + isPerformed);

        var nextState = isPerformed ? AbilityState.RANGE : AbilityState.NONE;

        CancelCurrentAction(nextState);
        m_CurrentAbilityState = nextState;
    }

    private void TriggerUltimateAbility()
    {
        if (m_CurrentAbilityState == AbilityState.CAN_NOT_PERFORM_ABILITY)
            return;

        Debug.Log("Ultimate Ability");

        CancelCurrentAction(AbilityState.ULTIMATE);
        m_CurrentAbilityState = AbilityState.ULTIMATE;
    }

    #endregion

    #region Animation Callbacks

    public void MeleeLookToBoss()
    {
        if (Vector3.Distance(transform.position, m_Boss.position) <= m_RangeAutoLock)
        {
            var dir = (m_Boss.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
        
        m_AudioSource?.PlayOneShot(m_MeleeSFX);
    }

    public void DetectMeleeCollision()
    {
        if (m_CurrentAbilityState != AbilityState.MELEE)
            return;

        //var cols = Physics.OverlapCapsule(m_BottomDetectionPoint.position, m_UpDetectionPoint.position,
        //    m_MeleeAbilityData.DetectionRadius, ~m_IgnoreLayer);

        var cols = Physics.OverlapSphere(
            transform.position + (transform.rotation * m_MeleeAbilityData.DetectionPositionOffset),
            m_MeleeAbilityData.DetectionRadius, ~m_IgnoreLayer);
        
        m_CanTriggerExit = true;

        foreach (var c in cols)
        {
            if (c.TryGetComponent(out Health h))
            {
                h.ReduceHealth(m_MeleeAbilityData.Damage + m_MeleeAbilityData.Damage * m_CurrentComboPercent * 0.01f);
                
                if (m_HitSFX != null && m_HitSFX.Length > 0)
                    m_AudioSource?.PlayOneShot(m_HitSFX[Random.Range(0, m_HitSFX.Length)]);
                
                if (m_CurrentMunition < m_RangeAbilityData.MaxMunition)
                {
                    m_CurrentMunition = Mathf.Clamp(m_CurrentMunition + 1, 0, m_RangeAbilityData.MaxMunition);
                    CharacterEvents.ProjectileAdded.Invoke(m_CurrentMunition);
                }
            }
        }
    }

    public void TriggerExitComboDelay()
    {
        if (m_CurrentAbilityState != AbilityState.MELEE)
            return;

        if (!m_CanTriggerExit)
            return;

        //m_AskedNextMeleeAttack = false;
        //m_CharacterAnimator.ResetTrigger(nextMeleeAttackTrigger);
        m_TriggerExitComboDelay = true;
        m_ExitComboDelay = Time.time + m_MeleeAbilityData.DelayBeforeExitCombo;
    }

    public void TriggerEndCombo()
    {
        m_TriggerExitComboDelay = false;
        //m_AskedNextMeleeAttack = false;
        m_CanTriggerExit = false;
        m_CurrentComboPercent = 0f;
        m_CurrentAbilityState = AbilityState.NONE;

        m_DelayAfterEndCombo = Time.time + m_MeleeAbilityData.DelayAfterEndCombo;
        m_MeleeAbilityAvailable = false;

        m_CharacterAnimator.SetTrigger(cancelMeleeTrigger);
        m_CharacterAnimator.ResetTrigger(meleeAttackTrigger);
        m_CharacterAnimator.ResetTrigger(nextMeleeAttackTrigger);


        CharacterEvents.UpdateCanMoveEvent?.Invoke(true);
    }

    public void TriggerAskedNextAttack()
    {
        //if (!m_AskedNextMeleeAttack)
        //    return;

        //m_CharacterAnimator.ResetTrigger(meleeAttackTrigger);
        //m_AskedNextMeleeAttack = false;

        Debug.Log("NEXT ATTAQUE CAN BE PERFORMED ! CLIC !");
        m_CharacterAnimator.SetTrigger(nextMeleeAttackTrigger);
    }

    public void LaunchProjectile()
    {
        var angleToTarget = Vector3.Angle(transform.forward, (m_Boss.position - transform.position).normalized);
        Quaternion rot;
        if (angleToTarget < m_RangeAbilityData.AngleTresholdToAutoAim)
        {
            var dir = (m_Boss.position - transform.position).normalized;
            rot = Quaternion.LookRotation(dir, Vector3.up);
        }
        else
            rot = Quaternion.LookRotation(m_CharacterAiming.AimingDirection, Vector3.up);

        Projectile p = Instantiate(m_Projectile, transform.position + (rot * m_RangeAbilityData.LaunchPositionOffset),
            rot);
        p.CollisionDetectedEvent += ProjectileCollideWithSomething;
        if (m_CurrentMunition > 0)
        {
            m_CurrentMunition = Mathf.Clamp(m_CurrentMunition - 1, 0, m_RangeAbilityData.MaxMunition);
            CharacterEvents.ProjectileRemoved.Invoke(m_CurrentMunition);
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + (transform.rotation * m_MeleeAbilityData.DetectionPositionOffset), m_MeleeAbilityData.DetectionRadius);
    }
}