using MarwilsTD;
using MarwilsTD.LevelSystem;

using UnityEngine;

public class TowerNode : TowerController
{
    [Header("Tower Node")]

    [SerializeField]
    [Tooltip("Initial stats of the tower. Use a TowerConfiguration to set them.")]
    private TowerConfiguration _towerSettings;
    public TowerConfiguration TowerSettings => _towerSettings;

    [SerializeField]
    [Tooltip("Initial price of the tower.")]
    private int _price = 100;
    public int Price => _price;

    [SerializeField]
    [Tooltip("These tower nodes can be built on top of this one.")]
    private TowerNode[] _buildableOnTop;
    public TowerNode[] BuildableOnTop => _buildableOnTop;
    public bool AllowsBuildingOnTop { get { return _buildableOnTop != null && _buildableOnTop.Length > 0; } }

    [SerializeField]
    [Tooltip("This tower node can be upgraded into these ones.")]
    private TowerNode[] _upgradesInto;
    public TowerNode[] UpgradesInto => _upgradesInto;
    public bool HasUpgrades { get { return _upgradesInto != null && _upgradesInto.Length > 0; } }

    [SerializeField]
    private bool _replaceRootNode = false;
    public bool ReplaceRootNode => _replaceRootNode;

    [SerializeField]
    [Tooltip("Defines on which height the next tower node will be placed.")]
    private float _height = 1.0f;
    public float Height => _height;

    [SerializeField]
    [Tooltip("Defines the offset on the Y-axis for the tower node's position (upon another tower node).")]
    private float _yOffset;
    public float YOffset => _yOffset;

    public string Name { get { return gameObject.name; } }
}