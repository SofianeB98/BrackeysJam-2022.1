using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    public Vector3 AimingDirection => m_AimingDirection;
    
    [Header("Dependencies")]
    [SerializeField] private CharacterInput m_CharacterInput = null;
    [SerializeField] private Camera m_CameraReferential = null;
    [SerializeField] private CharacterController m_CharacterController = null;
    [SerializeField] private Animator m_CharacterAnimator = null;

    private Vector3 m_AimingDirection = Vector3.forward;
    private Plane m_Plane;
    
    private void Awake()
    {
        if (m_CharacterController == null)
            m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterInput == null)
            m_CharacterInput = GetComponent<CharacterInput>();

        m_AimingDirection = transform.forward;
        
        m_Plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Start()
    {
        if (m_CameraReferential == null)
            m_CameraReferential = Camera.main; 
    }

    private void OnEnable()
    {
        m_CharacterInput.AimingEvent += UpdateAiming;
    }

    private void OnDisable()
    {
        m_CharacterInput.AimingEvent -= UpdateAiming;
    }

    private void UpdateAimingByGamepad(Vector2 dir)
    {
        Vector3 dir3D = new Vector3(dir.x, 0.0f, dir.y).normalized;
        Quaternion qt = Quaternion.Euler(0.0f, m_CameraReferential.transform.eulerAngles.y, 0.0f);
        m_AimingDirection = qt * dir3D;
        m_AimingDirection.Normalize();
        transform.rotation = Quaternion.LookRotation(m_AimingDirection.sqrMagnitude > Mathf.Epsilon ? m_AimingDirection : m_CharacterController.velocity.normalized, Vector3.up);
    }
    
    private void UpdateAimingByMouse(Vector2 screenPos)
    {
        Ray ray = m_CameraReferential.ScreenPointToRay(screenPos);
        if (!m_Plane.Raycast(ray, out float val))
            return;
        
        Vector3 point = ray.GetPoint(val);
        Vector3 position = transform.position;
        position.y = 0;
        point.y = 0;
        m_AimingDirection = (point - position).normalized;
        transform.rotation = Quaternion.LookRotation(m_AimingDirection.sqrMagnitude > Mathf.Epsilon ? m_AimingDirection : m_CharacterController.velocity.normalized, Vector3.up);
    }
    
    #region Callbacks

    private void UpdateAiming(Vector2 dir, bool isGamepadInput)
    {
        if (isGamepadInput)
            UpdateAimingByGamepad(dir);
        else
            UpdateAimingByMouse(dir);

        var direction = m_CharacterController.velocity.normalized;
        var aiming = m_AimingDirection.normalized;
        var angle = Vector3.Dot(direction,aiming);
        angle = Mathf.Acos(angle) * Mathf.Rad2Deg * Mathf.Sign(angle);

        var dirToLook = (direction - aiming).normalized;
        dirToLook.y = 0;
        Quaternion qt = Quaternion.Euler(0.0f, angle, 0.0f);
        dirToLook = qt * direction;

        m_CharacterAnimator.SetFloat("BlendX", dirToLook.x);
        m_CharacterAnimator.SetFloat("BlendY", dirToLook.z);
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var point = transform.position + m_AimingDirection;
        Gizmos.DrawLine(transform.position, point);
        Gizmos.DrawWireSphere(point, 0.25f);
    }
}
