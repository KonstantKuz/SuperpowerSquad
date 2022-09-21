using System.Collections.Generic;
using Feofun.Extension;
using Feofun.Util.SerializableDictionary;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.Model;
using Survivors.Upgrade.UpgradeSelection;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.View
{
    public class UpgradeKitView : MonoBehaviour
    {
        [SerializeField]
        private UpgradeKitItemView _itemPrefab;

        [SerializeField]
        public SerializableDictionary<UpgradeBranchType, Transform> _roots;
        [Inject]
        private DiContainer _container;
        
        private CompositeDisposable _disposable;

        public void Init(IReadOnlyDictionary<UpgradeBranchType, ReactiveProperty<UpgradeKitCollection>> upgradeKit)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            upgradeKit.Values.ForEach(upgradeCollection => {
                upgradeCollection.Subscribe(CreateUpgradeKitCollection).AddTo(_disposable);
            });
        }

        private void CreateUpgradeKitCollection(UpgradeKitCollection upgradeCollection)
        {
            var root = _roots[upgradeCollection.BranchType];
            root.DestroyAllChildren();
            upgradeCollection.Items.ForEach(kitItemModel => {
                var item = _container.InstantiatePrefabForComponent<UpgradeKitItemView>(_itemPrefab, root);
                item.Init(kitItemModel);
            });
        }
        
        private void Dispose()
        {
            _roots.Values.ForEach(it => it.DestroyAllChildren());
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}