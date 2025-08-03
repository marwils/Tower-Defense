namespace MarwilsTD.LevelSystem
{
    [System.Serializable]
    public abstract class SequenceElementConfiguration : Runner, ISequenceElement
    {
        protected IRouteProvider _routeProvider;

        public virtual void Initialize(IRouteProvider routeProvider)
        {
            _routeProvider = routeProvider;
        }
    }

}