using System;
using System.Collections.Generic;

using NUnit.Framework.Interfaces;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<Vector2> OnPointAt;
    public event Action<Vector2> OnSelect;
    public event Action<float> OnCameraZoom;

    public Vector2 CameraMovementVector => _input.Camera.Move.ReadValue<Vector2>();

    private PlayerInputActions _input;

    private Camera _camera;

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
        _input.Gameplay.PointAt.performed += PointAtActionPerformed;
        _input.Gameplay.Select.performed += SelectActionPerformed;
        _input.Camera.Zoom.performed += ZoomActionPerformed;
        _input.Enable();

        foreach (var listener in _pendingListeners)
        {
            listener.RegisterInput(this);
        }
        _pendingListeners.Clear();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void SelectActionPerformed(InputAction.CallbackContext ctx)
    {
        OnSelect?.Invoke(GetPointerScreenPosition());
    }

    private void PointAtActionPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 screenPosition = ctx.ReadValue<Vector2>();
        OnPointAt?.Invoke(screenPosition);
    }

    private void ZoomActionPerformed(InputAction.CallbackContext ctx)
    {
        float zoom = -ctx.ReadValue<Vector2>().y;
        OnCameraZoom?.Invoke(zoom);
    }

    public Vector2 GetPointerScreenPosition()
    {
        if (Touchscreen.current != null)
            return Touchscreen.current.primaryTouch.position.ReadValue();

        return Mouse.current?.position.ReadValue() ?? Vector2.zero;
    }

    public bool RaycastFromScreenPosition(out RaycastHit hit)
    {
        int groundLayer = LayerMask.NameToLayer("Ground");
        int mask = ~(1 << groundLayer); // alles außer Ground

        Vector2 screenPosition = GetPointerScreenPosition();

        Ray ray = _camera.ScreenPointToRay(screenPosition);

        bool result = Physics.Raycast(ray, out RaycastHit hitInfo, 100f, mask);
        hit = hitInfo;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return false;

        return result;
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

    public static void TryRegister(Action<InputManager> registerAction)
    {
        if (Instance != null)
        {
            registerAction(Instance);
        }
        else
        {
            _pendingListeners.Add(new LambdaInputListener(registerAction));
        }
    }

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private class LambdaInputListener : IInputListener
    {
        private readonly Action<InputManager> _registerAction;

        public LambdaInputListener(Action<InputManager> registerAction)
        {
            _registerAction = registerAction;
        }

        public void RegisterInput(InputManager input)
        {
            _registerAction(input);
        }
    }
}
