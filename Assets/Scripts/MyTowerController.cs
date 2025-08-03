using System;

using MarwilsTD;

using UnityEngine;

public class MyTowerController : TowerController
{
    [Header("My Tower Controller")]

    [SerializeField]
    [Tooltip("Root tower node. If not set, the first child will be used.")]
    private TowerNode _rootNode;

    protected override void Awake()
    {
        base.Awake();

        if (_rootNode == null)
        {
            Debug.LogWarning($"Root node is not set in tower <{gameObject.name}>.");
            Destroy(gameObject);
            return;
        }

        SetAllButRootNodeActive(false);
    }

    private void SetAllButRootNodeActive(bool value)
    {
        foreach (Transform child in transform)
        {
            if (child != _rootNode.transform)
            {
                TransformHelper.SetChildrenActive(child, value);
            }
        }
    }

    public void ExtendTower()
    {
        throw new NotImplementedException();
    }

    public void UpgradeTower()
    {
        throw new NotImplementedException();
    }
}
