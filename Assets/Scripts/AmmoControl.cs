using LevelSystem;

using UnityEngine;

public class AmmoControl : MonoBehaviour
{
    [SerializeField]
    private Ammo _ammoSettings;
    public Ammo AmmoSettings => _ammoSettings;

    [SerializeField]
    private int _currentAmmo;
    public int CurrentAmmo => _currentAmmo;

    public void Initialize(Ammo ammo)
    {
        _ammoSettings = ammo;
        _currentAmmo = ammo.MaxAmmo;
    }

    public void Reload()
    {
        _currentAmmo = _ammoSettings.MaxAmmo;
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