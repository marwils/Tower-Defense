using UnityEngine;
using UnityEngine.InputSystem;

public class GridSelection : MonoBehaviour
{
    [SerializeField]
    private Transform _selectionIndicator;

    [SerializeField]
    private float _gridSize = 1f;

    [SerializeField]
    private float _yOffset = .2f;

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            float x = Mathf.Round(hitPoint.x / _gridSize) * _gridSize;
            float z = Mathf.Round(hitPoint.z / _gridSize) * _gridSize;

            Vector3 gridPosition = new Vector3(x, _yOffset, z);
            _selectionIndicator.position = gridPosition;
        }
    }
}