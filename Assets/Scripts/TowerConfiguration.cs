using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Configuration", menuName = "Game/Tower Configuration")]
public class TowerConfiguration : ScriptableObject
{
    [SerializeField]
    [Tooltip("The base tower node that this configuration is built upon.")]
    private TowerNode _base;
    public TowerNode Base => _base;
}