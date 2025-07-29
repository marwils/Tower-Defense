using UnityEngine;

public class GizmoBehaviour : MonoBehaviour
{
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = GetGizmoColor();
        Gizmos.DrawWireSphere(transform.position, GetGizmoRadius());
    }

    protected virtual Color GetGizmoColor()
    {
        return Color.blue;
    }

    protected virtual float GetGizmoRadius()
    {
        return 0.5f;
    }
}
