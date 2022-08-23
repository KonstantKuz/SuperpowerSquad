using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using Feofun.Localization;
using Survivors.Upgrade.Config;
using Survivors.Upgrade.UpgradeSelection;

namespace Survivors.UI.Dialog.StartUnitDialog.Model
{
    public class StartUnitDialogModel
    {
        private const int UNIT_COUNT = 3;
        private const string START_UNIT_LOCALIZATION_POSTFIX = "Weapon";

        private readonly List<StartUnitItemModel> _units;
        private readonly UpgradesConfig _upgradesConfig;
        public IReadOnlyCollection<StartUnitItemModel> Units => _units;

        public StartUnitDialogModel(UpgradesConfig upgradesConfig,
                                    Action<StartUnitSelection> onUpgrade)
        {
            _upgradesConfig = upgradesConfig;
            _units = _upgradesConfig.GetUpgradeBranchIds(UpgradeBranchType.Unit)
                                    .ToList()
                                    .SelectRandomElements(UNIT_COUNT)
                                    .Select(upgradeBranchId => BuildStartUnitModel(GetUnitIdByUpgradeId(upgradeBranchId), upgradeBranchId, onUpgrade)).ToList();
        }
        private string GetUnitIdByUpgradeId(string upgradeBranchId) => 
                _upgradesConfig.GetUpgradeBranch(upgradeBranchId).Levels.First(it => it.Type == UpgradeType.Unit).TargetId;
        private StartUnitItemModel BuildStartUnitModel(string unitId, string upgradeId, Action<StartUnitSelection> onApplyUnit)
        {
            return new StartUnitItemModel() {
                    Id = unitId,
                    Name = LocalizableText.Create(unitId + START_UNIT_LOCALIZATION_POSTFIX),
                    OnClick = () => onApplyUnit?.Invoke(new StartUnitSelection(unitId, upgradeId)),
            };
        }
    }
}