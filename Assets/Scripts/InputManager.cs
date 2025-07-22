using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action OnSelect;

    private PlayerInputActions _input;

    private static readonly List<IInputListener> _pendingListeners = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        _input = new PlayerInputActions();
        _input.Gameplay.Select.performed += ctx => OnSelect?.Invoke();

        foreach (var listener in _pendingListeners)
        {
            listener.RegisterInput(this);
        }
        _pendingListeners.Clear();
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    public Vector2 GetPointerScreenPosition()
    {
        if (Touchscreen.current?.primaryTouch.press.isPressed == true)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        return Mouse.current?.position.ReadValue() ?? Vector2.zero;
    }

    public static void TryRegister(IInputListener listener)
    {
        if (Instance != null)
        {
            listener.RegisterInput(Instance);
        }
        else
        {
            _pendingListeners.Add(listener);
        }
    }
}
