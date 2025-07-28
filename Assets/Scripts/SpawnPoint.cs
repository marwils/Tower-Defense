using UnityEngine;

public class SpawnPoint : AbstractPoint
{
    private void Start()
    {
        RouteRegistry.RegisterSpawnPoint(transform);
    }

    private void OnDestroy()
    {
        RouteRegistry.UnregisterSpawnPoint(transform);
    }

    public void DoSpawn(Enemy enemy, Vector3 destination)
    {
        GameObject enemyInstance = Instantiate(enemy.gameObject, transform.position, enemy.transform.rotation);
        enemyInstance.GetComponent<Enemy>().Destination = destination;
        enemyInstance.GetComponent<Enemy>().FindDestination();
    }

    protected override Color GetGizmoColor()
    {
        return Color.green;
    }
}