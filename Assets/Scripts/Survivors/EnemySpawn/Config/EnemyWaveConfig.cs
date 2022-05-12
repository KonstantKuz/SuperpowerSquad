using System.Runtime.Serialization;

namespace Survivors.EnemySpawn.Config
{
    public class EnemyWaveConfig
    {
        [DataMember]
        public int Second;
        [DataMember]
        public int Count;
    }
}