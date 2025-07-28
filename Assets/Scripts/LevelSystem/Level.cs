using System.Collections.Generic;

using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
    public class Level : ScriptableObject
    {
        [SerializeField] private string _title;
        public string Title => _title;

        [SerializeField] private List<Wave> _waves = new();
        public List<Wave> Waves => _waves;

        public void StartLevel()
        {
            Debug.Log($"Starting level: {_title}");
            foreach (var wave in _waves)
            {
                wave.StartWave();
            }
        }
    }
}
