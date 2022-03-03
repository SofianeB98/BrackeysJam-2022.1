using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    public event Action<Transform, float> CollisionDetectedEvent;

    [Header("Projectile Data")] [SerializeField]
    private ProjectileData m_ProjectileData;

    [SerializeField] private LayerMask m_IgnoreLayer;
    [SerializeField] private bool m_IsVfxGraph = false;
    [SerializeField] private ParticleSystem m_Effect;
    [SerializeField] private ParticleSystem m_GlitchedEffect;
    [SerializeField] private bool m_IsNotReal = false;
    [SerializeField] private VisualEffect ShaderGraphFX;

    [Header("Gizmo")] [SerializeField] private Mesh m_CubeMesh;

    private Vector3 m_DetectionPoint;
    private Vector3 m_PreviousDetectionPoint;
    private bool m_PreviousPointInitialized = false;

    private bool m_CanDestroy = false;

    private void Start()
    {
        if (!m_IsVfxGraph)
            InitializeParticleSystem();

        Destroy(gameObject, m_ProjectileData.LifeTime + 1.0f);
        m_DetectionPoint = transform.position;
        m_PreviousPointInitialized = true;
    }

    public void SetNotReal(bool v)
    {
        m_IsNotReal = v;
        if (ShaderGraphFX != null)
        {
            ShaderGraphFX.SetBool("Glitch", m_IsNotReal);
        }
    }

    private void Update()
    {
        if (m_CanDestroy)
            return;
        
        m_PreviousDetectionPoint = m_DetectionPoint;
        Move();

        if (!m_IsNotReal)
            Detect();
    }

    private void InitializeParticleSystem()
    {
        var mainModule = m_Effect.main;

        if (m_IsNotReal)
            mainModule = m_GlitchedEffect.main;

        mainModule.duration = m_ProjectileData.LifeTime + 0.02f;
        mainModule.startSpeed = m_ProjectileData.Speed;
        mainModule.startLifetime = m_ProjectileData.LifeTime;
        if (m_IsNotReal)
            m_GlitchedEffect.gameObject.SetActive(true);
        else
            m_Effect.gameObject.SetActive(true);
    }

    private void Detect()
    {
        if (m_CanDestroy)
            return;

        float dist = Vector3.Distance(m_DetectionPoint, m_PreviousDetectionPoint);
        float addingPercent = dist / m_ProjectileData.DetectionSize.z;
        float adding = m_ProjectileData.DetectionSize.z * addingPercent;
        var addingOffset = Vector3.forward * adding;

        var cols = Physics.OverlapBox(
            m_DetectionPoint + (transform.rotation * (m_ProjectileData.DetectionPointOffset - addingOffset)),
            (m_ProjectileData.DetectionSize + addingOffset) * 0.5f, transform.rotation, ~m_IgnoreLayer);

        if (cols == null || cols.Length == 0)
        {
            Debug.Log("No collision detected");
            return;
        }

        for (int i = 0; i < cols.Length; i++)
        {
            Debug.Log("COLLIDE : " + cols[i].gameObject.name);
            CollisionDetectedEvent?.Invoke(cols[i].transform, m_ProjectileData.Damage);
        }


        DestroyProjectile();
    }

    private void Move()
    {
        m_DetectionPoint += Time.deltaTime * m_ProjectileData.Speed * transform.forward;
        if (m_IsVfxGraph)
            transform.position = m_DetectionPoint;
    }

    private void DestroyProjectile()
    {
        m_CanDestroy = true;
        Destroy(gameObject, 1.0f);
    }

    private void OnDrawGizmos()
    {
        if (m_IsNotReal || m_CanDestroy)
            return;

        Gizmos.color = Color.cyan;

        if (m_CubeMesh != null)
        {
            float dist = m_PreviousPointInitialized ? Vector3.Distance(m_DetectionPoint, m_PreviousDetectionPoint) : 0f;
            float addingPercent = dist / m_ProjectileData.DetectionSize.z;
            float adding = m_ProjectileData.DetectionSize.z * addingPercent;

            Gizmos.DrawWireMesh(m_CubeMesh,
                m_DetectionPoint + (transform.rotation * m_ProjectileData.DetectionPointOffset), transform.rotation,
                m_ProjectileData.DetectionSize);
            Gizmos.color = Color.blue;
            var size = m_ProjectileData.DetectionSize;
            size.x *= 0.5f;
            size.y *= 0.5f;
            var addingOffset = Vector3.forward * adding;
            Gizmos.DrawWireMesh(m_CubeMesh,
                m_DetectionPoint + (transform.rotation * (m_ProjectileData.DetectionPointOffset - addingOffset * 0.5f)),
                transform.rotation, size + addingOffset);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(m_DetectionPoint + Vector3.up * 0.1f, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(m_PreviousDetectionPoint + Vector3.up * 0.1f, 0.1f);
    }
}