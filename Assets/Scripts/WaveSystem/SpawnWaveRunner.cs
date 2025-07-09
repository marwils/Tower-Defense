using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaveRunner : MonoBehaviour
{
    public SpawnPoint assignedSpawnPoint;

    private Queue<WaveElementBase> queue = new();

    private bool running = false;

    public Transform Target;

    public void Enqueue(WaveElementBase element)
    {
        queue.Enqueue(element);
        if (!running) StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        running = true;

        while (queue.Count > 0)
        {
            var element = queue.Dequeue();
            if (element is IRouteAware aware)
            {
                aware.AssignSpawnPoint(assignedSpawnPoint);
                aware.AssignTarget(Target);
            }

            bool done = false;
            element.OnComplete.AddListener(() => done = true);
            element.StartElement();
            yield return new WaitUntil(() => done);
            element.OnComplete.RemoveAllListeners(); // cleanup
        }

        running = false;
    }
}
