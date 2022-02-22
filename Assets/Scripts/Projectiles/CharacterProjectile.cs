using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProjectile : MonoBehaviour
{
    [Header("Projectile Data")] 
    [SerializeField] private ProjectileData m_ProjectileData;
    [SerializeField] private LayerMask m_IgnoreLayer;
    
    private Vector3 m_DetectionPoint;
    
    private void Start()
    {
        Destroy(gameObject, m_ProjectileData.LifeTime);
        m_DetectionPoint = transform.position;
    }

    private void Update()
    {
        Detect();
        Move();
    }

    private void Detect()
    {
        Collider[] cols = Physics.OverlapCapsule(m_DetectionPoint, m_DetectionPoint + Vector3.up * m_ProjectileData.ProjectileHeight, m_ProjectileData.DetectionRadius, ~m_IgnoreLayer);
        if (cols == null || cols.Length == 0)
        {
            Debug.Log("No collision detected");
            return;
        }

        for (int i = 0; i < cols.Length; i++)
        {
            Debug.Log("COLLIDE : " + cols[i].gameObject.name);
            // if (cols[i].TryGetcomponent())
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
        Gizmos.DrawWireSphere(m_DetectionPoint, m_ProjectileData != null ? m_ProjectileData.DetectionRadius : 1.0f);
        Gizmos.DrawWireSphere(m_DetectionPoint + Vector3.up * m_ProjectileData.ProjectileHeight, m_ProjectileData != null ? m_ProjectileData.DetectionRadius : 1.0f);
    }
}
