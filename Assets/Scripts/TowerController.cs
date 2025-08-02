using LevelSystem;

using UnityEngine;

public class TowerController : EntityController
{
    [SerializeField]
    private Tower _towerSettings;
    public Tower TowerSettings => _towerSettings;

    [SerializeField]
    private WeaponController _weaponController;
    public WeaponController WeaponController => _weaponController;

    void Start()
    {

    }

    void Update()
    {

    }

    protected override Tower GetEntitySettings<Tower>()
    {
        return _towerSettings as Tower;
    }
}
