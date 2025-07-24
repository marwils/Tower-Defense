using System.Collections.Generic;

using UnityEngine;

public class MasterWaveController : MonoBehaviour
{
    [System.Serializable]
    public class WaveRoute
    {
        [Tooltip("Spawner that runs the wave sequence.")]
        public SpawnWaveRunner Spawner;

        [Tooltip("The wave definition to spawn.")]
        public WaveDefinitionSO Wave;
    }

    [SerializeField]
    private List<WaveRoute> _waveRoutes;

    private void Start()
    {
        foreach (WaveRoute route in _waveRoutes)
        {
            foreach (WaveElementBase element in route.Wave.Sequence)
            {
                route.Spawner.Enqueue(element);
            }
        }
    }
}
