using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Feofun.Config;
using Feofun.Extension;
using Feofun.Localization;
using Survivors.Modifiers;
using Survivors.Modifiers.Config;
using Survivors.UI.Dialog.UpgradeDialog.Star;
using Survivors.Upgrade;
using Survivors.Upgrade.Config;

namespace Survivors.UI.Dialog.UpgradeDialog.Model
{
    public class UpgradeDialogModel
    {
        private const string ADD_UNIT_LOCALIZATION_ID = "weapon +1";

        private readonly List<UpgradeItemModel> _upgrades;
        private readonly UpgradesConfig _upgradesConfig;
        private readonly SquadUpgradeState _upgradeState;
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        public IReadOnlyCollection<UpgradeItemModel> Upgrades => _upgrades;
        public UpgradeDialogInitModel InitModel { get; }
        public string Level { get; }

        public UpgradeDialogModel(UpgradeDialogInitModel initModel,
                                  UpgradesConfig upgradesConfig,
                                  SquadUpgradeState upgradeState,
                                  StringKeyedConfigCollection<ParameterUpgradeConfig> modifierConfigs,
                                  Action<string> onUpgrade)
        {
            InitModel = initModel;
            Level = initModel.Level.ToString();
            _upgradesConfig = upgradesConfig;
            _upgradeState = upgradeState;
            _modifierConfigs = modifierConfigs;
            _upgrades = InitModel.UpgradeBranchIds.Select(id => BuildUpgradeItemModel(id, onUpgrade)).ToList();
        }

        private UpgradeItemModel BuildUpgradeItemModel(string upgradeBranchId, Action<string> onUpgrade)
        {
            var nextLevel = _upgradeState.GetLevel(upgradeBranchId) + 1;
            var nextUpgradeLevelConfig = _upgradesConfig.GetUpgradeConfig(upgradeBranchId, nextLevel);
            var description = nextUpgradeLevelConfig.Type == UpgradeType.Unit
                                      ? LocalizableText.Create(ADD_UNIT_LOCALIZATION_ID)
                                      : CreateModifierDescription(nextUpgradeLevelConfig);
            return new UpgradeItemModel() {
                    Id = upgradeBranchId,
                    Name = upgradeBranchId,
                    Description = description,
                    Stars = CreateStars(upgradeBranchId),
                    OnClick = () => onUpgrade?.Invoke(upgradeBranchId),
            };
        }

        private IEnumerable<StarViewModel> CreateStars(string upgradeBranchId)
        {
            var countActive = _upgradeState.GetLevel(upgradeBranchId);
            var countInactive = Math.Max(0, _upgradesConfig.GetMaxLevel(upgradeBranchId) - countActive);
            return StarViewModel.CreateCollection(countActive, StarState.Active)
                                .Concat(StarViewModel.CreateCollection(countInactive, StarState.Inactive));
        }

        private LocalizableText CreateModifierDescription(UpgradeLevelConfig nextUpgradeLevelConfig)
        {
            var modifier = _modifierConfigs.Get(nextUpgradeLevelConfig.ModifierId).ModifierConfig;
            var modifierType = EnumExt.ValueOf<ModifierType>(modifier.Modifier);
            return LocalizableText.Create(modifier.ParameterName, GetDescriptionValue(modifierType, modifier.Value.ToString(CultureInfo.InvariantCulture)));
        }

        private string GetDescriptionValue(ModifierType modifierType, string value)
        {
            return modifierType switch {
                    ModifierType.AddPercent => $"+{value}%",
                    ModifierType.RemovePercent => $"-{value}%",
                    ModifierType.AddValue => AddSignPrefix(value),
                    _ => throw new ArgumentOutOfRangeException(nameof(modifierType), modifierType, null)
            };
        }
        private string AddSignPrefix(string value) => value[0] == '-' ? value : $"+{value}";
    }
}