using MarwilsTD.LevelSystem;

using UnityEngine;

namespace MarwilsTD
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Level _level;

        private void Start()
        {
            StartLevel();
        }

        private void StartLevel()
        {
            _level.StartLevel();
        }
    }
}