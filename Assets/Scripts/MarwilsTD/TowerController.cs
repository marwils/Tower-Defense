using UnityEngine;

namespace MarwilsTD
{
    public class TowerController : EntityController
    {
        [Header("Weapon Settings")]
        [SerializeField]
        private float _range;
        public float Range => _range;
    }
}
