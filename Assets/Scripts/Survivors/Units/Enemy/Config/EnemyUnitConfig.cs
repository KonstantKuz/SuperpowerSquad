using System.Runtime.Serialization;
using Feofun.Config;
using Survivors.Loot.Config;
using UnityEngine;

namespace Survivors.Units.Enemy.Config
{
    public class EnemyUnitConfig : ICollectionItem<string>
    {
        public const int MIN_LEVEL = 1;        
        
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
        [DataMember] 
        public int HealthStep;
        [DataMember]
        public float ScaleStep;

        public float GetScaleForLevel(int level) => Mathf.Pow(ScaleStep, level - MIN_LEVEL);

        public int GetHealthForLevel(int level) => Health + (level - MIN_LEVEL) * HealthStep;
    }
}
