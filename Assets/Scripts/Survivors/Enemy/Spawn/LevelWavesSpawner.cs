using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using Survivors.Session.Messages;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Spawn
{
    public class LevelWavesSpawner : MonoBehaviour, IWorldScope
    {
        [Inject] private IMessenger _messenger;
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;

        private LevelWavesConfig _currentLevelConfig;
        private int _currentWaveIndex;

        public void StartSpawn(LevelWavesConfig levelConfig)
        {
            _currentLevelConfig = levelConfig;
            _currentWaveIndex = 0;
            
            SpawnCurrentWave();
            _messenger.Subscribe<WaveClearedMessage>(SpawnNextWave);
        }

        private void SpawnNextWave(WaveClearedMessage msg)
        {
            _currentWaveIndex++;
            SpawnCurrentWave();
        }

        private void SpawnCurrentWave()
        {
            var waveConfig = _currentLevelConfig.Waves[_currentWaveIndex];
            var enemiesLeft = waveConfig.Count;
            var subWavesCount = Random.Range(3, 5);
            var spawnCount = enemiesLeft / subWavesCount;
            while (enemiesLeft > spawnCount)
            {
                enemiesLeft -= spawnCount;
                SpawnSubWave(spawnCount, waveConfig);
            }
            SpawnSubWave(enemiesLeft, waveConfig);
        }

        private void SpawnSubWave(int subWaveCount, EnemyWaveConfig waveConfig)
        {
            var subWaveConfig = new EnemyWaveConfig
            {
                Count =  subWaveCount, 
                EnemyId = waveConfig.EnemyId,
                EnemyLevel = waveConfig.EnemyLevel
            };
            var spawnPlace = _enemyWavesSpawner.GetPlaceForWave(subWaveConfig);
            _enemyWavesSpawner.SpawnWave(subWaveConfig, spawnPlace);
        }

        public void OnWorldSetup()
        {
        }

        public void OnWorldCleanUp()
        {
            _messenger.Unsubscribe<WaveClearedMessage>(SpawnNextWave);
        }
    }
}
