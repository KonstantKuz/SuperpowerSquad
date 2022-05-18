using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Units.Player.Config
{
    [DataContract]
    public class PlayerUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")]
        private string _id;
        [DataMember(Name = "Health")]
        private int _health;
        [DataMember]
        private AttackConfig _attackConfig;

        public string Id => _id;
        public int Health => _health;
        public AttackConfig AttackConfig => _attackConfig;
    }
}