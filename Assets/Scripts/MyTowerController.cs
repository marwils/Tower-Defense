using MarwilsTD;

using Unity.VisualScripting;

using UnityEngine;

public class MyTowerController : TowerController
{
    [Header("Tower")]
    [SerializeField]
    private MyTowerConfiguration _towerConfiguration;
    public MyTowerConfiguration TowerConfiguration => _towerConfiguration;
}
