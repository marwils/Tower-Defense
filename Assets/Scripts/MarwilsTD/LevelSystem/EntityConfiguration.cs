using System;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [Serializable]
    public abstract class EntityConfiguration : ScriptableObject
    {
        [Header("Damage and Healing")]

        [SerializeField]
        protected bool _canTakeDamage;
        public bool CanTakeDamage => _canTakeDamage;

        [SerializeField]
        protected bool _canBeHealed;
        public bool CanBeHealed => _canBeHealed;

        [SerializeField]
        protected float _health = 100f;
        public float Health => _health;

        [SerializeField]
        protected float _shield = 0f;
        public float Shield => _shield;

        [Header("Movement")]

        [SerializeField]
        protected bool _canMove = false;
        public bool CanMove => _canMove;

        [SerializeField]
        protected float _speed = 0f;
        public float Speed => _speed;
    }
}