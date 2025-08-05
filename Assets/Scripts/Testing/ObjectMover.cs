using UnityEngine;
using UnityEngine.InputSystem;

namespace Testing
{
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Modifier key to activate the action tester. If empty, no modifier key is used.")]
        private Key _modifierKey = Key.None;

        [SerializeField]
        private float _amount = 0.1f;

        [SerializeField]
        private Vector3 _vector = Vector3.up;

        private void Update()
        {
            if (_modifierKey == Key.None || Keyboard.current[_modifierKey].isPressed)
            {
                if (Keyboard.current[Key.NumpadPlus].wasPressedThisFrame)
                {
                    Move(_amount);
                }
                if (Keyboard.current[Key.NumpadMinus].wasPressedThisFrame)
                {
                    Move(-_amount);
                }
            }
        }

        private void Move(float distance)
        {
            transform.localPosition += _vector * distance;
        }
    }
}
