using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.EnemySpawn.Config
{
    public class EnemyWavesConfig : ILoadableConfig
    {
        public IReadOnlyCollection<EnemyWaveConfig> EnemySpawns { get; private set; }
        
        public void Load(Stream stream)
        {
            EnemySpawns = new CsvSerializer().ReadObjectArray<EnemyWaveConfig>(stream).ToList();
        }
    }
}