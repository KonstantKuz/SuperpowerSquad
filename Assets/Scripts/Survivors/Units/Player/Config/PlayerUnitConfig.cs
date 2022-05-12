using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Units.Player.Config
{
    [DataContract]
    public class PlayerUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")]
        private string _id;
        [DataMember]
        private AttackConfig _attackConfig;

        public AttackConfig AttackConfig => _attackConfig;
        public string Id => _id;
    }
}