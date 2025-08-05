using System;
using UnityEngine;

namespace MarwilsTD
{
    using LevelSystem;

    /// <summary>
    /// Represents a tower node in the game, which can be upgraded or extended.
    /// </summary>
    [Serializable]
    public class TowerNode : TowerController
    {
        [Header("Tower Node")]
        [SerializeField]
        [Tooltip("Initial stats of the tower. Use a TowerConfiguration to set them.")]
        private TowerConfiguration _towerSettings;
        public TowerConfiguration TowerSettings => _towerSettings;

        [SerializeField]
        [Tooltip("Initial price of the tower node.")]
        private float _price = 100;
        public float Price => _price;

        [SerializeField]
        [Tooltip("These tower nodes can be built on top of this one.")]
        private TowerNode[] _availableExtensions;
        public TowerNode[] AvailableExtensions => _availableExtensions;
        public bool IsExtendable
        {
            get { return _availableExtensions != null && _availableExtensions.Length > 0; }
        }

        [SerializeField]
        [Tooltip("This tower node can be upgraded into these ones.")]
        private TowerNode[] _availableUpgrades;
        public TowerNode[] AvailableUpgrades => _availableUpgrades;
        public bool IsUpgradable
        {
            get { return _availableUpgrades != null && _availableUpgrades.Length > 0; }
        }

        private int _currentUpgradeIndex = -1;
        public TowerNode CurrentUpgrade
        {
            get { return GetUpgradeNode(); }
            set { SetUpgradeNode(value); }
        }
        public bool HasUpgrade
        {
            get { return _currentUpgradeIndex != -1; }
        }

        private int _currentExtensionIndex = -1;
        public TowerNode CurrentExtension
        {
            get { return GetExtensionNode(); }
            set { SetExtensionNode(value); }
        }
        public bool HasExtension
        {
            get { return _currentExtensionIndex != -1; }
        }

        public string Name
        {
            get { return gameObject.name; }
        }

        protected virtual TowerNode GetUpgradeNode()
        {
            return IsUpgradeValid() ? _availableUpgrades[_currentUpgradeIndex] : null;
        }

        private bool IsUpgradeValid()
        {
            return _currentUpgradeIndex >= 0 && _currentUpgradeIndex < _availableUpgrades.Length;
        }

        protected virtual void SetUpgradeNode(TowerNode upgradeNode)
        {
            if (upgradeNode == null)
            {
                Debug.LogWarning($"Upgrade node is null for tower <{gameObject.name}>.");
                return;
            }

            _currentUpgradeIndex = Array.IndexOf(_availableUpgrades, upgradeNode);
        }

        protected virtual TowerNode GetExtensionNode()
        {
            return IsExtensionValid() ? _availableExtensions[_currentExtensionIndex] : null;
        }

        private bool IsExtensionValid()
        {
            return _currentExtensionIndex >= 0 && _currentExtensionIndex < _availableExtensions.Length;
        }

        protected virtual void SetExtensionNode(TowerNode extensionNode)
        {
            if (extensionNode == null)
            {
                Debug.LogWarning($"Extension node is null for tower <{gameObject.name}>.");
                return;
            }

            _currentExtensionIndex = Array.IndexOf(_availableExtensions, extensionNode);
        }
    }
}
