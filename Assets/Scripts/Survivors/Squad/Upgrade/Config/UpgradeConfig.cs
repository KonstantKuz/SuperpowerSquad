using System.Runtime.Serialization;

namespace Survivors.Squad.Upgrade.Config
{
    public class UpgradeConfig
    {
        [DataMember]
        public UpgradeType Type;
        [DataMember]
        public string ImprovementId;
    }
}