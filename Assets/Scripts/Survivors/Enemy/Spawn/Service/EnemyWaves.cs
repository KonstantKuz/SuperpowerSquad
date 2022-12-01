using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using Survivors.Enemy.Spawn.Config;
using Survivors.Units.Enemy.Config;
using Zenject;

namespace Survivors.Enemy.Spawn.Service
{
    public class EnemyWaves
    {
        [Inject]
        private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject]
        private EnemyWavesConfig _enemyWavesConfig; 
        
        public IEnumerable<EnemyWaveConfig> GetWavesConfigs(bool isBoss)
        { 
            return _enemyWavesConfig.EnemySpawns.SelectMany(it => it.Value)
                                    .OrderBy(it => it.SpawnTime)
                                    .Where(it => _enemyUnitConfigs.Get(it.EnemyId).IsBoss == isBoss);
        }

    }
}