using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Testing
{
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField]
        private string _modifierKey = "";

        [SerializeField]
        private float _amount = 0.1f;

        [SerializeField]
        private Vector3 _vector = Vector3.up;

        private Key _modKey;

        private void Start()
        {
            UpdateModifierKey();
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