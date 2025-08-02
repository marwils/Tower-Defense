using UnityEngine;
using MarwilsTD.LevelSystem;

namespace MarwilsTD
{
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
            if (IsSpawnPointCorrect(request))
            {
                DoSpawn(request);
            }
        }

        private bool IsSpawnPointCorrect(EnemySpawnRequest request)
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

            EnemyController enemyInstance = Instantiate(request.EnemyPrefab, transform.position, request.EnemyPrefab.transform.rotation);

            enemyInstance.SetDestination(request.TargetTransform);

            Debug.Log($"SpawnPoint '{name}' spawned enemy: {request.EnemyPrefab.name} -> {request.TargetTransform?.name}");
        }
    }
}