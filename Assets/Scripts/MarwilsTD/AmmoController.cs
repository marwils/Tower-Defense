using System;
using UnityEngine;

namespace MarwilsTD
{
    [RequireComponent(typeof(Rigidbody))]
    public class AmmoController : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The amount of damage the ammo deals")]
        private float _damage = 10f;
        public float Damage
        {
            get { return _damage; }
        }

        [SerializeField]
        private float _speed = 5f;
        public float Speed
        {
            get { return _speed; }
        }

        [SerializeField]
        private float _lifetime = 5f;

        private string _targetTag;
        public string TargetTag
        {
            get { return _targetTag; }
        }

        private bool _isFired = false;

        void Update()
        {
            if (!_isFired)
            {
                return;
            }

            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void Fire(Vector3 startDirection, string targetTag)
        {
            GetComponent<Rigidbody>().AddForce(startDirection * _speed, ForceMode.Impulse);
            _targetTag = targetTag;
            _isFired = true;
        }

        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}
