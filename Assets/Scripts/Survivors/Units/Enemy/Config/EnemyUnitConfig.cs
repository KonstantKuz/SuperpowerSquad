using System.Runtime.Serialization;
using Feofun.Config;
using Survivors.Loot.Config;

namespace Survivors.Units.Enemy.Config
{
    public class EnemyUnitConfig : ICollectionItem<string>
    {
        public const int MIN_LEVEL = 1;        
        
        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember] 
        public int Health;
        [DataMember] 
        public float MoveSpeed;
        [DataMember] 
        public EnemyAttackConfig EnemyAttackConfig;
        [DataMember] 
        public DroppingLootConfig DroppingLootConfig;      
        [DataMember] 
        public EnemyScaleConfig EnemyScaleConfig;
        [DataMember] 
        public int HealthStep;
        
        public string Id => _id; 
        
        public int GetHealthForLevel(int level) => Health + (level - MIN_LEVEL) * HealthStep;
        public float CalculateScale(int level) => EnemyScaleConfig.CalculateScale(level);


    }
}
