using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // DATA
    [SerializeField] private CharacterMovementData m_MovementData;
    [SerializeField] private Transform m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    private Vector3 m_Translation = Vector3.zero;
    
    // METHODS
    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (m_CameraReferential == null)
            m_CameraReferential = Camera.main.transform;

        InputDispatcher id = ScriptableObjectContainer.Instance.GetScriptableObjectFromContainer<InputDispatcher>();
        id.MoveEvent += Move;
    }

    private void OnDisable()
    {
        InputDispatcher id = ScriptableObjectContainer.Instance.GetScriptableObjectFromContainer<InputDispatcher>();
        id.MoveEvent -= Move;
    }

    private void Update()
    {
        m_CharacterController.Move(m_Translation * (m_MovementData.Speed * Time.deltaTime));
    }

    public void Move(Vector2 dir)
    {
        Vector3 dir3D = new Vector3(dir.x, 0.0f, dir.y).normalized;
        Quaternion qt = Quaternion.Euler(0.0f, m_CameraReferential.eulerAngles.y, 0.0f);
        m_Translation = qt * dir3D;
    }
}
