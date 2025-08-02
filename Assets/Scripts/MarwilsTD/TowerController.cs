namespace MarwilsTD
{
    using UnityEngine;

    public class TowerController : EntityController
    {
        [SerializeField]
        private WeaponController _weaponController;
        public WeaponController WeaponController => _weaponController;

        protected override Tower GetEntitySettings<Tower>()
        {
            return _entitySettings as Tower;
        }
    }
}