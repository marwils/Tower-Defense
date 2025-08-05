using System;
using UnityEngine;

namespace MarwilsTD.LevelSystem
{
    [CreateAssetMenu(fileName = "New Tower", menuName = "Marwils.TD/Stats/Tower")]
    [Serializable]
    public class TowerConfiguration : EntityConfiguration
    {
        public TowerConfiguration()
        {
            _canTakeDamage = true;
            _canBeHealed = false;
            _health = 200f;
            _shield = 50f;
            _canMove = false;
            _speed = 0f;
        }
    }
}
