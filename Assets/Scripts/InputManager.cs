using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action OnSelect;
    public Vector2 CameraMove => _input.Camera.Move.ReadValue<Vector2>();
    public event Action<float> OnCameraZoom;

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
        _input.Camera.Zoom.performed += OnZoom;
        _input.Enable();

        foreach (var listener in _pendingListeners)
        {
            listener.RegisterInput(this);
        }
        _pendingListeners.Clear();
    }

    private void OnZoom(InputAction.CallbackContext ctx)
    {
        float zoom = -ctx.ReadValue<Vector2>().y;
        OnCameraZoom?.Invoke(zoom);
    }

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
    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

}
