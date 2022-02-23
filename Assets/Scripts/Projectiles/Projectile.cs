using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public event Action<Transform, float> CollisionDetectedEvent;
    
    [Header("Projectile Data")] 
    [SerializeField] private ProjectileData m_ProjectileData;
    [SerializeField] private LayerMask m_IgnoreLayer;
    [SerializeField] private ParticleSystem m_Effect;

    [Header("Gizmo")] 
    [SerializeField] private Mesh m_CubeMesh;
    
    private float m_LifeTime = 1.0f;
    private Vector3 m_DetectionPoint;
    
    private void Start()
    {
        m_LifeTime = (m_ProjectileData.DistanceToReach / m_ProjectileData.Speed);
        InitializeParticleSystem();
        
        Destroy(gameObject, m_LifeTime + 0.02f);
        m_DetectionPoint = transform.position;
    }

    private void Update()
    {
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
        //Collider[] cols = Physics.OverlapCapsule(m_DetectionPoint, m_DetectionPoint + Vector3.up * m_ProjectileData.ProjectileHeight, m_ProjectileData.DetectionRadius, ~m_IgnoreLayer);
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
    }
    
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        
        if (m_CubeMesh != null)
            Gizmos.DrawWireMesh(m_CubeMesh, m_DetectionPoint + (transform.rotation * m_ProjectileData.DetectionPointOffset), transform.rotation, m_ProjectileData.DetectionSize);
        //Gizmos.DrawWireSphere(m_DetectionPoint, m_ProjectileData != null ? m_ProjectileData.DetectionRadius : 1.0f);
        //Gizmos.DrawWireSphere(m_DetectionPoint + Vector3.up * m_ProjectileData.ProjectileHeight, m_ProjectileData != null ? m_ProjectileData.DetectionRadius : 1.0f);
    }
}