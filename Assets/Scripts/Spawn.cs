using UnityEngine;

public class Spawn : MonoBehaviour
{
    public void DoSpawn()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
}
