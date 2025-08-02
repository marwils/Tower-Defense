using MarwilsTD.LevelSystem;

namespace MarwilsTD
{
    public class TowerController : EntityController
    {
        public new Tower GetEntitySettings()
        {
            return _entitySettings as Tower;
        }

        protected void SetEntitySettings(Tower towerSettings)
        {
            _entitySettings = towerSettings;
            InitializeEntity();
        }
    }
}