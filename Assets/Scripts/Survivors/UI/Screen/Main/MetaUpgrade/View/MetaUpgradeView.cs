using System.Collections.Generic;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Screen.Main.MetaUpgrade.Model;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.Main.MetaUpgrade.View
{
    public class MetaUpgradeView : MonoBehaviour
    {
        [SerializeField]
        private MetaUpgradeItemView _upgradeItemPrefab;
        [SerializeField]
        private Transform _root;

        [Inject]
        private DiContainer _container;

        public void Init(MetaUpgradeModel model)
        {
            RemoveAllCreatedObjects();
            CreateUpgradeItems(model.Upgrades);
        }

        private void CreateUpgradeItems(IReadOnlyCollection<MetaUpgradeItemModel> upgrades)
        {
            upgrades.ForEach(it => {
                var itemView = _container.InstantiatePrefabForComponent<MetaUpgradeItemView>(_upgradeItemPrefab, _root);
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