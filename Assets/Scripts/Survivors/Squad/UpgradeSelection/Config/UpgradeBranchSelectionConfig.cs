using System;
using System.IO;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Squad.UpgradeSelection.Config
{
    public class UpgradeBranchSelectionConfig : ILoadableConfig
    {
        private UpgradeBranchSelectionConfig _config;

        [DataMember(Name = "MaxUnitUpgrade")]
        private int _maxUnitUpgrade;
        [DataMember(Name = "MaxAbilityUpgrade")]
        private int _maxAbilityUpgrade; 

        public int MaxUnitUpgrade => _config._maxUnitUpgrade;

        public int MaxAbilityUpgrade => _config._maxAbilityUpgrade;
        

        public void Load(Stream stream)
        {
            _config = new CsvSerializer().ReadSingleObject<UpgradeBranchSelectionConfig>(stream);
        }
        
        public int GetMaxUpgradeCount(UpgradeBranchType type)
        {
            return type switch {
                    UpgradeBranchType.Unit => MaxUnitUpgrade,
                    UpgradeBranchType.Ability => MaxAbilityUpgrade,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}