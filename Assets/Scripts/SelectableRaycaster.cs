using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableRaycaster : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        InputManager.TryRegister(input => input.OnSelect += HandleSelect);
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnSelect -= HandleSelect;
        }
    }

    private void HandleSelect(Vector2 screenPosition)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<ISelectable>(out var selectable))
            {
                Debug.Log($"Select: {hit.collider.gameObject.name}");
                selectable.OnSelect();
            }
        }
    }
}
