using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableEntity : MonoBehaviour
{
    public int EntityID = -1;

    public bool EntityActive => gameObject.activeSelf;

    protected virtual void InitializeEntity()
    {
        
    }

    protected virtual void DesInitializeEntity()
    {
        
    }
    
    public void EnableEntity()
    {
        InitializeEntity();
        gameObject.SetActive(true);
    }
    
    public void DisableEntity()
    {
        DesInitializeEntity();
        gameObject.SetActive(false);
    }
}
