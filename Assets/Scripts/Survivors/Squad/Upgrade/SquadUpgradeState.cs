using System.Collections.Generic;
using Survivors.Squad.Upgrade.Config;

namespace Survivors.Squad.Upgrade
{
    public class SquadUpgradeState
    {
        private readonly Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();

        public int GetLevel(string upgradeBranchId) => _upgradeLevels.ContainsKey(upgradeBranchId) ? _upgradeLevels[upgradeBranchId] : 0;

        public void IncreaseLevel(string upgradeBranchId)
        {
            if (!_upgradeLevels.ContainsKey(upgradeBranchId))
            {
                _upgradeLevels[upgradeBranchId] = 0;
            }

            _upgradeLevels[upgradeBranchId]++;
        }
        public bool IsMaxLevel(string upgradeBranchId, UpgradesConfig upgradesConfig)
        {
            return GetLevel(upgradeBranchId) >= upgradesConfig.GetMaxLevel(upgradeBranchId);
        }
        public IEnumerable<string> GetUpgradeBranchIds() => _upgradeLevels.Keys;
        public IReadOnlyDictionary<string, int> Upgrades => _upgradeLevels;
    }
}