using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Survivors.Modifiers.Config;
using Survivors.Squad.Upgrade;
using Survivors.Squad.Upgrade.Config;

namespace Survivors.UI.Dialog.Model
{
    public class UpgradeDialogModel
    {
        private readonly List<UpgradeItemModel> _upgrades;
        private readonly UpgradesConfig _upgradesConfig;
        private readonly SquadUpgradeState _upgradeState;    
        private readonly UpgradeDialogInitModel _initModel;
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;

        public IReadOnlyCollection<UpgradeItemModel> Upgrades => _upgrades;
        public UpgradeDialogInitModel InitModel => _initModel;
        public UpgradeDialogModel(UpgradeDialogInitModel initModel,
                                  UpgradesConfig upgradesConfig,
                                  SquadUpgradeState upgradeState,
                                  StringKeyedConfigCollection<ParameterUpgradeConfig> modifierConfigs,
                                  Action<string> onUpgrade)
        {
            _initModel = initModel;
            _upgradesConfig = upgradesConfig;
            _upgradeState = upgradeState;
            _modifierConfigs = modifierConfigs;
            _upgrades = _initModel.UpgradeBranchIds.Select(id => BuildUpgradeItemModel(id, onUpgrade)).ToList();
        }

        private UpgradeItemModel BuildUpgradeItemModel(string upgradeBranchId, Action<string> onUpgrade)
        {
            var nextLevel = _upgradeState.GetLevel(upgradeBranchId) + 1;
            var nextUpgradeLevelConfig = _upgradesConfig.GetUpgradeConfig(upgradeBranchId, nextLevel);
            return new UpgradeItemModel() {
                    Id = upgradeBranchId,
                    UpgradeName = upgradeBranchId,
                    UpgradeTypeName = _upgradesConfig.GetUpgradeBranch(upgradeBranchId).BranchType.ToString(),
                    NextLevel = nextLevel.ToString(),
                    Modifier = nextUpgradeLevelConfig.Type == UpgradeType.Unit
                                       ? $"Add unit: {upgradeBranchId}"
                                       : _modifierConfigs.Get(nextUpgradeLevelConfig.ModifierId).ToString(),
                    OnClick = () => onUpgrade?.Invoke(upgradeBranchId),
            };
        }
    }
}