using System;
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
        private Dictionary<string, IReadOnlyList<UpgradeConfig>> _upgrades;

        public void Load(Stream stream)
        {
            _upgrades = new CsvSerializer().ReadNestedTable<UpgradeConfig>(stream);
        }

        public IReadOnlyList<UpgradeConfig> GetLevelConfigs(string upgradeId)
        {
            if (!_upgrades.ContainsKey(upgradeId))
            {
                throw new Exception($"No upgrades for id {upgradeId} in upgrades config");
            }
            return _upgrades[upgradeId];
        }

        public UpgradeConfig GetUpgradeConfig(string upgradeId, int level)
        {
            var levels = GetLevelConfigs(upgradeId);
            if (level <= 0 || level >= levels.Count)
            {
                throw new Exception($"Wrong upgrade level {level} for id {upgradeId}");
            }
            return levels[level - 1];
        }

        public int GetMaxLevel(string upgradeId)
        {
            return GetLevelConfigs(upgradeId).Count - 1;
        }

        public IEnumerable<string> Keys() => _upgrades.Keys;

        public bool IsUnitUpgrade(string upgradeId)
        {
            return GetLevelConfigs(upgradeId).Any(it => it.Type == UpgradeType.Unit);
        }

        public string GetUnitName(string upgradeId)
        {
            return GetLevelConfigs(upgradeId)[0].ModifierId;
        }
    }
}