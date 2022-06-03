using System;
using System.Collections;
using Feofun.Config;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.EnemySpawn
{
    public class HpsSpawner : MonoBehaviour, IWorldScope
    {
        [SerializeField] private EnemyWavesSpawner _enemyWavesSpawner;
        private Coroutine _spawnCoroutine;
        
        [Inject] private UnitFactory _unitFactory;
        [Inject] private World _world;
        [Inject] private HpsSpawnerConfigLoader _config;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        private HpsSpawnerConfig Config => _config.Config;

        public void OnWorldSetup()
        {
            
        }

        public void OnWorldCleanUp()
        {
            
        }

        public void StartSpawn()
        {
            Dispose();
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }
        
        private void Dispose()
        {
            if (_spawnCoroutine == null) return;
            
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator SpawnCoroutine()
        {
            var time = 0.0f;
            while (true)
            {
                var timeToNextWave = Random.Range(Config.MinInterval, Config.MaxInterval);
                yield return new WaitForSeconds(timeToNextWave);
                time += timeToNextWave;
                var health = timeToNextWave * (Config.StartingHPS + Config.HPSSpeed * time);
                var unitCount = Random.Range(Config.MinWaveSize, Config.MaxWaveSize + 1);
                var averageHealth = health / unitCount;
                var enemyUnitConfig = _enemyUnitConfigs.Get(UnitFactory.SIMPLE_ENEMY_ID);
                var level = (averageHealth - enemyUnitConfig.Health) / enemyUnitConfig.HealthStep;
                if (level < 1.0f)
                {
                    _enemyWavesSpawner.SpawnNextWave(Mathf.RoundToInt(health / enemyUnitConfig.Health), 1);
                }
                else
                {
                    var partition = level % 1;
                    var count = (int)(partition * unitCount);
                    var intLevel = (int)Math.Floor(level);
                    _enemyWavesSpawner.SpawnNextWave(count, intLevel);
                    _enemyWavesSpawner.SpawnNextWave(Mathf.RoundToInt(unitCount - count), intLevel + 1);
                }
            }
        }
    }
}