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
        private PlayerAttackConfig _playerAttackConfig;

        public string Id => _id;
        public int Health => _health;
        public PlayerAttackConfig PlayerAttackConfig => _playerAttackConfig;
    }
}