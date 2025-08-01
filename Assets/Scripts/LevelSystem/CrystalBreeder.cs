using System;

using UnityEngine;

namespace LevelSystem
{
        [CreateAssetMenu(fileName = "New Crystal Breeder", menuName = "Game/Building/Crystal Breeder")]
        [Serializable]
        public class CrystalBreeder : Entity
        {
                [Header("Crystal Breeding")]

                [SerializeField]
                private float _breedingTime = 5f;
                public float BreedingTime { get { return _breedingTime; } }

                [SerializeField]
                [Range(1, 8)]
                private int _maxCrystals = 4;
                public int MaxCrystals { get { return _maxCrystals; } }
        }
}