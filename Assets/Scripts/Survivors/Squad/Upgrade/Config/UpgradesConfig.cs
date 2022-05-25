﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;
using JetBrains.Annotations;

namespace Survivors.Squad.Upgrade.Config
{
    [PublicAPI]
    public class UpgradesConfig: ILoadableConfig
    {
        private IReadOnlyDictionary<string, UpgradeBranchConfig> _upgradeBranches;

        public void Load(Stream stream)
        {
            _upgradeBranches = new CsvSerializer().ReadNestedTable<UpgradeConfig>(stream)
                .ToDictionary(it => it.Key, it => new UpgradeBranchConfig(it.Key, it.Value));
        }

        public UpgradeBranchConfig GetUpgradeBranch(string upgradeBranchId)
        {
            if (!_upgradeBranches.ContainsKey(upgradeBranchId))
            {
                throw new Exception($"No upgrades for id {upgradeBranchId} in upgrades config");
            }

            return _upgradeBranches[upgradeBranchId];
        }

        public UpgradeConfig GetUpgradeConfig(string upgradeBranchId, int level)
        {
            var branch = GetUpgradeBranch(upgradeBranchId);
            return branch.GetLevel(level);
        }

        public int GetMaxLevel(string upgradeBranchId)
        {
            return GetUpgradeBranch(upgradeBranchId).MaxLevel;
        }

        public IEnumerable<string> GetUpgradeBranchIds() => _upgradeBranches.Keys;

        public string GetUnitName(string upgradeId)
        {
            return upgradeId;
        }
    }
}