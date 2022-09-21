using System.Runtime.Serialization;
using Feofun.Config;

namespace Survivors.Enemy.Spawn.Config
{
    public class SpawnableEnemyConfig : ICollectionItem<string>
    {
        public string Id => EnemyId;
        
        [DataMember] 
        public string EnemyId;
        [DataMember]
        public float Delay;
        [DataMember] 
        public float Chance;
        [DataMember]
        public int MinWaveSize;
        [DataMember]
        public int MaxWaveSize;
    }
}