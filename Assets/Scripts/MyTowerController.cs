using MarwilsTD;

using Unity.VisualScripting;

using UnityEngine;

public class MyTowerController : TowerController
{
    [Header("Tower")]
    [SerializeField]
    private TowerElement _baseElement;
    public TowerElement BaseElement => _baseElement;
}
