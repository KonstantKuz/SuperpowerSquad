using System.Runtime.Serialization;

namespace Survivors.EnemySpawn.Config
{
    public class EnemyWaveConfig
    {
        [DataMember]
        public int SpawnTime;
        [DataMember]
        public int Count;
    }
}