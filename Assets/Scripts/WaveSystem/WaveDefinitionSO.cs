using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "WaveSystem/WaveDefinition")]
public class WaveDefinitionSO : ScriptableObject
{
    [SerializeField]
    public List<WaveElementBase> Sequence;
}