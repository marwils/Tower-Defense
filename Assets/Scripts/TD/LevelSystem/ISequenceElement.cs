namespace LevelSystem
{
    public interface ISequenceElement : IRoutine
    {
        void Initialize(IRouteProvider routeProvider);
    }
}