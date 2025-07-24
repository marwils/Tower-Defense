using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpawnWaveRunner : MonoBehaviour
{
    [SerializeField]
    private SpawnPoint _assignedSpawnPoint;

    [SerializeField]
    private Transform _assignedTarget;

    private Queue<WaveElementBase> _queue = new();

    private bool _running = false;


    public void Enqueue(WaveElementBase element)
    {
        _queue.Enqueue(element);
        if (!_running) StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        _running = true;

        while (_queue.Count > 0)
        {
            var element = _queue.Dequeue();
            if (element is IRouteAware aware)
            {
                aware.AssignSpawnPoint(_assignedSpawnPoint);
                aware.AssignTarget(_assignedTarget);
            }

            bool done = false;
            element.OnComplete.AddListener(() => done = true);
            element.StartElement();
            yield return new WaitUntil(() => done);
            element.OnComplete.RemoveAllListeners(); // cleanup
        }

        _running = false;
    }
}
