using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorMovement : MonoBehaviour
{
    private const float GridSize = 1f;

    [SerializeField]
    private GameObject _selectionPrefab;

    private GameObject _selectionInstance;

    private CameraControl _cameraController;

    private bool _cameraEventRegistered = false;

    private void Start()
    {
        if (_selectionPrefab == null)
        {
            Debug.LogError($"No Selection Prefab set for cursor in scene: {SceneManager.GetActiveScene().name}");
            Destroy(gameObject);
            return;
        }
        _selectionInstance = Instantiate(_selectionPrefab, transform.position, _selectionPrefab.transform.rotation, transform);

        TryRegisterEvents();
    }

    private void UpdateCursorPosition()
    {
        if (InputManager.Instance.RaycastFromScreenPosition(out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
                return;

            Bounds bounds = hit.collider.bounds;

            Vector3 hitPoint = hit.point;

            float x, y, z;

            if (IsTransformRotated(hit.collider.transform))
            {
                x = hit.collider.transform.position.x;
                z = hit.collider.transform.position.z;
            }
            else
            {
                float minX = bounds.min.x;
                float maxX = bounds.max.x;
                float minZ = bounds.min.z;
                float maxZ = bounds.max.z;

                x = Mathf.Round(hitPoint.x / GridSize) * GridSize;
                z = Mathf.Round(hitPoint.z / GridSize) * GridSize;

                x = Mathf.Clamp(x, Mathf.Floor(minX / GridSize) * GridSize, Mathf.Floor(maxX / GridSize) * GridSize);
                z = Mathf.Clamp(z, Mathf.Floor(minZ / GridSize) * GridSize, Mathf.Floor(maxZ / GridSize) * GridSize);
            }

            if (hit.collider.CompareTag("Building") && hit.collider.transform.parent != null)
            {
                Collider parentCollider = hit.collider.transform.parent.GetComponent<Collider>();
                y = parentCollider != null ? parentCollider.bounds.max.y : bounds.max.y;
            }
            else
            {
                y = bounds.min.y + bounds.size.y;
                _selectionInstance.transform.localRotation = hit.collider.transform.localRotation;
            }

            _selectionInstance.transform.position = new Vector3(x, y, z);
            _selectionInstance.SetActive(true);
        }
        else
        {
            _selectionInstance.SetActive(false);
        }
    }

    private static bool IsTransformRotated(Transform transform)
    {
        return Mathf.Round(transform.localEulerAngles.y) % 90 != 0;
    }

    private void OnEnable()
    {
        TryRegisterEvents();
    }

    private void OnDisable()
    {
        _cameraEventRegistered = false;

        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnPointAt -= UpdateCursorPosition;
        }

        _cameraController.OnCameraMove -= UpdateCursorPosition;
    }

    private void TryRegisterEvents()
    {
        if (!_cameraEventRegistered)
        {
            InputManager.TryRegister(input => input.OnPointAt += UpdateCursorPosition);
            _cameraController = FindFirstObjectByType<CameraControl>();
            _cameraController.OnCameraMove += UpdateCursorPosition;
            _cameraEventRegistered = true;
        }
    }
}