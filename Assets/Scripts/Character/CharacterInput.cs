using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour, PlayerInputs.IPlayerActions
{
    public const string GAMEPAD_SCHEME = "GamepadScheme";
    public const string KEYBOARD_MOUSE_SCHEME = "Keyboard-Mouse";
    
    
    public event Action<Vector2> MoveEvent = null;
    public event Action DashEvent = null;
    public event Action MeleeAbilityEvent = null;
    public event Action<bool> RangeAbilityEvent = null;
    public event Action UltimateAbilityEvent = null;

    [SerializeField] private bool m_UseGamepad = false;
    private PlayerInputs m_GameInput;

    private void OnValidate()
    {
        if (m_GameInput != null)
            m_GameInput.bindingMask = new InputBinding {groups = m_UseGamepad ? GAMEPAD_SCHEME : KEYBOARD_MOUSE_SCHEME};
    }


    private void OnEnable()
    {
        if (m_GameInput == null)
        {
            m_GameInput = new PlayerInputs();
            m_GameInput.bindingMask = new InputBinding {groups = m_UseGamepad ? GAMEPAD_SCHEME : KEYBOARD_MOUSE_SCHEME};
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
        Debug.Log(context.control.device);
        Debug.Log(context.ReadValue<Vector2>());
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.action.triggered)
            return;

        DashEvent?.Invoke();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        var state = context.phase;

        switch (state)
        {
            case InputActionPhase.Started:
                RangeAbilityEvent?.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                RangeAbilityEvent?.Invoke(false);
                break;
            default:
                break;
        }

    }

    public void OnMeleeHit(InputAction.CallbackContext context)
    {
        if (!context.action.triggered)
            return;

        MeleeAbilityEvent?.Invoke();
    }

    public void OnParry(InputAction.CallbackContext context)
    {
    }

    public void OnRangedUlt(InputAction.CallbackContext context)
    {
    }

    public void OnMeleeUlt(InputAction.CallbackContext context)
    {
        if (!context.action.triggered)
            return;
        
        UltimateAbilityEvent?.Invoke();
    }
}