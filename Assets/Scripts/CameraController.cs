using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float _zoomAmount = 0.5f;

    [SerializeField] private float _zoomScrollSpeed = 20f;
    [SerializeField] private float _zoomSmoothTime = 0.15f;

    [Header("References")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Zoom")]
    [SerializeField] private float _minZoomZ = 5f;
    [SerializeField] private float _maxZoomZ = 14f;

    [Header("Pitch")]
    [SerializeField] private float _minPitch = -25f;
    [SerializeField] private float _maxPitch = -70f;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10f;

    [Header("Input")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _zoomAction;

    private float _currentZoomAmount = 0.5f;
    private float _zoomVelocity = 0f;

    void Start()
    {
        _currentZoomAmount = _zoomAmount;
    }

    private void OnEnable()
    {
        _moveAction.action.Enable();
        _zoomAction.action.Enable();
    }

    private void OnDisable()
    {
        _moveAction.action.Disable();
        _zoomAction.action.Disable();
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {
        Vector2 input = _moveAction.action.ReadValue<Vector2>();

        Vector3 right = -transform.right;
        Vector3 forward = -Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        Vector3 move = (right * input.x + forward * input.y) * _moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void HandleZoom()
    {
        float scroll = -_zoomAction.action.ReadValue<Vector2>().y;
        if (!Mathf.Approximately(scroll, 0f))
        {
            _zoomAmount = Mathf.Clamp01(_zoomAmount + scroll * _zoomScrollSpeed * Time.deltaTime);
        }

        _currentZoomAmount = Mathf.SmoothDamp(_currentZoomAmount, _zoomAmount, ref _zoomVelocity, _zoomSmoothTime);

        float cameraZ = Mathf.Lerp(_minZoomZ, _maxZoomZ, _currentZoomAmount);
        Vector3 camLocalPos = _cameraTransform.localPosition;
        camLocalPos.z = cameraZ;
        _cameraTransform.localPosition = camLocalPos;

        float pitch = Mathf.Lerp(_minPitch, _maxPitch, _currentZoomAmount);
        Vector3 euler = transform.localEulerAngles;
        euler.x = pitch;
        transform.localEulerAngles = euler;
    }
}
