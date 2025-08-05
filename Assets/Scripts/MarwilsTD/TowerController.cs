namespace MarwilsTD
{
    using LevelSystem;

    public class TowerController : EntityController
    {
        public new TowerConfiguration GetConfiguration()
        {
            return _configuration as TowerConfiguration;
        }

        protected void SetConfiguration(TowerConfiguration towerConfiguration)
        {
            _configuration = towerConfiguration;
            InitializeEntity();
        }
    }
}
