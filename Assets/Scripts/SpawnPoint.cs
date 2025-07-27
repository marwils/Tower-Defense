using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public void DoSpawn(Enemy enemy, Transform destination)
    {
        GameObject enemyInstance = Instantiate(enemy.gameObject, transform.position, enemy.transform.rotation);
        enemyInstance.GetComponent<Enemy>().Destination = destination;
        enemyInstance.GetComponent<Enemy>().FindDestination();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
}
