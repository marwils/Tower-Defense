using UnityEngine;
using UnityEngine.Events;

namespace LevelSystem
{
    public interface ISequenceElement
    {
        void StartElement(Vector3 spawnPoint);

        UnityEvent OnComplete { get; }
    }
}