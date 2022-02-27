using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSingleton : MonoBehaviour
{
    public static PersistentObjectSingleton Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject(nameof(PersistentObjectSingleton));
                m_Instance = go.AddComponent<PersistentObjectSingleton>();
            }

            return m_Instance;
        }
    }
    
    private static PersistentObjectSingleton m_Instance;

    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
    }
}
