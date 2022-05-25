using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Squad.UpgradeSelection.Config
{
    public class UpgradeBranchSelectionConfig : ILoadableConfig
    {
        private UpgradeBranchSelectionConfig _config;

        [DataMember(Name = "MaxUnitUpgradeBranchCount")]
        private int _maxUnitUpgradeBranchCount;
        [DataMember(Name = "MaxAbilityUpgradeBranchCount")]
        private int _maxAbilityUpgradeBranchCount;

        public int MaxUnitUpgradeBranchCount => _config._maxUnitUpgradeBranchCount;

        public int MaxAbilityUpgradeBranchCount => _config._maxAbilityUpgradeBranchCount;
        

        public void Load(Stream stream)
        {
            _config = new CsvSerializer().ReadSingleObject<UpgradeBranchSelectionConfig>(stream);
        }
        
        public int GetMaxUpgradeBranchCount(UpgradeBranchType type)
        {
            return type switch {
                    UpgradeBranchType.Unit => MaxUnitUpgradeBranchCount,
                    UpgradeBranchType.Ability => _maxAbilityUpgradeBranchCount,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}