using MarwilsTD.LevelSystem;

using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Element Configuration", menuName = "Game/Tower Element Configuration")]
public class TowerElementConfiguration : ScriptableObject
{
    [SerializeField]
    private string _name = "Tower Element";
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
    private TowerElementConfiguration[] _isExtensibleBy;
    public TowerElementConfiguration[] IsExtensibleBy => _isExtensibleBy;

    [SerializeField]
    private TowerElementConfiguration[] _isUpgradableBy;
    public TowerElementConfiguration[] IsUpgradableBy => _isUpgradableBy;

    [SerializeField]
    public bool IsExtensible { get { return _isExtensibleBy != null && _isExtensibleBy.Length > 0; } }

    [SerializeField]
    private bool _isBase = false;
    public bool IsBase => _isBase;

    [SerializeField]
    private float _height = 1.0f;
    public float Height => _height;

    [SerializeField]
    private float _yOffset = 1.0f;
    public float YOffset => _yOffset;
}