namespace MarwilsTD
{
    public class TargetPoint : GizmoBehaviour
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
}