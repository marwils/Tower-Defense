namespace LevelSystem
{
    [System.Serializable]
    public abstract class SequenceElement : Runner, ISequenceElement
    {
        protected IRouteProvider _routeProvider;

        public virtual void Initialize(IRouteProvider routeProvider)
        {
            _routeProvider = routeProvider;
        }
    }

}