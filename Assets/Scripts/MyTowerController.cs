using System;
using System.Collections;
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

        DeactivateUpgradesAndExtensions(_mainNode);
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

    private void DeactivateUpgradesAndExtensions(TowerNode node)
    {
        if (node == null)
            return;

        IEnumerable<GameObject> upgradesAndExtensionGameObjects = node
            .AvailableUpgrades.Concat(node.AvailableExtensions)
            .Select(u => u.gameObject);

        GameObjectHelper.SetActive(upgradesAndExtensionGameObjects, false);
    }
}
