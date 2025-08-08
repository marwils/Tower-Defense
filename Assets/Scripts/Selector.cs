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

    private void HandleSelect(ISelectable selectable)
    {
        selectable.OnSelect();
    }
}
