using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Survivors.Squad.Upgrade.Config
{
    public class UpgradeLevelConfig
    {
        [DataMember]
        public UpgradeType Type;
        [DataMember]
        public string ModifierId;
       
        [CanBeNull]
        [DataMember]
        public string TargetId;
        public bool IsTargetAllUnits => string.IsNullOrEmpty(TargetId);

        public bool IsValidTarget(string unitId) => IsTargetAllUnits || unitId.Equals(TargetId);

    }
}