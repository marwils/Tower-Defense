using System;

using UnityEngine;

namespace LevelSystem
{
        [CreateAssetMenu(fileName = "Tower", menuName = "Game/Tower")]
        [Serializable]
        public class Tower : Entity
        {
                public Tower()
                {
                        _canTakeDamage = true;
                        _canBeHealed = false;
                        _health = 200f;
                        _shield = 50f;
                        _canMove = false; // Towers typically do not move
                        _speed = 0f; // No movement speed for towers
                }
        }
}