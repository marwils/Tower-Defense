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

        SetUpgradesAndExtensionsActiveRecursively(_mainNode, false);
    }

    private void Start()
    {
        // Register to button events
        //UIButtonRegister.OnUpgradeRequested += UpgradeTower;
        //UIButtonRegister.OnExtendRequested += ExtendTower;
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

    private void SetUpgradesAndExtensionsActiveRecursively(TowerNode node, bool isActive = true)
    {
        if (node == null)
            return;

        TowerNode[] upgradesAndExtensions = node.AvailableUpgrades.Concat(node.AvailableExtensions).ToArray();

        foreach (var childNode in upgradesAndExtensions)
        {
            if (childNode != null)
            {
                childNode.gameObject.SetActive(isActive);
                SetUpgradesAndExtensionsActiveRecursively(childNode, isActive);
            }
        }
    }
}
