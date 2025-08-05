using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField, Range(0f, 1f)]
    private float _initialZoomAmount = 0.5f;

    [SerializeField]
    private float _zoomScrollSensitivity = 0.05f;

    [SerializeField]
    private float _zoomSmoothTime = 0.15f;

    [SerializeField]
    private float _minZoomZ = -5f;

    [SerializeField]
    private float _maxZoomZ = -14f;

    [SerializeField]
    private float _minPitch = 25f;

    [SerializeField]
    private float _maxPitch = 70f;

    [Header("Movement")]
    [SerializeField]
    private float _moveSpeed = 10f;

    public event Action OnCameraMove;

    private float _currentZoomAmount;
    private float _targetZoomAmount;
    private float _zoomVelocity;
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _currentZoomAmount = _initialZoomAmount;
        _targetZoomAmount = _initialZoomAmount;
    }

    private void Update()
    {
        Vector2 movement = InputManager.Instance.CameraMovementVector;
        if (movement != Vector2.zero)
            HandleMovement(movement);

        UpdateZoomSmooth();
    }

    private void HandleMovement(Vector2 input)
    {
        Vector3 right = transform.right;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 move = (right * input.x + forward * input.y) * _moveSpeed * Time.deltaTime;
        transform.position += move;
        OnCameraMove?.Invoke();
    }

    private void HandleZoomImpulse(float input)
    {
        if (Mathf.Approximately(input, 0f))
            return;

        _targetZoomAmount = Mathf.Clamp01(_targetZoomAmount + input * _zoomScrollSensitivity);
    }

    private void UpdateZoomSmooth()
    {
        _currentZoomAmount = Mathf.SmoothDamp(
            _currentZoomAmount,
            _targetZoomAmount,
            ref _zoomVelocity,
            _zoomSmoothTime
        );

        float cameraZ = Mathf.Lerp(_minZoomZ, _maxZoomZ, _currentZoomAmount);
        Vector3 camLocalPos = _cameraTransform.localPosition;
        camLocalPos.z = cameraZ;

        if (_cameraTransform.localPosition != camLocalPos)
        {
            OnCameraMove?.Invoke();
        }
        _cameraTransform.localPosition = camLocalPos;

        float pitch = Mathf.Lerp(_minPitch, _maxPitch, _currentZoomAmount);
        Vector3 euler = transform.localEulerAngles;
        euler.x = pitch;
        transform.localEulerAngles = euler;
    }

    private void OnEnable()
    {
        InputManager.TryRegister(input => input.OnCameraZoom += HandleZoomImpulse);
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnCameraZoom -= HandleZoomImpulse;
    }
}
