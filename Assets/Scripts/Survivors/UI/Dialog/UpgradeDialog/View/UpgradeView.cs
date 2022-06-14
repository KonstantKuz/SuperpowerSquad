using System.Collections.Generic;
using Feofun.Extension;
using Feofun.UI.Components;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Dialog.UpgradeDialog.Model;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.UpgradeDialog.View
{
    public class UpgradeView : MonoBehaviour
    {
        private const string LEVEL_PREFIX = "Level";
        [SerializeField]
        private UpgradeItemView _upgradeItemPrefab;
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private TextMeshProLocalization _level;
        
        [Inject] private DiContainer _container;
        public void Init(UpgradeDialogModel dialogModel)
        {
            RemoveAllCreatedObjects();
            _level.SetTextFormatted(LEVEL_PREFIX, dialogModel.Level);
            CreateUpgradeItems(dialogModel.Upgrades);
        }
        private void CreateUpgradeItems(IReadOnlyCollection<UpgradeItemModel> upgrades)
        {
            upgrades.ForEach(it => {
                var itemView = _container.InstantiatePrefabForComponent<UpgradeItemView>(_upgradeItemPrefab, _root);
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