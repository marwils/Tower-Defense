using System;

using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [Serializable]
    /// <summary>
    /// Represents a route in the game level system.
    /// </summary>
    /// <remarks>
    /// This class defines the spawn and target points for entities in a wave sequence.
    /// It is used to manage the flow of entities from spawn to target.
    /// </remarks>
    public class RouteConfiguration : ScriptableObject
    {
        [SerializeField]
        private string _title = "New Route";
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [SerializeField]
        private string _spawnPointId;
        public string SpawnPointId => _spawnPointId;

        [SerializeField]
        private string _targetPointId;
        public string TargetPointId => _targetPointId;

        public Transform SpawnTransform => RouteRegistry.GetSpawnPoint(_spawnPointId);
        public Transform TargetTransform => RouteRegistry.GetTargetPoint(_targetPointId);

        /// <summary>
        /// Checks if the route is valid.
        /// Do not use this method in the editor, as it may not be initialized properly.
        /// </summary>
        /// <returns>True if spawn point and target point are assigned, false otherwise.</returns>
        public bool IsValid => SpawnTransform != null && TargetTransform != null;
    }
}