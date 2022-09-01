using System.Runtime.Serialization;

namespace Survivors.Enemy.Spawn.Config
{
    public class HpsSpawnerConfig
    {
        [DataMember]
        public float StartingHPS;
        [DataMember]
        public float HPSSpeed;
        [DataMember]
        public float MinInterval;
        [DataMember]
        public float MaxInterval;
    }
}