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
        [Tooltip("Modifier key to activate the action tester. If empty, no modifier key is used.")]
        private Key _modifierKey = Key.None;

        [SerializeField]
        private bool _activateOnStart = true;

        private static readonly Key[] NumpadKeys =
        {
            Key.Numpad0,
            Key.Numpad1,
            Key.Numpad2,
            Key.Numpad3,
            Key.Numpad4,
            Key.Numpad5,
            Key.Numpad6,
            Key.Numpad7,
            Key.Numpad8,
            Key.Numpad9,
        };

        private void Start()
        {
            SwitchChild(_activateOnStart ? 0 : -1);
        }

        private void Update()
        {
            if (_modifierKey == Key.None || Keyboard.current[_modifierKey].isPressed)
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
