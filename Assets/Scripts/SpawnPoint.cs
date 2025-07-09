using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void DoSpawn(Enemy enemy, Transform target)
    {
        GameObject enemyInstance = Instantiate(enemy.gameObject, transform.position, enemy.transform.rotation);
        enemyInstance.GetComponent<Enemy>().Target = target;
        enemyInstance.GetComponent<Enemy>().FindTarget();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
}
