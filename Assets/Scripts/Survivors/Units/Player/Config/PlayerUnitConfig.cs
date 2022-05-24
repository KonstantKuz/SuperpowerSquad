using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Units.Player.Config
{
    [DataContract]
    public class PlayerUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")]
        private string _id;

        public string Id => _id;
        [DataMember]
        public int Health;
        [DataMember]
        public PlayerAttackConfig PlayerAttackConfig;
    }
}