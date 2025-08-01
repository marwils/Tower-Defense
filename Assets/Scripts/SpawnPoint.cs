using UnityEngine;
using LevelSystem;

public class SpawnPoint : GizmoBehaviour
{
    private void Start()
    {
        RouteRegistry.RegisterSpawnPoint(transform);
        EnemySpawnEvent.OnEnemySpawnRequested += HandleEnemySpawnRequest;
    }

    private void OnDestroy()
    {
        RouteRegistry.UnregisterSpawnPoint(transform);
        EnemySpawnEvent.OnEnemySpawnRequested -= HandleEnemySpawnRequest;
    }

    protected override Color GetGizmoColor()
    {
        return Color.green;
    }

    private void HandleEnemySpawnRequest(EnemySpawnRequest request)
    {
        if (IsCorrectSpawn(request))
        {
            DoSpawn(request);
        }
    }

    private bool IsCorrectSpawn(EnemySpawnRequest request)
    {
        return request.SpawnTransform == transform;
    }


    private void DoSpawn(EnemySpawnRequest request)
    {
        if (request.EnemyPrefab == null)
        {
            Debug.LogError($"SpawnPoint '{name}': Enemy prefab is null!");
            return;
        }

        EnemyControl enemyInstance = Instantiate(request.EnemyPrefab, transform.position, request.EnemyPrefab.transform.rotation);

        enemyInstance.SetDestination(request.TargetTransform);

        Debug.Log($"SpawnPoint '{name}' spawned enemy: {request.EnemyPrefab.name} -> {request.TargetTransform?.name}");
    }
}