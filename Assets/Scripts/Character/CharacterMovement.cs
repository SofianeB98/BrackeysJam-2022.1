using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Dependencies")] [SerializeField]
    private CharacterInput m_CharacterInput = null;
    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;

    [Header("Movement Data")] 
    [SerializeField] private CharacterMovementData m_MovementData;

    [Header("Dash Data")] 
    [SerializeField] private CharacterDashData m_DashData;
    [SerializeField] private int m_IgnoreCollisionsLayer = 7;
    [SerializeField] private int m_DefaultLayer = 6;
    
    
    private Vector3 m_CurrentDirection = Vector3.zero;
    private float m_CurrentSpeed = 0.0f;
    private Vector3 m_Translation = Vector3.zero;

    private bool m_HasSuperSpeedOn = false;
    private float m_SuperSpeedTimer = 0.0f;

    private bool m_IsDashing = false;
    private float m_DashLoadingTimer = 0.0f;
    private float m_CurrentDashDuration = 0.0f;
    private float m_DashRecoveryTimer = 0.0f;

    private CollisionFlags m_DefaultCollisionFlags;
    
    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterInput == null)
            m_CharacterInput = GetComponent<CharacterInput>();

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
    }

    private void OnDisable()
    {
        m_CharacterInput.MoveEvent -= UpdateMove;
        m_CharacterInput.DashEvent -= TriggerDash;
    }

    private void Update()
    {
        if (m_IsDashing)
        {
            ProcessDash();
            return;
        }
        
        CheckResetSuperSpeed();
        TriggerSuperSpeed();
        UpdateCurrentSpeed();
        
        m_CharacterController.Move(m_Translation * (m_CurrentSpeed * Time.deltaTime));
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
            }
            
            m_CharacterController.Move(m_Translation * (m_CurrentSpeed * Time.deltaTime));
            m_DashRecoveryTimer += Time.deltaTime;
            return;
        }
        
        m_CharacterController.Move(m_Translation * (m_DashData.DashSpeed * Time.deltaTime));
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

        if ((m_CurrentDirection.sqrMagnitude > Mathf.Epsilon && m_CurrentSpeed < m_MovementData.SuperSpeed) || m_CurrentSpeed <= 0f)
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
        
        if (!m_IsDashing)
            m_Translation = m_CurrentDirection.sqrMagnitude > 0 ? m_CurrentDirection : m_Translation;
    }

    /// <summary>
    /// Callback from OnDash input
    /// </summary>
    private void TriggerDash()
    {
        if (m_IsDashing)
            return;
        
        m_IsDashing = true;
        gameObject.layer = m_IgnoreCollisionsLayer;
        m_DashRecoveryTimer = 0f;
        m_CurrentDashDuration = 0f;
        m_DashLoadingTimer = Time.time + m_DashData.DashLoadDuration;
    }
    #endregion
}