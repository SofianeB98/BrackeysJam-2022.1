using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputDispatcher", menuName = "Input/New Input Dispatcher", order = 0)]
public class InputDispatcher : ScriptableObject, PlayerInputs.IPlayerActions
{
    public event Action<Vector2> MoveEvent = null;
    
    private PlayerInputs m_GameInput;
    
    private void OnEnable()
    {
        if (m_GameInput == null)
        {
            m_GameInput = new PlayerInputs();
            m_GameInput.Player.SetCallbacks(this);
            m_GameInput.Player.Enable();
        }
    }

    private void OnDisable()
    {
        m_GameInput.Player.Disable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        
    }

    public void OnMeleeHit(InputAction.CallbackContext context)
    {
        
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        
    }

    public void OnRangedUlt(InputAction.CallbackContext context)
    {
        
    }

    public void OnMeleeUlt(InputAction.CallbackContext context)
    {
        
    }
}
