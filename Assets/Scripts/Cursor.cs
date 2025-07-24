using System;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Cursor : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectionPrefab;

    [SerializeField]
    private float _gridSize = 1f;

    [SerializeField]
    private float _yOffset = .2f;

    private GameObject _selectionInstance;

    void Start()
    {
        if (_selectionPrefab == null)
        {
            Debug.LogError($"No Selection Prefab set for cursor in scene: {SceneManager.GetActiveScene().name}");
            Destroy(gameObject);
            return;
        }
        _selectionInstance = Instantiate(_selectionPrefab, transform.position, _selectionPrefab.transform.rotation, transform);
    }

    void OnEnable()
    {
        InputManager.TryRegister(input => input.OnPointAt += OnCursorMoved);
    }

    void OnDisable()
    {
        InputManager.Instance.OnPointAt -= OnCursorMoved;
    }

    private void OnCursorMoved(Vector2 screenPosition)
    {
        UpdateCursorPosition();
    }

    public void UpdateCursorPosition()
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

                x = Mathf.Round(hitPoint.x / _gridSize) * _gridSize;
                z = Mathf.Round(hitPoint.z / _gridSize) * _gridSize;

                x = Mathf.Clamp(x, Mathf.Floor(minX / _gridSize) * _gridSize, Mathf.Floor(maxX / _gridSize) * _gridSize);
                z = Mathf.Clamp(z, Mathf.Floor(minZ / _gridSize) * _gridSize, Mathf.Floor(maxZ / _gridSize) * _gridSize);
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
}