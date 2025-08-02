using MarwilsTD.LevelSystem;

namespace MarwilsTD
{
    public class TowerController : EntityController
    {
        protected new Tower _entitySettings;

        public override Tower GetEntitySettings<Tower>()
        {
            return _entitySettings as Tower;
        }
    }
}