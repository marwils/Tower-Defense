using System;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [CreateAssetMenu(fileName = "New Ammo", menuName = "Marwils.TD/Stats/Ammo")]
    [Serializable]
    public class AmmoConfiguration : ScriptableObject
    {
        [SerializeField]
        private int _damage;
        public int Damage => _damage;

        [SerializeField]
        private float _reloadTime;
        public float ReloadTime => _reloadTime;

        [SerializeField]
        private int _maxAmmo;
        public int MaxAmmo => _maxAmmo;
    }
}
