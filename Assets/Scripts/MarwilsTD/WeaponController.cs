using System;
using UnityEngine;

namespace MarwilsTD
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField]
        private string _targetTag;

        [SerializeField]
        [Tooltip("The maximum distance the weapon can shoot (in meters)")]
        private float _range = 5f;

        [SerializeField]
        [Tooltip("How long the weapon should take between shots (in seconds)")]
        private float _fireRate = 0.5f;

        [SerializeField]
        [Tooltip(
            "Time taken to reload ammo (in seconds); if set to any value greater than 0, ammo will be visible at "
                + "muzzle point after reload; should not be greater than fire rate."
        )]
        private float _reloadTime = 0f;
        public bool RequiresReloading => _reloadTime > 0f;

        [SerializeField]
        [Tooltip("Defines how long the weapon takes to complete one full rotation (in seconds)")]
        private float _rotationDurationPerRevolution = 1f;

        [SerializeField]
        private GameObject _ammoPrefab;

        [SerializeField]
        private Transform[] _muzzlePoints;

        [SerializeField]
        [Tooltip(
            "Optional: The transform that tilts or elevates when aiming (e.g., the turret head or barrel). If not "
                + "set, only the base object will rotate."
        )]
        private Transform _tiltTransform;

        [SerializeField]
        private bool _useAnimator = false;

        private Transform _currentTarget;

        private bool _isTargetInitialized = false;

        private float _aimingTime = 0f;

        private float _aimingProgress = 0f;

        private float _coolDown = 0f;

        private int _nextMuzzleIndex = 0;

        private Quaternion _initialYawRotation;

        private Quaternion _initialTiltRotation;

        private bool _ammoLoaded = false;

        private GameObject _ammoInstance;

        public void Start()
        {
            if (RequiresReloading)
            {
                InstantiateAmmoAtMuzzle();
                _ammoLoaded = true;
            }
        }

        public bool HasTargetWithinRange
        {
            get
            {
                if (_currentTarget == null)
                    return false;

                float distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position);
                return distanceToTarget <= _range;
            }
        }

        private void Update()
        {
            if (_coolDown > 0)
            {
                _coolDown -= Time.deltaTime;

                if (RequiresReloading)
                {
                    if (!_ammoLoaded && _coolDown <= _fireRate - _reloadTime)
                    {
                        InstantiateAmmoAtMuzzle();
                    }
                }
            }

            if (HasTargetWithinRange)
            {
                AimAtTarget();

                if (_ammoLoaded)
                {
                    UpdateAmmoPositionAndRotation();
                }

                if (_aimingProgress >= 1f && _coolDown <= 0f)
                {
                    if (_useAnimator)
                    {
                        Animator animator = GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.SetTrigger("Shoot");
                        }
                        else
                        {
                            Debug.LogWarning(
                                $"WeaponController <{name}>: No Animator component found, cannot use animation to shoot."
                            );
                            Shoot();
                        }
                    }
                    else
                    {
                        Shoot();
                    }
                }
            }
            else
            {
                SeekNewTarget();
            }
        }

        protected void Shoot()
        {
            if (_ammoPrefab != null)
            {
                if (_muzzlePoints.Length == 0)
                {
                    Debug.LogWarning($"WeaponController <{name}>: No muzzle points assigned. Cannot shoot ammo.");
                    return;
                }

                if (!RequiresReloading && !_ammoLoaded)
                {
                    InstantiateAmmoAtMuzzle();
                }

                _ammoInstance.GetComponent<AmmoController>()?.Fire(_muzzlePoints[_nextMuzzleIndex].forward, _targetTag);
                _nextMuzzleIndex = (_nextMuzzleIndex + 1) % _muzzlePoints.Length;

                _ammoLoaded = false;

                _coolDown = _fireRate;
            }
        }

        private void InstantiateAmmoAtMuzzle()
        {
            Debug.Log($"WeaponController <{name}>: Reloading ammo at muzzle point.");
            _ammoInstance = Instantiate(
                _ammoPrefab,
                _muzzlePoints[_nextMuzzleIndex].position,
                _muzzlePoints[_nextMuzzleIndex].rotation
            );
            _ammoLoaded = true;
        }

        private void UpdateAmmoPositionAndRotation()
        {
            Debug.Log($"WeaponController <{name}>: Updating ammo position and rotation at muzzle point.");
            _ammoInstance.transform.position = _muzzlePoints[_nextMuzzleIndex].position;
            _ammoInstance.transform.rotation = _muzzlePoints[_nextMuzzleIndex].rotation;
        }

        /// <summary>
        /// Finds a new target within range and sets it as the current target.
        /// </summary>
        protected void SeekNewTarget()
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(_targetTag);
            float closestDistance = Mathf.Infinity;
            Transform closestTarget = null;

            foreach (GameObject target in potentialTargets)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (distanceToTarget < closestDistance && distanceToTarget <= _range)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = target.transform;
                }
            }

            _currentTarget = closestTarget;
            _isTargetInitialized = false;
        }

        protected void AimAtTarget()
        {
            if (_currentTarget == null)
                return;

            Vector3 directionToTarget = (_currentTarget.position - transform.position).normalized;

            // Calculate yaw rotation (Y-axis only) for the base object
            Vector3 flatDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z).normalized;
            Quaternion targetYawRotation = Quaternion.LookRotation(flatDirection);

            // Calculate tilt/pitch rotation (X-axis) for the tilt transform
            float pitch = -Mathf.Asin(directionToTarget.y) * Mathf.Rad2Deg;
            Quaternion targetTiltRotation = Quaternion.Euler(pitch, 0f, 0f);

            if (!_isTargetInitialized)
            {
                InitializeTarget(targetYawRotation);
                _isTargetInitialized = true;
            }

            if (_aimingProgress >= 1f)
            {
                transform.rotation = targetYawRotation;
                if (_tiltTransform != null)
                {
                    _tiltTransform.localRotation = targetTiltRotation;
                    UpdateMuzzlePointsLookDirection(directionToTarget);
                }
                else
                {
                    UpdateMuzzlePointsLookDirection(flatDirection);
                }
                return;
            }

            _aimingProgress += Time.deltaTime / _aimingTime;
            transform.rotation = Quaternion.Slerp(_initialYawRotation, targetYawRotation, _aimingProgress);

            if (_tiltTransform != null)
            {
                _tiltTransform.localRotation = Quaternion.Slerp(
                    _initialTiltRotation,
                    targetTiltRotation,
                    _aimingProgress
                );
            }
        }

        private void InitializeTarget(Quaternion targetYawRotation)
        {
            _initialYawRotation = transform.rotation;
            _initialTiltRotation = _tiltTransform != null ? _tiltTransform.localRotation : Quaternion.identity;

            float yawAngle = Quaternion.Angle(transform.rotation, targetYawRotation);

            _aimingTime = yawAngle / 360f * _rotationDurationPerRevolution;
            _aimingProgress = 0f;
        }

        private void UpdateMuzzlePointsLookDirection(Vector3 targetLookDirection)
        {
            foreach (Transform muzzlePoint in _muzzlePoints)
            {
                muzzlePoint.rotation = Quaternion.LookRotation(targetLookDirection);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}
