using SuperMaxim.Messaging;
using Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.Model;
using Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.View;
using Survivors.Upgrade;
using Survivors.Upgrade.Config;
using Survivors.Upgrade.Messages;
using Survivors.Upgrade.UpgradeSelection.Config;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.UpgradeDialog.UpgradeKit
{
    public class UpgradeKitPresenter : MonoBehaviour
    {
        [SerializeField]
        private UpgradeKitView _view;
        [Inject]
        private UpgradeService _upgradeService;
        [Inject]
        private UpgradesConfig _upgradesConfig;
        [Inject]
        private UpgradeBranchSelectionConfig _upgradeSelectionConfig;     
        [Inject]
        private IMessenger _messenger;

        private UpgradeKitModel _model;

        private void OnEnable()
        {
            _model = new UpgradeKitModel(_upgradeService.SquadUpgradeState, _upgradesConfig, _upgradeSelectionConfig);
            _view.Init(_model.UpgradeKit);
            _messenger.Subscribe<UpgradeAppliedMessage>(OnUpgradeApplied);
        }

        private void OnUpgradeApplied(UpgradeAppliedMessage evn)
        {
            var upgradeBranchType = _upgradesConfig.GetUpgradeBranch(evn.UpgradeBranchId).BranchType; 
            _model.UpdateItemCollection(upgradeBranchType);
        }

        private void OnDisable()
        {
            _messenger.Unsubscribe<UpgradeAppliedMessage>(OnUpgradeApplied);
            _model = null;
        }
    }
}