using System.Collections.Generic;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Dialog.StartUnitDialog.Model;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.StartUnitDialog.View
{
    public class StartUnitDialogView : MonoBehaviour
    {
        [SerializeField]
        private StartUnitItemView _itemPrefab;
        [SerializeField]
        private Transform _root;

        [Inject] private DiContainer _container;
        public void Init(StartUnitDialogModel dialogModel)
        {
            RemoveAllCreatedObjects();
            CreateItems(dialogModel.Units);
        }
        private void CreateItems(IReadOnlyCollection<StartUnitItemModel> units)
        {
            units.ForEach(it => {
                var itemView = _container.InstantiatePrefabForComponent<StartUnitItemView>(_itemPrefab, _root);
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