using LevelSystem;

using UnityEngine;

public class LevelControl : MonoBehaviour
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
