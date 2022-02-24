using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("Dependencies")] [SerializeField]
    private CharacterInput m_CharacterInput = null;

    [SerializeField] private CharacterAiming m_CharacterAiming = null;
    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    [SerializeField] private Animator m_CharacterAnimator = null;

    [Header("Melee Ability")] [SerializeField]
    private CharacterMeleeAbilityData m_MeleeAbilityData;

    [SerializeField] private Transform m_BottomDetectionPoint;
    [SerializeField] private Transform m_UpDetectionPoint;
    [SerializeField] private LayerMask m_IgnoreLayer;
    private bool m_TriggerExitComboDelay = false;
    private bool m_CanTriggerExit = false;
    private float m_ExitComboDelay = 0f;

    [Header("Range Ability")] [SerializeField]
    private CharacterRangeAbilityData m_RangeAbilityData;

    [SerializeField] private Projectile m_Projectile;
    private float m_RangeAbilityTimer = 0f;

    private AbilityState m_CurrentAbilityState = AbilityState.NONE;
    private readonly int meleeAttackTrigger = Animator.StringToHash("MeleeAttackTrigger");
    private readonly int rangeAttackTrigger = Animator.StringToHash("RangeAttackTrigger");
    private readonly int cancelMeleeTrigger = Animator.StringToHash("CancelMeleeTrigger");
    private readonly int exitCombo = Animator.StringToHash("ExitCombo");

    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterInput == null)
            m_CharacterInput = GetComponent<CharacterInput>();

        if (m_CharacterAnimator == null)
            m_CharacterAnimator = GetComponentInChildren<Animator>();
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
        switch (m_CurrentAbilityState)
        {
            case AbilityState.NONE:
            case AbilityState.CAN_NOT_PERFORM_ABILITY:
                return;
            case AbilityState.MELEE:
                MeleeAbility();
                break;
            case AbilityState.RANGE:
                RangeAbility();
                break;
            case AbilityState.ULTIMATE:
                break;
        }
    }

    private void MeleeAbility()
    {
        if (!m_CanTriggerExit)
        {
            m_TriggerExitComboDelay = false;
            return;
        }

        if (!m_TriggerExitComboDelay)
            return;

        if (m_ExitComboDelay < Time.time)
        {
            m_CharacterAnimator.SetTrigger(cancelMeleeTrigger);
            m_TriggerExitComboDelay = false;
            m_CurrentAbilityState = AbilityState.NONE;
        }
    }


    private void RangeAbility()
    {
        if (m_RangeAbilityTimer < Time.time)
        {
            m_CharacterAnimator.SetTrigger(rangeAttackTrigger);
            m_RangeAbilityTimer = Time.time + m_RangeAbilityData.DelayBetweenShoot;
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
                m_CharacterAnimator.ResetTrigger(meleeAttackTrigger);
                m_CharacterAnimator.ResetTrigger(cancelMeleeTrigger);
                m_TriggerExitComboDelay = false;
                m_CanTriggerExit = false;
                m_ExitComboDelay = Mathf.Infinity;
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

    private void TriggerCanNotPerformAbility(bool canPerform)
    {
        var next = canPerform ? AbilityState.NONE : AbilityState.CAN_NOT_PERFORM_ABILITY;
        CancelCurrentAction(next);
        m_CharacterAnimator.ResetTrigger(meleeAttackTrigger);
        m_CharacterAnimator.ResetTrigger(rangeAttackTrigger);
        m_CurrentAbilityState = next;
    }

    private void TriggerMeleeAbility()
    {
        if (m_CurrentAbilityState == AbilityState.CAN_NOT_PERFORM_ABILITY)
            return;

        Debug.Log("Melee Ability !!");

        CancelCurrentAction(AbilityState.MELEE);

        m_TriggerExitComboDelay = false;
        m_CanTriggerExit = false;
        m_ExitComboDelay = Mathf.Infinity;
        m_CurrentAbilityState = AbilityState.MELEE;
        m_CharacterAnimator.SetTrigger(meleeAttackTrigger);
    }

    private void TriggerRangeAbility(bool isPerformed)
    {
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

    public void DetectMeleeCollision()
    {
        if (m_CurrentAbilityState != AbilityState.MELEE)
            return;
            
        var cols = Physics.OverlapCapsule(m_BottomDetectionPoint.position, m_UpDetectionPoint.position,
            m_MeleeAbilityData.DetectionRadius, ~m_IgnoreLayer);

        m_CanTriggerExit = true;

        foreach (var c in cols)
            if (c.TryGetComponent(out Health h))
                h.ReduceHealth(m_MeleeAbilityData.Damage);
    }

    public void TriggerExitComboDelay()
    {
        if (m_CurrentAbilityState != AbilityState.MELEE)
            return;
        
        if (!m_CanTriggerExit)
            return;
        
        m_TriggerExitComboDelay = true;
        m_ExitComboDelay = Time.time + m_MeleeAbilityData.DelayBeforeExitCombo;
    }

    public void LaunchProjectile()
    {
        Projectile p = Instantiate(m_Projectile, transform.position,
            Quaternion.LookRotation(m_CharacterAiming.AimingDirection, Vector3.up));
        p.CollisionDetectedEvent += ProjectileCollideWithSomething;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (m_UpDetectionPoint != null)
            Gizmos.DrawWireSphere(m_UpDetectionPoint.position, m_MeleeAbilityData.DetectionRadius);
        if (m_BottomDetectionPoint != null)
            Gizmos.DrawWireSphere(m_BottomDetectionPoint.position, m_MeleeAbilityData.DetectionRadius);
    }
}