using System.Runtime.Serialization;
using Feofun.Config;
using Survivors.Session.Model;

namespace Survivors.Reward.Config
{
    [DataContract]
    public class MissionRewardsConfig : ICollectionItem<SessionResult>
    {
        [DataMember(Name = "Result")]
        public SessionResult Result;

        [DataMember(Name = "KilledFactor")]
        public float KilledFactor;
        public SessionResult Id => Result;
    }
}