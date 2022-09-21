using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using Survivors.Upgrade;
using Survivors.Upgrade.Config;
using Survivors.Upgrade.UpgradeSelection;
using Survivors.Upgrade.UpgradeSelection.Config;
using UniRx;

namespace Survivors.UI.Dialog.UpgradeDialog.UpgradeKit.Model
{
    public class UpgradeKitModel
    {
        private readonly SquadUpgradeState _upgradeState;
        private readonly UpgradesConfig _upgradesConfig;
        private readonly UpgradeBranchSelectionConfig _upgradeSelectionConfig;

        private readonly Dictionary<UpgradeBranchType, ReactiveProperty<UpgradeKitCollection>> _upgradeKit;

        public IReadOnlyDictionary<UpgradeBranchType, ReactiveProperty<UpgradeKitCollection>> UpgradeKit => _upgradeKit;

        public UpgradeKitModel(SquadUpgradeState upgradeState, UpgradesConfig upgradesConfig, UpgradeBranchSelectionConfig upgradeSelectionConfig)
        {
            _upgradeState = upgradeState;
            _upgradesConfig = upgradesConfig;
            _upgradeSelectionConfig = upgradeSelectionConfig;
            _upgradeKit = EnumExt.Values<UpgradeBranchType>()
                                 .ToDictionary(type => type, type => new ReactiveProperty<UpgradeKitCollection>(CreateUpgradeCollection(type)));
        }

        public void UpdateItemCollection(UpgradeBranchType upgradeType)
        {
            _upgradeKit[upgradeType].SetValueAndForceNotify(CreateUpgradeCollection(upgradeType));
        }

        private UpgradeKitCollection CreateUpgradeCollection(UpgradeBranchType upgradeType)
        {
            var items = GetActiveUpgradeItems(upgradeType).ToList();
            var countInactive = Math.Max(0, _upgradeSelectionConfig.GetMaxUpgradeCount(upgradeType) - items.Count());

            return new UpgradeKitCollection(upgradeType, items.Concat(GetInactiveUpgradeItems(countInactive)));
        }

        private IEnumerable<UpgradeKitItemModel> GetInactiveUpgradeItems(int count)
        {
            for (int i = 0; i < count; i++) {
                yield return new UpgradeKitItemModel(UpgradeKitViewState.Inactive);
            }
        }

        private IEnumerable<UpgradeKitItemModel> GetActiveUpgradeItems(UpgradeBranchType upgradeType)
        {
            return GetActiveUpgradeIds(upgradeType).Select(id => new UpgradeKitItemModel(UpgradeKitViewState.Active, id));
        }

        private IEnumerable<string> GetActiveUpgradeIds(UpgradeBranchType upgradeType)
        {
            return _upgradeState.GetUpgradeBranchIds()
                                .Select(_upgradesConfig.GetUpgradeBranch)
                                .Where(config => config.BranchType.Equals(upgradeType))
                                .Select(config => config.Id);
        }
    }
}