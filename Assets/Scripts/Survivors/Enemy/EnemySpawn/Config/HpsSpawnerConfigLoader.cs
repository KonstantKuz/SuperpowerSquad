using System.IO;
using Feofun.Config;
using Feofun.Config.Csv;
using JetBrains.Annotations;

namespace Survivors.Enemy.EnemySpawn.Config
{
    [PublicAPI]
    public class HpsSpawnerConfigLoader : ILoadableConfig
    {
        public HpsSpawnerConfig Config { get; private set; }
        
        public void Load(Stream stream)
        {
            Config = new CsvSerializer().ReadSingleObject<HpsSpawnerConfig>(stream);
        }
    }
}