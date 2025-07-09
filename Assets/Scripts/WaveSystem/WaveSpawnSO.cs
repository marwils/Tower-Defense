using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveSystem/WaveSpawn")]
public class WaveSpawnSO : WaveElementBase, IRouteAware
{
    [SerializeField]
    public List<Enemy> _enemies;
    [SerializeField]
    public float _spawnInterval = 0.5f;

    [System.NonSerialized] private SpawnPoint _assignedSpawn;
    [System.NonSerialized] private Transform _target;

    public void AssignSpawnPoint(SpawnPoint spawnPoint)
    {
        _assignedSpawn = spawnPoint;
    }

    public void AssignTarget(Transform target)
    {
        _target = target;
    }

    public override void StartElement()
    {
        CoroutineRunner.Instance.StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        foreach (var enemy in _enemies)
        {
            _assignedSpawn.DoSpawn(enemy, _target);
            yield return new WaitForSeconds(_spawnInterval);
        }

        onComplete?.Invoke();
    }
}