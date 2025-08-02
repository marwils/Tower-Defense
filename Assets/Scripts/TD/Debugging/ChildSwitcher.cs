using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A script to switch active child GameObjects based on number keys (0-9). Just for testing purposes.
/// </summary>
public class ChildSwitcher : MonoBehaviour
{
    Key[] digitKeys = { Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4,
                   Key.Numpad5, Key.Numpad6, Key.Numpad7, Key.Numpad8, Key.Numpad9 };

    private void Start()
    {
        SwitchChild(0);
    }

    private void Update()
    {
        for (int i = 0; i < digitKeys.Length; i++)
        {
            if (Keyboard.current[digitKeys[i]].wasPressedThisFrame)
            {
                Debug.Log($"Switching to child {i}");
                SwitchChild(i);
            }
        }
    }

    private void SwitchChild(int index)
    {
        int childCount = transform.childCount;
        if (index < 0 || index >= childCount)
        {
            Debug.LogWarning($"Index {index} is out of bounds for child switching.");
            return;
        }
        for (int i = 0; i < childCount; i++)
        {
            Debug.Log($"Setting child {i} active: {i == index}");
            transform.GetChild(i).gameObject.SetActive(i == index);
        }
    }
}
