using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputRaycaster : MonoBehaviour, IInputListener
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        InputManager.TryRegister(this);
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnSelect -= HandleSelection;
        }
    }

    public void RegisterInput(InputManager input)
    {
        input.OnSelect += HandleSelection;
    }

    private void HandleSelection()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 screenPosition = InputManager.Instance.GetPointerScreenPosition();
        Debug.Log(screenPosition);
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
