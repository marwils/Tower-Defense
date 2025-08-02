using UnityEngine;

public class Selector : MonoBehaviour
{
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

    private void HandleSelect()
    {
        if (InputManager.Instance.RaycastFromScreenPosition(out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelect();
                Debug.Log($"Selected: {hit.collider.gameObject.name}");
            }
        }
    }
}
