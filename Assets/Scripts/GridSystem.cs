using UnityEngine;
using UnityEngine.InputSystem;

public class GridSelection : MonoBehaviour
{
    public Transform selectionIndicator;
    public float gridSize = 1f;
    public float yOffset = .2f;

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            float x = Mathf.Round(hitPoint.x / gridSize) * gridSize;
            float z = Mathf.Round(hitPoint.z / gridSize) * gridSize;

            Vector3 gridPosition = new Vector3(x, yOffset, z);
            selectionIndicator.position = gridPosition;
        }
    }
}