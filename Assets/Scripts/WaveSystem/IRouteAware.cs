using UnityEngine;

public interface IRouteAware
{
    void AssignSpawnPoint(SpawnPoint spawnPoint);
    void AssignTarget(Transform target);
}