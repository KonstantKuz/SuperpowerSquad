using System.Runtime.Serialization;

namespace Survivors.Enemy.Spawn.Config
{
    public class EnemyWaveConfig
    {
        [DataMember]
        public int SpawnTime;
        [DataMember]
        public int Count;    
        [DataMember]
        public string EnemyId;       
        [DataMember]
        public int EnemyLevel;
        [DataMember] 
        public WavePlacingType PlacingType;

        public static EnemyWaveConfig Create(string id, int count, int level)
        {
            return new EnemyWaveConfig
            {
                EnemyId = id,
                Count = count,
                EnemyLevel = level,
            };
        }
    }
}