using System;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "New Ammo", menuName = "Game/Ammo")]
    [Serializable]
    public class Ammo : ScriptableObject
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
