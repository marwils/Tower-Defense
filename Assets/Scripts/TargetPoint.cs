public class TargetPoint : AbstractPoint
{
    private void Start()
    {
        RouteRegistry.RegisterTargetPoint(transform);
    }

    private void OnDestroy()
    {
        RouteRegistry.UnregisterTargetPoint(transform);
    }


}