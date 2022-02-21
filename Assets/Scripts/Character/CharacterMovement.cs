using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // DATA
    [Header("Dependencies")] 
    [SerializeField] private CharacterInput m_CharacterInput = null;
    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    
    [Header("Movement Data")]
    [SerializeField] private CharacterMovementData m_MovementData;

    private float m_CurrentSpeed = 0.0f;
    private Vector3 m_CurrentDirection = Vector3.zero;
    private Vector3 m_Translation = Vector3.zero;
    
    // METHODS
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

        m_CharacterInput.MoveEvent += Move;
    }

    private void OnDisable()
    {
        m_CharacterInput.MoveEvent -= Move;
    }

    private void Update()
    {
        m_Translation = m_CurrentDirection;
        m_CharacterController.Move(m_Translation * (m_MovementData.Speed * Time.deltaTime));
    }

    private void Move(Vector2 dir)
    {
        Vector3 dir3D = new Vector3(dir.x, 0.0f, dir.y).normalized;
        Quaternion qt = Quaternion.Euler(0.0f, m_CameraReferential.eulerAngles.y, 0.0f);
        m_CurrentDirection = qt * dir3D;
    }
}
