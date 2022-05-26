using System.Collections.Generic;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Dialog.Model;
using UnityEngine;

namespace Survivors.UI.Dialog.View
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField]
        private UpgradeItemView _upgradeItemPrefab;
        [SerializeField]
        private Transform _root;

        public void Init(IReadOnlyCollection<UpgradeItemModel> upgrades)
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