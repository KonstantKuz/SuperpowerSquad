using System.Runtime.Serialization;

namespace Survivors.Location.EnemySpawn
{
    public class EnemyWaveConfig
    {
        [DataMember]
        public int Second;
        [DataMember]
        public int Count;
    }
}