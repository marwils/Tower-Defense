using System;

using UnityEngine;

namespace LevelSystem
{
        [CreateAssetMenu(fileName = "Weapon", menuName = "Game/Weapon")]
        [Serializable]
        public class Weapon : Entity
        {
                public Weapon()
                {
                        _canTakeDamage = true;
                        _canBeHealed = false;
                        _health = 100f;
                        _shield = 0f;
                        _canMove = false; // Weapons typically do not move
                        _speed = 0f; // No movement speed for weapons
                }

                [Header("Weapon")]

                [SerializeField]
                [Tooltip("The amount of damage the weapon deals")]
                private float _damage = 10f;
                public float Damage { get { return _damage; } }

                [SerializeField]
                [Range(0, 100)]
                [Tooltip("The maximum distance the weapon can shoot (in meters)")]
                private float _range = 15f;
                public float Range { get { return _range; } }

                [SerializeField]
                [Range(0, 5)]
                [Tooltip("How long the weapon should take between shots (in seconds)")]
                private float _fireRate = 0.5f;
                public float FireRate { get { return _fireRate; } }

                [SerializeField]
                [Range(0, 10)]
                [Tooltip("How long the weapon should take to reload (in seconds)")]
                private float _reloadTime = 2f;
                public float ReloadTime { get { return _reloadTime; } }

                [SerializeField]
                [Range(0, 100)]
                [Tooltip("After how many shots the weapon should reload (0 = infinite)")]
                private int _magazineSize = 0;
                public int MagazineSize { get { return _magazineSize; } }

                [SerializeField]
                [Range(0, 5)]
                [Tooltip("How long the weapon should take to seek a target (in seconds) (0 = no seeking)")]
                private float _seekTime = .2f;
                public float SeekTime { get { return _seekTime; } }

                [SerializeField]
                private TargetStrategy _targetStrategy;
                public TargetStrategy TargetStrategy => _targetStrategy;

                [SerializeField]
                private KeepTargetStrategy _keepTargetStrategy;
                public KeepTargetStrategy KeepTargetStrategy => _keepTargetStrategy;
        }
}