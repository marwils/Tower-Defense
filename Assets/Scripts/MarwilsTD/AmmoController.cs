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
        public float Lifetime
        {
            get { return _lifetime; }
        }

        [SerializeField]
        private string _targetTag;
        public string TargetTag
        {
            get { return _targetTag; }
        }

        void Update()
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void Initialize(Vector3 startDirection, string targetTag)
        {
            GetComponent<Rigidbody>().AddForce(startDirection * _speed, ForceMode.Impulse);
            _targetTag = targetTag;
        }

        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}
