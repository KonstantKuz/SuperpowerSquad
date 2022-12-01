using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Enemy.Spawn.Config
{
    public class EnemyWavesConfig : ILoadableConfig
    {
        public Dictionary<string, IReadOnlyList<EnemyWaveConfig>> EnemySpawns { get; private set; }
        
        public void Load(Stream stream)
        {
            EnemySpawns = new CsvSerializer().ReadNestedTable<EnemyWaveConfig>(stream)
                .ToDictionary(it => it.Key, it => it.Value);
        }

        public IReadOnlyCollection<EnemyWaveConfig> GetWave(string id)
        {
            if (!EnemySpawns.ContainsKey(id))
            {
                throw new ArgumentException($"There is no wave with id := {id}");
            }
            
            return EnemySpawns[id];
        }
    }
}