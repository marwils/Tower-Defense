using MarwilsTD.LevelSystem;

using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Node", menuName = "Game/Tower Node")]
public class TowerNode : ScriptableObject
{
    [SerializeField]
    private string _name = "Tower Node";
    public string Name => _name;

    [SerializeField]
    private GameObject _prefab;
    public GameObject Prefab => _prefab;

    [SerializeField]
    private Tower _towerSettings;
    public Tower TowerSettings => _towerSettings;

    [SerializeField]
    private int _price = 100;
    public int Price => _price;

    [SerializeField]
    private TowerNode[] _extensibleBy;
    public TowerNode[] ExtensibleBy => _extensibleBy;
    public bool IsExtensible { get { return _extensibleBy != null && _extensibleBy.Length > 0; } }

    [SerializeField]
    private TowerNode[] _upgradableBy;
    public TowerNode[] UpgradableBy => _upgradableBy;
    public bool IsUpgradable { get { return _upgradableBy != null && _upgradableBy.Length > 0; } }

    [SerializeField]
    private bool _isBase = false;
    public bool IsBase => _isBase;

    [SerializeField]
    private float _height = 1.0f;
    public float Height => _height;

    [SerializeField]
    private float _yOffset;
    public float YOffset => _yOffset;
}