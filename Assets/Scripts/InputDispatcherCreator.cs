using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDispatcherCreator : MonoBehaviour
{
    [SerializeField] private InputDispatcher m_InputDispatcher = null;

    private void Start()
    {
        ScriptableObjectContainer.Instance.AddScriptableObjectToContainer(m_InputDispatcher);
    }

    private void OnDisable()
    {
        
        ScriptableObjectContainer.Instance.RemoveScriptableObjectFromContainer<InputDispatcher>();
    }
}
