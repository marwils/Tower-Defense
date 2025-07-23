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
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            float x = Mathf.Round(hitPoint.x / _gridSize) * _gridSize;
            float z = Mathf.Round(hitPoint.z / _gridSize) * _gridSize;

            Vector3 gridPosition = new Vector3(x, _yOffset, z);
            _selectionInstance.transform.position = gridPosition;
        }
    }
}