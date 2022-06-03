using System.IO;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

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
    }

    public class HpsSpawnerConfigLoader : ILoadableConfig
    {
        public HpsSpawnerConfig Config { get; private set; }
        
        public void Load(Stream stream)
        {
            Config = new CsvSerializer().ReadSingleObject<HpsSpawnerConfig>(stream);
        }
    }
}