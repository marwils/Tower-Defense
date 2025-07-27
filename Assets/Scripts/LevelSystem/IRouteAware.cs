using UnityEngine;

namespace LevelSystem
{
    public interface IRouteAware
    {
        Transform SpawnTransform { get; set; }
        Transform TargetTransform { get; set; }

    }
}