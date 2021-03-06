using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Dependencies")] [SerializeField]
    private CharacterInput m_CharacterInput = null;

    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    [SerializeField] private Animator m_CharacterAnimator = null;
    [SerializeField] private AudioSource m_AudioSource;
    
    [Header("Movement Data")] [SerializeField]
    private CharacterMovementData m_MovementData;
    [SerializeField] private bool m_EnableSuperSpeed = false;

    [Header("Dash Data")] [SerializeField] private CharacterDashData m_DashData;
    [SerializeField] private int m_IgnoreCollisionsLayer = 7;
    [SerializeField] private int m_DefaultLayer = 6;
    [SerializeField] private GameObject m_DashVFX;
    [SerializeField] private AudioClip m_DashSFX;
    
    private Vector3 m_DashDirection = Vector3.forward;
    private Vector3 m_CurrentDirection = Vector3.zero;
    private float m_CurrentSpeed = 0.0f;
    private Vector3 m_Translation = Vector3.zero;

    private float m_SuperSpeedTimer = 0.0f;
    private bool m_HasSuperSpeedOn = false;

    private bool m_IsDashing = false;
    private float m_DashLoadingTimer = 0.0f;
    private float m_CurrentDashDuration = 0.0f;
    private float m_DashRecoveryTimer = 0.0f;

    private CollisionFlags m_DefaultCollisionFlags;

    private bool m_CanMove = true;
    private readonly int m_AnimIsDashing = Animator.StringToHash("IsDashing");
    private readonly int isDashing = Animator.StringToHash("IsDashing");
    
    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterInput == null)
            m_CharacterInput = GetComponent<CharacterInput>();

        if (m_AudioSource == null)
            m_AudioSource = GetComponent<AudioSource>();
        
        m_Translation = transform.forward;
        m_DefaultCollisionFlags = m_CharacterController.collisionFlags;
        m_DefaultLayer = gameObject.layer;
    }

    private void Start()
    {
        if (m_CameraReferential == null)
            m_CameraReferential = Camera.main.transform;
    }

    private void OnEnable()
    {
        m_CharacterInput.MoveEvent += UpdateMove;
        m_CharacterInput.DashEvent += TriggerDash;
        CharacterEvents.UpdateCanMoveEvent += TriggerCanMove;
    }

    private void OnDisable()
    {
        m_CharacterInput.MoveEvent -= UpdateMove;
        m_CharacterInput.DashEvent -= TriggerDash;
        CharacterEvents.UpdateCanMoveEvent -= TriggerCanMove;
    }

    private void Update()
    {
        if (!m_CharacterInput.IsActive)
        {
            m_CurrentSpeed = 0;
            m_Translation = Vector3.zero;
            m_DashDirection = Vector3.zero;
            return;
        }
        
        if (m_IsDashing)
        {
            ProcessDash();
        }
        else
        {
            if (m_DashVFX.activeSelf)
            {
                m_DashVFX.SetActive(false);
            }
            
            m_DashDirection = m_CurrentDirection.sqrMagnitude > 0 ? m_CurrentDirection : m_DashDirection;
            if (m_EnableSuperSpeed) CheckResetSuperSpeed();

            if (m_CanMove)
            {
                m_Translation = m_CurrentDirection.sqrMagnitude > 0 ? m_CurrentDirection : m_Translation;
                
                if (m_EnableSuperSpeed) TriggerSuperSpeed();
                UpdateCurrentSpeed();

                m_CharacterController.Move(m_Translation * (m_CurrentSpeed * Time.deltaTime));
            }
        }

        UpdateAnimator();
    }

    private void LateUpdate()
    {
        if (transform.position.y <= 0.12f && transform.position.y >= 0f)
            return;
        
        var pos = transform.position;
        pos.y = 0.11f;
        transform.position = pos;
    }

    private void UpdateAnimator()
    {
        m_CharacterAnimator.SetBool(m_AnimIsDashing, m_IsDashing);
    }

    private void ProcessDash()
    {
        if (!CheckDashLoadDuration())
        {
            return;
        }

        if (m_CurrentDashDuration > m_DashData.DashDuration)
        {
            gameObject.layer = m_DefaultLayer;
            if (m_DashRecoveryTimer > m_DashData.DashRecoveryDuration)
            {
                m_IsDashing = false;
                m_DashRecoveryTimer = 0f;
                m_CurrentDashDuration = 0f;
                m_DashLoadingTimer = 0f;

                m_CharacterAnimator.SetBool(isDashing, m_IsDashing);
                CharacterEvents.DashStateUpdate?.Invoke(m_IsDashing);
            }

            m_CharacterController.Move(m_Translation * (m_CurrentSpeed * Time.deltaTime));
            m_DashRecoveryTimer += Time.deltaTime;
            return;
        }

        m_CharacterController.Move(m_DashDirection * (m_DashData.DashSpeed * Time.deltaTime));
        m_CurrentDashDuration += Time.deltaTime;
    }

    private bool CheckDashLoadDuration()
    {
        if (m_DashLoadingTimer < Time.time)
            return true;

        return false;
    }

    private void CheckResetSuperSpeed()
    {
        if (!m_HasSuperSpeedOn)
            return;

        if ((m_CurrentDirection.sqrMagnitude > Mathf.Epsilon && m_CurrentSpeed < m_MovementData.SuperSpeed) ||
            m_CurrentSpeed <= 0f)
        {
            m_HasSuperSpeedOn = false;
            m_SuperSpeedTimer = 0f;
        }
    }

    private void TriggerSuperSpeed()
    {
        if (m_HasSuperSpeedOn)
            return;

        if (m_CurrentDirection.sqrMagnitude <= Mathf.Epsilon)
            return;

        if (!Mathf.Approximately(m_CurrentSpeed, m_MovementData.Speed))
            return;

        m_SuperSpeedTimer += Time.deltaTime;
        if (m_SuperSpeedTimer > m_MovementData.SuperSpeedUnlockTimer)
        {
            m_HasSuperSpeedOn = true;
            m_CurrentSpeed = m_MovementData.SuperSpeed;
            m_SuperSpeedTimer = 0f;
        }
    }

    private void UpdateCurrentSpeed()
    {
        if (m_CurrentDirection.sqrMagnitude <= Mathf.Epsilon && m_CurrentSpeed >= 0)
        {
            m_CurrentSpeed = Mathf.MoveTowards(m_CurrentSpeed, 0f,
                Time.deltaTime * m_MovementData.Speed /
                (m_HasSuperSpeedOn ? m_MovementData.DecelSuperSpeedDuration : m_MovementData.DecelDuration));
            return;
        }

        if (m_HasSuperSpeedOn)
            return;

        float speedTarget = m_HasSuperSpeedOn ? m_MovementData.SuperSpeed : m_MovementData.Speed;

        m_CurrentSpeed = Mathf.MoveTowards(m_CurrentSpeed, speedTarget,
            Time.deltaTime * speedTarget / m_MovementData.AccelDuration);
    }


    #region Callbacks

    /// <summary>
    /// Callback from OnMove input
    /// </summary>
    /// <param name="dir">Input direction from player</param>
    private void UpdateMove(Vector2 dir)
    {
        Vector3 dir3D = new Vector3(dir.x, 0.0f, dir.y).normalized;
        Quaternion qt = Quaternion.Euler(0.0f, m_CameraReferential.eulerAngles.y, 0.0f);
        m_CurrentDirection = qt * dir3D;
        m_CurrentDirection.Normalize();
    }

    /// <summary>
    /// Callback from OnDash input
    /// </summary>
    private void TriggerDash()
    {
        if (m_IsDashing)
            return;
        
        m_IsDashing = true;

        m_DashVFX.SetActive(true);
        
        if (m_DashSFX != null)
            m_AudioSource?.PlayOneShot(m_DashSFX);
        
        gameObject.layer = m_IgnoreCollisionsLayer;
        m_DashRecoveryTimer = 0f;
        m_CurrentDashDuration = 0f;
        m_DashLoadingTimer = Time.time + m_DashData.DashLoadDuration;

        m_CharacterAnimator.SetBool(isDashing, m_IsDashing);
        CharacterEvents.DashStateUpdate?.Invoke(m_IsDashing);
        CharacterEvents.TriggerInvicible?.Invoke(m_DashData.InvincibleDuration);
    }

    private void TriggerCanMove(bool val)
    {
        m_CanMove = val;
        m_CurrentSpeed = 0f;
    }
    
    #endregion
}