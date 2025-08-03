using System;

using UnityEngine;

namespace MarwilsTD
{
    using LevelSystem;

    public class WeaponController : MonoBehaviour
    {
        [SerializeField]
        private WeaponConfiguration _weaponSettings;
        public WeaponConfiguration WeaponSettings => _weaponSettings;

        [SerializeField]
        private AmmoConfiguration _ammoSettings;
        public AmmoConfiguration AmmoSettings => _ammoSettings;

        [SerializeField]
        private Transform _aimTarget;
        public Transform AimTarget { get => _aimTarget; }

        [SerializeField]
        private Transform _firePoint;
        public Transform FirePoint => _firePoint;

        [SerializeField]
        private int _currentAmmo;

        private string _targetTag;
        public string TargetTag { get => _targetTag; set { _targetTag = value; FindTarget(); } }

        protected void FindTarget()
        {
            if (_aimTarget == null)
            {
                _aimTarget = GameObject.FindGameObjectWithTag(TargetTag)?.transform;
                if (_aimTarget == null)
                {
                    Debug.LogWarning($"No target found with tag <{TargetTag}> in <{gameObject.name}>.");
                }
            }
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void SeekTarget()
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public bool HasAmmo()
        {
            return _currentAmmo > 0;
        }
    }
}