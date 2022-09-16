using Survivors.App.Config;
using Zenject;

namespace Survivors.Enemy.Spawn
{
    public class CompositeEnemySpawner : IEnemySpawner
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyHpsSpawner _enemyHpsSpawner;
        [Inject] private ConstantsConfig _constantsConfig;
        
        public void StartSpawn()
        {
            _enemyWavesSpawner.StartSpawn();
            if (_constantsConfig.EnemyHpsSpawnerEnabled) {
                _enemyHpsSpawner.StartSpawn();
            }
        }
    }
}