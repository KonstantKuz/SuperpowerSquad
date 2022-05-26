﻿using System.Collections.Generic;

namespace Survivors.Squad.Upgrade
{
    public class SquadUpgradeState
    {
        private readonly Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();

        public int GetLevel(string upgradeId) => _upgradeLevels.ContainsKey(upgradeId) ? _upgradeLevels[upgradeId] : 0;

        public void IncreaseLevel(string upgradeId)
        {
            if (!_upgradeLevels.ContainsKey(upgradeId))
            {
                _upgradeLevels[upgradeId] = 0;
            }

            _upgradeLevels[upgradeId]++;
        }

        public IReadOnlyDictionary<string, int> Upgrades => _upgradeLevels;
    }
}