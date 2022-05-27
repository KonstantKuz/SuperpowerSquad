using System;
using System.Collections.Generic;
using System.Linq;
using Survivors.Squad.UpgradeSelection;

namespace Survivors.Squad.Upgrade.Config
{
    public class UpgradeBranchConfig
    {
        public readonly string Id;
        public readonly UpgradeBranchType BranchType;
        
        private readonly IReadOnlyList<UpgradeLevelConfig> _levels;

        public UpgradeBranchConfig(string id, IReadOnlyList<UpgradeLevelConfig> levels)
        {
            Id = id;
            _levels = levels;
            BranchType = IsUnitBranch ? UpgradeBranchType.Unit : UpgradeBranchType.Ability;
        }

        public UpgradeLevelConfig GetLevel(int level)
        {
            if (level <= 0 || level > MaxLevel)
            {
                throw new Exception($"Wrong upgrade level {level} for id {Id}");
            }

            return _levels[level - 1];
        }
        private bool IsUnitBranch => _levels.Any(it => it.Type == UpgradeType.Unit);
        public int MaxLevel => _levels.Count;
        public string BranchUnitName => BranchType == UpgradeBranchType.Unit ? Id : null;
    }
}