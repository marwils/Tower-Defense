using UnityEngine;

namespace MarwilsTD
{
    public abstract class EntityController : MonoBehaviour
    {
        [Header("Damage and Healing")]
        [SerializeField]
        private bool _canTakeDamage = true;
        public bool CanTakeDamage => _canTakeDamage;

        [SerializeField]
        private bool _canBeHealed = false;
        public bool CanBeHealed => _canBeHealed;

        [SerializeField]
        private float _health;
        public float Health => _health;
        public bool IsAlive
        {
            get { return _health > 0; }
        }

        private float _initialHealth;
        public float InitialHealth
        {
            get { return _initialHealth; }
        }

        protected virtual void Awake()
        {
            _initialHealth = _health;
        }

        public void TakeDamage(float amount)
        {
            if (_canTakeDamage)
            {
                _health -= amount;
            }
        }

        public void Heal(float amount)
        {
            if (_canBeHealed)
            {
                _health += amount;

                if (_health > _initialHealth)
                {
                    _health = _initialHealth;
                }
            }
        }
    }
}
