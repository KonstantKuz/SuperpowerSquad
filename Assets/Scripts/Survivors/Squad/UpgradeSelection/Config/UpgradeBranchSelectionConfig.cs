using System;
using System.Runtime.Serialization;

namespace Survivors.Squad.UpgradeSelection.Config
{
    public class UpgradeBranchSelectionConfig
    {
        [DataMember(Name = "MaxUnitUpgrade")]
        public int MaxUnitUpgrade;
        [DataMember(Name = "MaxAbilityUpgrade")]
        public int MaxAbilityUpgrade;

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