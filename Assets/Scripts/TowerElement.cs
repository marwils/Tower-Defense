using MarwilsTD.LevelSystem;

using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Element", menuName = "Game/Tower Element")]
public class TowerElement : ScriptableObject
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
    private TowerElement[] _isExtensibleBy;
    public TowerElement[] IsExtensibleBy => _isExtensibleBy;

    [SerializeField]
    private TowerElement[] _isUpgradableBy;
    public TowerElement[] IsUpgradableBy => _isUpgradableBy;

    [SerializeField]
    public bool IsExtensible { get { return _isExtensibleBy != null && _isExtensibleBy.Length > 0; } }

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