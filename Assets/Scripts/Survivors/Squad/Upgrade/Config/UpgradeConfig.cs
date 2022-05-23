using System.Runtime.Serialization;

namespace Survivors.Squad.Upgrade
{
    public class UpgradeConfig
    {
        [DataMember]
        public UpgradeType Type;
        [DataMember]
        public string UpgradeId;
    }
}