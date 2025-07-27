using System;

using UnityEngine;

namespace LevelSystem
{
    [Serializable]
    public abstract class AbstractBuilding : ScriptableObject
    {
        [SerializeField]
        private float _health = 100f;
        public float Health { get { return _health; } }

        [SerializeField]
        private float _shield = 0f;
        public float Shield { get { return _shield; } }

        [SerializeField]
        private bool _isDestructible = true;
        public bool IsDestructible { get { return _isDestructible; } }

        [SerializeField]
        private float _repairVelocity = 5f;
        public float RepairVelocity { get { return _repairVelocity; } }
    }
}