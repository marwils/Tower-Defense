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

    private void Start()
    {
        foreach (WaveRoute route in _waveRoutes)
        {
            foreach (WaveElementBase element in route.wave.sequence)
            {
                route.runner.Enqueue(element);
            }
        }
    }
}
