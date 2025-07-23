using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float _zoomAmount = 0.5f;

    [SerializeField] private float _zoomScrollSensitivity = 0.05f;
    [SerializeField] private float _zoomSmoothTime = 0.15f;

    [Header("Zoom")]

    [SerializeField] private float _minZoomZ = -5f;
    [SerializeField] private float _maxZoomZ = -14f;

    [Header("Pitch")]

    [SerializeField] private float _minPitch = 25f;
    [SerializeField] private float _maxPitch = 70f;

    [Header("Movement")]

    [SerializeField] private float _moveSpeed = 10f;

    private float _currentZoomAmount;
    private float _targetZoomAmount;
    private float _zoomVelocity = 0f;
    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _currentZoomAmount = _zoomAmount;
        _targetZoomAmount = _zoomAmount;
    }

    void Update()
    {
        Vector2 move = InputManager.Instance.CameraMove;
        HandleMovement(move);

        UpdateZoomSmooth();
    }

    void HandleMovement(Vector2 input)
    {
        Vector3 right = transform.right;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 move = (right * input.x + forward * input.y) * _moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void HandleZoomImpulse(float input)
    {
        if (Mathf.Approximately(input, 0f)) return;

        _targetZoomAmount = Mathf.Clamp01(_targetZoomAmount + input * _zoomScrollSensitivity);
    }

    void UpdateZoomSmooth()
    {
        _currentZoomAmount = Mathf.SmoothDamp(_currentZoomAmount, _targetZoomAmount, ref _zoomVelocity, _zoomSmoothTime);

        float cameraZ = Mathf.Lerp(_minZoomZ, _maxZoomZ, _currentZoomAmount);
        Vector3 camLocalPos = _cameraTransform.localPosition;
        camLocalPos.z = cameraZ;
        _cameraTransform.localPosition = camLocalPos;

        float pitch = Mathf.Lerp(_minPitch, _maxPitch, _currentZoomAmount);
        Vector3 euler = transform.localEulerAngles;
        euler.x = pitch;
        transform.localEulerAngles = euler;
    }

    void OnEnable()
    {
        InputManager.TryRegister(new CameraInputListener(this));
    }

    void OnDisable()
    {
        InputManager.Instance.OnCameraZoom -= HandleZoomImpulse;
    }

    private class CameraInputListener : IInputListener
    {
        private readonly CameraController _controller;

        public CameraInputListener(CameraController controller)
        {
            _controller = controller;
        }

        public void RegisterInput(InputManager input)
        {
            input.OnCameraZoom += _controller.HandleZoomImpulse;
        }
    }
}
