using System.Runtime.Serialization;
using Feofun.Config;
using Survivors.Loot.Config;

namespace Survivors.Units.Enemy.Config
{
    public class EnemyUnitConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;

        public string Id => _id; 
        [DataMember] 
        public int Health;
        [DataMember] 
        public float MoveSpeed;
        [DataMember] 
        public EnemyAttackConfig EnemyAttackConfig;
        [DataMember] 
        public DroppingLootConfig DroppingLootConfig;
    }
}
