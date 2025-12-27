using UnityEngine;

namespace MarwilsTD
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The maximum distance the weapon can shoot (in meters)")]
        private float _range = 5f;

        [SerializeField]
        [Tooltip("How long the weapon should take between shots (in seconds)")]
        private float _fireRate = 0.5f;

        [SerializeField]
        [Tooltip("How long the weapon should take to rotate around the own axis (in seconds) when aiming at a target")]
        private float _rotationDurationPerRevolution = 1f;

        [SerializeField]
        private GameObject _ammoPrefab;

        [SerializeField]
        private Transform _shootPoint;

        [SerializeField]
        private string _targetTag = "Enemy";

        private Transform _currentTarget;

        private float _coolDown;

        private float _aimTime = 0f;
        private float _aimProgress = 0f;

        private Quaternion _initialAimRotation;

        private bool _isNewTarget = true;

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
            }

            if (HasTargetWithinRange)
            {
                if (AimAtTarget() && _coolDown <= 0f)
                {
                    Shoot();
                }
            }
            else
            {
                SeekNewTarget();
            }
        }

        protected void Shoot()
        {
            if (_ammoPrefab != null && _shootPoint != null)
            {
                GameObject ammoInstance = Instantiate(_ammoPrefab, _shootPoint.position, _shootPoint.rotation);
                ammoInstance.GetComponent<AmmoController>()?.Initialize(_shootPoint.forward, _currentTarget.gameObject);

                _coolDown = _fireRate;
            }
        }

        protected void SeekNewTarget()
        {
            // Find the closest target with the specified tag within range

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
            _isNewTarget = true;
        }

        protected bool AimAtTarget()
        {
            if (_currentTarget == null)
                return false;

            Vector3 directionToTarget = (_currentTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget) * Quaternion.identity;

            // Store initial rotation when targeting a new enemy
            if (_isNewTarget)
            {
                _initialAimRotation = transform.rotation;
                float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);
                _aimTime = angleDifference / 360f * _rotationDurationPerRevolution;
                _aimProgress = 0f;
                _isNewTarget = false;
            }

            if (_aimProgress >= 1f)
            {
                transform.rotation = targetRotation;
                return true;
            }

            _aimProgress += Time.deltaTime / _aimTime;
            transform.rotation = Quaternion.Slerp(_initialAimRotation, targetRotation, _aimProgress);

            return false;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}
