using Feofun.Config;
using Feofun.UI.Components;
using Feofun.UI.Dialog;
using Survivors.Modifiers.Config;
using Survivors.Squad.Upgrade;
using Survivors.Squad.Upgrade.Config;
using Survivors.UI.Dialog.Model;
using Survivors.UI.Dialog.View;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Dialog
{
    public class UpgradeDialog : BaseDialog, IUiInitializable<UpgradeDialogInitModel>
    {
        [SerializeField]
        private UpgradeView _view;

        [Inject]
        private DialogManager _dialogManager;
        [Inject]
        private UpgradeService _upgradeService;
        [Inject]
        private UpgradesConfig _upgradesConfig;
        [Inject]
        private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;

        private UpgradeDialogModel _model;

        public void Init(UpgradeDialogInitModel initModel)
        {
            _model = new UpgradeDialogModel(initModel, _upgradesConfig, _upgradeService.SquadUpgradeState, _modifierConfigs, OnUpgrade);
            _view.Init(_model.Upgrades);
        }

        private void OnUpgrade(string upgradeBranchId)
        {
            _model.InitModel.OnUpgrade?.Invoke(upgradeBranchId);
            _dialogManager.Hide<UpgradeDialog>();
        }

        private void OnDisable()
        {
            _model = null;
        }
    }
}