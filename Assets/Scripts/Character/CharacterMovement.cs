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

    [Header("Movement Data")] [SerializeField]
    private CharacterMovementData m_MovementData;

    private Vector3 m_CurrentDirection = Vector3.zero;
    private float m_CurrentSpeed = 0.0f;
    private Vector3 m_Translation = Vector3.zero;

    private bool m_HasSuperSpeedOn = false;
    private float m_SuperSpeedTimer = 0.0f;

    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterInput == null)
            m_CharacterInput = GetComponent<CharacterInput>();
    }

    private void Start()
    {
        if (m_CameraReferential == null)
            m_CameraReferential = Camera.main.transform;
    }

    private void OnEnable()
    {
        m_CharacterInput.MoveEvent += Move;
    }

    private void OnDisable()
    {
        m_CharacterInput.MoveEvent -= Move;
    }

    private void Update()
    {
        CheckResetSuperSpeed();
        TriggerSuperSpeed();
        UpdateCurrentSpeed();

        m_Translation = m_CurrentDirection.sqrMagnitude > 0 ? m_CurrentDirection : m_Translation;
        m_CharacterController.Move(m_Translation * (m_CurrentSpeed * Time.deltaTime));
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
    private void Move(Vector2 dir)
    {
        Vector3 dir3D = new Vector3(dir.x, 0.0f, dir.y).normalized;
        Quaternion qt = Quaternion.Euler(0.0f, m_CameraReferential.eulerAngles.y, 0.0f);
        m_CurrentDirection = qt * dir3D;
        m_CurrentDirection.Normalize();
    }

    #endregion
}