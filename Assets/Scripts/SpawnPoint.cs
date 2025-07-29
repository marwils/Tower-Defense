using UnityEngine;

public class SpawnPoint : GizmoBehaviour
{
    private void Start()
    {
        RouteRegistry.RegisterSpawnPoint(transform);
    }

    private void OnDestroy()
    {
        RouteRegistry.UnregisterSpawnPoint(transform);
    }

    protected override Color GetGizmoColor()
    {
        return Color.green;
    }

    public void DoSpawn(EnemyControl enemyPrefab, Transform destination)
    {
        EnemyControl enemyInstance = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation);
        enemyInstance.SetDestination(destination);
    }
}