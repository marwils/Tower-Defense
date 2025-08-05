using UnityEngine;

namespace MarwilsTD
{
    using LevelSystem;

    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private LevelConfiguration _level;

        private void Start()
        {
            StartLevel();
        }

        private void StartLevel()
        {
            if (_level == null)
            {
                Debug.LogWarning($"LevelConfiguration is not set in LevelController <{this}>.");
                return;
            }

            _level.StartLevel();
        }
    }
}
