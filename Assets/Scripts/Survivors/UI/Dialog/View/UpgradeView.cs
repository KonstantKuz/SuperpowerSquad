using System.Collections.Generic;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Dialog.Model;
using TMPro;
using UnityEngine;

namespace Survivors.UI.Dialog.View
{
    public class UpgradeView : MonoBehaviour
    {
        private const string LEVEL_PREFIX = "Level:";
        [SerializeField]
        private UpgradeItemView _upgradeItemPrefab;
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private TextMeshProUGUI _level;

        public void Init(UpgradeDialogModel dialogModel)
        {
            RemoveAllCreatedObjects();
            _level.text = LEVEL_PREFIX + dialogModel.Level;
            CreateUpgradeItems(dialogModel.Upgrades);
        }
        private void CreateUpgradeItems(IReadOnlyCollection<UpgradeItemModel> upgrades)
        {
            upgrades.ForEach(it => {
                var itemView = Instantiate(_upgradeItemPrefab, _root, false);
                itemView.Init(it);
            });
        }
        
        private void OnDisable()
        {
            RemoveAllCreatedObjects();
        }

        private void RemoveAllCreatedObjects()
        {
            _root.DestroyAllChildren();
        }
    }
}