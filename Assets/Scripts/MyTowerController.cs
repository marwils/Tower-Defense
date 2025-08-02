using MarwilsTD;

using UnityEngine;

public class MyTowerController : TowerController
{
    [SerializeField]
    private GameObject _base;

    [SerializeField]
    private GameObject[] _elements;
    public GameObject[] Elements => _elements;
}
