using System;
using System.Collections.Generic;
using System.Linq;

namespace Survivors.Squad.Upgrade.Config
{
    public class UpgradeBranchConfig
    {
        public readonly string Id;
        private readonly IReadOnlyList<UpgradeConfig> _levels;

        public UpgradeBranchConfig(string id, IReadOnlyList<UpgradeConfig> levels)
        {
            Id = id;
            _levels = levels;
        }

        public UpgradeConfig GetLevel(int level)
        {
            if (level <= 0 || level >= MaxLevel)
            {
                throw new Exception($"Wrong upgrade level {level} for id {Id}");
            }

            return _levels[level - 1];
        }

        public int MaxLevel => _levels.Count;
        
        public bool IsUnitBranch => _levels.Any(it => it.Type == UpgradeType.Unit);

        public string BranchUnitName => IsUnitBranch ? Id : null;
    }
}