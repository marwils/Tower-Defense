using System;

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ChildSwitcher allows switching between child GameObjects based on numpad key presses.
/// It can be configured to require a modifier key (like P) to activate the switching.
/// The first child is activated on start if _activateOnStart is true.
/// </summary>
namespace Testing
{
    public class ChildSwitcher : MonoBehaviour
    {
        [SerializeField]
        private string _modifierKey = "";

        [SerializeField]
        private bool _activateOnStart = true;

        private static readonly Key[] NumpadKeys = { Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4,
                   Key.Numpad5, Key.Numpad6, Key.Numpad7, Key.Numpad8, Key.Numpad9 };

        private Key _modKey;

        private void Start()
        {
            UpdateModifierKey();
            SwitchChild(_activateOnStart ? 0 : -1);
        }

        private void UpdateModifierKey()
        {
            if (_modifierKey != "")
            {
                _modKey = Enum.Parse<Key>(_modifierKey, true);
            }
            else
            {
                Debug.LogWarning($"Invalid modifier key specified. No modifier key will be used in {gameObject.name}.");
                _modKey = Key.None;
            }
        }

        private void Update()
        {
            if (_modKey == Key.None || Keyboard.current[_modKey].isPressed)
            {
                for (int i = 0; i < NumpadKeys.Length; i++)
                {
                    if (Keyboard.current[NumpadKeys[i]].wasPressedThisFrame)
                    {
                        SwitchChild(i);
                    }
                }
            }
        }

        private void SwitchChild(int index)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == index);
            }
        }
    }
}
