using System;
using System.Collections.Generic;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.UI.Screen.Main.MetaUpgrade.Model;
using UniRx;
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
        
        private CompositeDisposable _disposable;
        
        public void Init(MetaUpgradeModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            RemoveAllCreatedObjects();
            CreateUpgradeItems(model.Upgrades);
        }

        private void CreateUpgradeItems(IReadOnlyCollection<IObservable<MetaUpgradeItemModel>> upgrades)
        {
            upgrades.ForEach(it => {
                var itemView = _container.InstantiatePrefabForComponent<MetaUpgradeItemView>(_upgradeItemPrefab, _root);
                it.Subscribe(model => itemView.Init(model)).AddTo(_disposable);
            });
        }

        private void OnDisable()
        {
            Dispose();
            RemoveAllCreatedObjects();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void RemoveAllCreatedObjects()
        {
            _root.DestroyAllChildren();
        }
    }
}