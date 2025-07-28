using UnityEngine;

public abstract class AbstractPoint : MonoBehaviour
{
    private const float GizmoRadius = 0.5f;

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = GetGizmoColor();
        Gizmos.DrawWireSphere(transform.position, GizmoRadius);
    }

    protected virtual Color GetGizmoColor()
    {
        return Color.blue;
    }
}
