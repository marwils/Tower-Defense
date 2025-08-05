using System;
using System.Collections.Generic;
using System.Linq;
using MarwilsTD;
using UnityEngine;

public class MyTowerController : TowerController
{
    [Header("My Tower Controller")]
    [SerializeField]
    [Tooltip("Main tower node. This is the root node of the tower.")]
    private MyTowerNode _mainNode;

    protected override void Awake()
    {
        base.Awake();

        if (_mainNode == null)
        {
            Debug.LogWarning($"Root node is not set in tower <{gameObject.name}>.");
            Destroy(gameObject);
            return;
        }

        SetUpgradesAndExtensionsActive(false);
    }

    public void ExtendTower()
    {
        throw new NotImplementedException();
    }

    public void UpgradeTower(MyTowerNode upgradeNode)
    {
        if (upgradeNode == null)
        {
            Debug.LogWarning($"Upgrade node is null in tower <{gameObject.name}>.");
            return;
        }

        _mainNode.CurrentUpgrade = upgradeNode;
    }

    private void SetUpgradesAndExtensionsActive(bool isActive = true)
    {
        if (_mainNode == null)
            return;

        IEnumerable<GameObject> upgradesAndExtensionGameObjects = _mainNode
            .AvailableUpgrades.Concat(_mainNode.AvailableExtensions)
            .Select(u => u.gameObject);

        GameObjectHelper.SetActive(upgradesAndExtensionGameObjects, isActive);
    }
}
