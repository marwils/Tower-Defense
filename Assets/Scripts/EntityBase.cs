using MarwilsTD.LevelSystem;

using UnityEngine;
using UnityEngine.Events;

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField]
    private Entity _settings;

    [Header("Damage and Healing")]

    [SerializeField]
    protected bool _canTakeDamage = true;
    public bool CanTakeDamage => _canTakeDamage;

    [SerializeField]
    protected bool _canBeHealed = true;
    public bool CanBeHealed => _canBeHealed;

    [SerializeField]
    protected float _health = 100f;
    public float Health => _health;

    [SerializeField]
    protected float _shield = 0f;
    public float Shield => _shield;

    private UnityEvent _onDie = new();
    public UnityEvent OnDie => _onDie;

    protected virtual void Awake()
    {
        if (_settings == null)
        {
            Debug.LogWarning($"Building settings not assigned in <{gameObject.name}>.");
            return;
        }

        _canTakeDamage = _settings.CanTakeDamage;
        _canBeHealed = _settings.CanBeHealed;
        _health = _settings.Health;
        _shield = _settings.Shield;
        InitState();
    }

    protected abstract void InitState();

    public virtual void TakeDamage(float damage)
    {
        if (_canTakeDamage && damage > 0)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _onDie?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    public T GetSettings<T>() where T : Entity
    {
        return _settings as T;
    }

    public abstract void OnSelect();
}
