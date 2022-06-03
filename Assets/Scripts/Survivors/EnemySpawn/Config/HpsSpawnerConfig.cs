using System.Runtime.Serialization;

namespace Survivors.EnemySpawn.Config
{
    public class HpsSpawnerConfig
    {
        [DataMember]
        public float StartingHPS;
        [DataMember]
        public float HPSSpeed;
        [DataMember]
        public float MinWaveSize;
        [DataMember]
        public float MaxWaveSize;
        [DataMember]
        public float MinInterval;
        [DataMember]
        public float MaxInterval;
        [DataMember] 
        public string EnemyId;
    }
}