using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptableObjectContainer : MonoBehaviour
{
    public static ScriptableObjectContainer Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType<ScriptableObjectContainer>();

                if (s_Instance == null)
                {
                    GameObject go = new GameObject(nameof(ScriptableObjectContainer));
                    ScriptableObjectContainer soc = go.AddComponent<ScriptableObjectContainer>();
                    s_Instance = soc;
                }
            }

            return s_Instance;
        }
    }
    private static ScriptableObjectContainer s_Instance;

    private Dictionary<Type, ScriptableObject> m_ScriptableObjectContainer = null;
    
    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (s_Instance == null)
            s_Instance = this;
        
        m_ScriptableObjectContainer = new Dictionary<Type, ScriptableObject>();
    }

    public void AddScriptableObjectToContainer<T>(T so) where T : ScriptableObject
    {
        m_ScriptableObjectContainer.TryAdd(typeof(T), so);
    }

    public T GetScriptableObjectFromContainer<T>() where T : ScriptableObject
    {
        if (!m_ScriptableObjectContainer.TryGetValue(typeof(T), out ScriptableObject v))
            return null;

        return v as T;
    }

    public void RemoveScriptableObjectFromContainer<T>() where T : ScriptableObject
    {
        if (!m_ScriptableObjectContainer.ContainsKey(typeof(T)))
            return;

        m_ScriptableObjectContainer.Remove(typeof(T));
    }
}