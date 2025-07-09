using System.Collections.Generic;
using UnityEngine;

public class MasterWaveController : MonoBehaviour
{
    [System.Serializable]
    public class WaveRoute
    {
        public SpawnWaveRunner runner;
        public WaveDefinitionSO wave;
    }

    [SerializeField]
    private List<WaveRoute> _waveRoutes;

    [SerializeField]
    private Transform _target;

    private void Start()
    {
        foreach (WaveRoute route in _waveRoutes)
        {
            route.runner.Target = _target;

            foreach (WaveElementBase element in route.wave.sequence)
            {
                route.runner.Enqueue(element);
            }
        }
    }
}
