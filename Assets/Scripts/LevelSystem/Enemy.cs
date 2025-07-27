using System;

using UnityEngine;

namespace LevelSystem
{
        [CreateAssetMenu(fileName = "Enemy", menuName = "Game/Enemy")]
        [Serializable]
        public class Enemy : ScriptableObject
        {
                [SerializeField]
                private float _health = 100f;
                public float Health { get { return _health; } }

                [SerializeField]
                private GameObject _defaultEnemyPrefab;
                public GameObject DefaultEnemyPrefab { get { return _defaultEnemyPrefab; } }

                [SerializeField]
                private GameObject _armedEnemyPrefab;
                public GameObject ArmedEnemyPrefab { get { return _armedEnemyPrefab; } }
        }
}