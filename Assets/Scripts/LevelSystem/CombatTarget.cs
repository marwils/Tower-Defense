using System;

using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public abstract class CombatTarget : ScriptableObject
    {
        [SerializeField]
        private bool _canTakeDamage;
        public bool CanTakeDamage => _canTakeDamage;

        [SerializeField]
        private bool _canBeHealed;
        public bool CanBeHealed => _canBeHealed;

        [SerializeField]
        private float _health;
        public float Health => _health;
    }
}