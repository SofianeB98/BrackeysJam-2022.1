using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    public event Action<Transform, float> CollisionDetectedEvent;
    
    [Header("Projectile Data")] 
    [SerializeField] private ProjectileData m_ProjectileData;
    [SerializeField] private LayerMask m_IgnoreLayer;
    [SerializeField] private bool m_IsVfxGraph = false;
    [SerializeField] private ParticleSystem m_Effect;
    [SerializeField] private bool m_IsNotReal = false;
    
    [Header("Gizmo")] 
    [SerializeField] private Mesh m_CubeMesh;
    
    private float m_LifeTime = 1.0f;
    private Vector3 m_DetectionPoint;
    
    private void Start()
    {
        m_LifeTime = (m_ProjectileData.DistanceToReach / m_ProjectileData.Speed);
        
        if (!m_IsVfxGraph)
            InitializeParticleSystem();
        
        Destroy(gameObject, m_LifeTime + 0.02f);
        m_DetectionPoint = transform.position;
    }

    public void SetNotReal(bool v)
    {
        m_IsNotReal = v;
    }
    
    private void Update()
    {
        if (!m_IsNotReal)
            Detect();
        
        Move();
    }

    private void InitializeParticleSystem()
    {
        var mainModule = m_Effect.main;
        mainModule.duration = m_LifeTime + 0.02f;
        mainModule.startSpeed = m_ProjectileData.Speed;
        mainModule.startLifetime = m_LifeTime;
        m_Effect.gameObject.SetActive(true);
    }
    
    private void Detect()
    {
        var cols = Physics.OverlapBox(m_DetectionPoint + (transform.rotation * m_ProjectileData.DetectionPointOffset), m_ProjectileData.DetectionSize * 0.5f, transform.rotation, ~m_IgnoreLayer);
        
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
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (m_IsNotReal)
            return;
        
        Gizmos.color = Color.cyan;
        
        if (m_CubeMesh != null)
            Gizmos.DrawWireMesh(m_CubeMesh, m_DetectionPoint + (transform.rotation * m_ProjectileData.DetectionPointOffset), transform.rotation, m_ProjectileData.DetectionSize);
        
    }
}
