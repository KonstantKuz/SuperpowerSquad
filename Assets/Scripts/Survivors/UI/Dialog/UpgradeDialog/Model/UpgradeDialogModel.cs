using System;
using System.Collections.Generic;
using System.Linq;
using Survivors.Squad.Upgrade;
using Survivors.Squad.Upgrade.Config;

namespace Survivors.UI.Dialog.UpgradeDialog.Model
{
    public class UpgradeDialogModel
    {
        private readonly List<UpgradeItemModel> _upgrades;
        private readonly UpgradesConfig _upgradesConfig;
        private readonly SquadUpgradeState _upgradeState;
        public IReadOnlyCollection<UpgradeItemModel> Upgrades => _upgrades;
        public UpgradeDialogInitModel InitModel { get; }  
        public string Level { get; }
        public UpgradeDialogModel(UpgradeDialogInitModel initModel,
                                  UpgradesConfig upgradesConfig,
                                  SquadUpgradeState upgradeState,
                                  Action<string> onUpgrade)
        {
            InitModel = initModel;
            Level = initModel.Level.ToString();
            _upgradesConfig = upgradesConfig;
            _upgradeState = upgradeState;
            _upgrades = InitModel.UpgradeBranchIds.Select(id => BuildUpgradeItemModel(id, onUpgrade)).ToList();
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
                                       : nextUpgradeLevelConfig.ModifierId,
                    OnClick = () => onUpgrade?.Invoke(upgradeBranchId),
            };
        }
    }
}