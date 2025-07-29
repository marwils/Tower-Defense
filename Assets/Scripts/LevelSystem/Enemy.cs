using System;

using UnityEngine;

namespace LevelSystem
{
        [CreateAssetMenu(fileName = "Enemy", menuName = "Game/Enemy")]
        [Serializable]
        public class Enemy : Entity
        {
                public Enemy()
                {
                        _canTakeDamage = true;
                        _canBeHealed = false;
                        _health = 100f;
                        _shield = 0f;
                        _canMove = true;
                        _speed = 3.5f;
                }
                //protected new bool _canMove = true;
        }
}