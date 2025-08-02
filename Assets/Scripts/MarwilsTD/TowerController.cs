namespace MarwilsTD
{
    public class TowerController : EntityController
    {
        public override Tower GetEntitySettings<Tower>()
        {
            return _entitySettings as Tower;
        }
    }
}