using MarwilsTD;

using Unity.VisualScripting;

using UnityEngine;

public class MyTowerController : TowerController
{
    [Header("Tower")]
    [SerializeField]
    private TowerConfiguration _towerConfiguration;
    public TowerConfiguration TowerConfiguration => _towerConfiguration;
}
