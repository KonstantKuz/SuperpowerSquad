using System.Runtime.Serialization;

namespace Survivors.Squad.Upgrade.Config
{
    public class UpgradeLevelConfig
    {
        [DataMember]
        public UpgradeType Type;
        [DataMember]
        public string ModifierId;
    }
}