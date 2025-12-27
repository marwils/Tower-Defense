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

        [SerializeField]
        private float _lifetime = 5f;

        private GameObject _target;
        public GameObject Target
        {
            get { return _target; }
        }

        void Update()
        {
            _lifetime -= Time.deltaTime;
            if (_lifetime <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void Initialize(Vector3 startDirection, GameObject target)
        {
            GetComponent<Rigidbody>().AddForce(startDirection * _speed, ForceMode.Impulse);
            _target = target;
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
