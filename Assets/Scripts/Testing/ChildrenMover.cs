using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Testing
{
    public class ChildrenMover : MonoBehaviour
    {
        [SerializeField]
        private float _amount = 0.1f;

        [SerializeField]
        private Vector3 _vector = Vector3.up;

        [SerializeField]
        private string _modifierKey = "";

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
                    MoveChildren(_amount);
                }
                if (Keyboard.current[Key.NumpadMinus].wasPressedThisFrame)
                {
                    MoveChildren(-_amount);
                }
            }
        }

        private void MoveChildren(float distance)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child != null)
                {
                    child.localPosition += _vector * distance;
                }
            }
        }
    }
}