using UnityEngine;

namespace MarwilsTD
{
    using LevelSystem;

    public class AmmoControl : MonoBehaviour
    {
        [SerializeField]
        private AmmoConfiguration _ammoConfiguration;
        public AmmoConfiguration AmmoConfiguration => _ammoConfiguration;

        [SerializeField]
        private int _currentAmmo;
        public int CurrentAmmo => _currentAmmo;

        public void Reload()
        {
            _currentAmmo = _ammoConfiguration.MaxAmmo;
        }

        public bool HasAmmo()
        {
            return _currentAmmo > 0;
        }

        public void UseAmmo()
        {
            if (HasAmmo())
            {
                _currentAmmo--;
            }
        }
    }
}