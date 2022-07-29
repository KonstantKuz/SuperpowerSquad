using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Survivors.Enemy.Spawn.Config
{
    public class WavesByLevelConfig : ILoadableConfig
    {
        private IReadOnlyDictionary<string, LevelWavesConfig> _levelConfigs;
        public IReadOnlyList<LevelWavesConfig> LevelConfigs;
        public int LevelsCount => LevelConfigs.Count;
        
        public void Load(Stream stream)
        {
            _levelConfigs = new CsvSerializer().ReadNestedTable<EnemyWaveConfig>(stream)
                .ToDictionary(it => it.Key, 
                    it => new LevelWavesConfig(int.Parse(it.Key), it.Value));
            LevelConfigs = _levelConfigs.Values.ToList();
        }
    }
}