using Feofun.Config;
using Feofun.UI.Components;
using Feofun.UI.Dialog;
using Survivors.Advertisment.Service;
using Survivors.Config;
using Survivors.Modifiers.Config;
using Survivors.UI.Dialog.UpgradeDialog.Model;
using Survivors.UI.Dialog.UpgradeDialog.View;
using Survivors.Upgrade;
using Survivors.Upgrade.Config;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog.UpgradeDialog
{
    public class UpgradeDialog : BaseDialog, IUiInitializable<UpgradeDialogInitModel>
    {
        [SerializeField]
        private UpgradeView _view;

        [Inject] private DialogManager _dialogManager;
        [Inject] private UpgradeService _upgradeService;
        [Inject] private UpgradesConfig _upgradesConfig;
        [Inject(Id = Configs.MODIFIERS)]
        private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject] private AdsManager _adsManager;

        private UpgradeDialogModel _model;

        public void Init(UpgradeDialogInitModel initModel)
        {
            _model = new UpgradeDialogModel(initModel, _upgradesConfig, _upgradeService.SquadUpgradeState, _modifierConfigs, OnUpgrade);
            _view.Init(_model);
        }

        private void OnUpgrade(string upgradeBranchId)
        {
            _model.InitModel.OnUpgrade?.Invoke(upgradeBranchId);
            _adsManager.ShowInterstitialAds(() => _dialogManager.Hide<UpgradeDialog>());
        }

        private void OnDisable()
        {
            _model = null;
        }
    }
}