using System;
using System.Collections;
using Feofun.Config;
using Survivors.EnemySpawn.Config;
using Survivors.Session;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.EnemySpawn
{
    public class HpsSpawner : MonoBehaviour, IWorldScope
    {
        private const string ENEMY_ID = "SimpleEnemy";
        
        [SerializeField] private EnemyWavesSpawner _enemyWavesSpawner;
        private Coroutine _spawnCoroutine;
        
        [Inject] private HpsSpawnerConfigLoader _config;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        private HpsSpawnerConfig Config => _config.Config;

        public void OnWorldSetup()
        {
            
        }

        public void OnWorldCleanUp()
        {
            Dispose();
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
                SpawnEnemies(health);
            }
        }

        private void SpawnEnemies(float health)
        {
            Log($"Spawning wave of health {health}");
            var unitCount = Random.Range(Config.MinWaveSize, Config.MaxWaveSize + 1);
            var averageHealth = health / unitCount;
            var enemyUnitConfig = _enemyUnitConfigs.Get(ENEMY_ID);
            var averageLevel = EnemyUnitModel.MIN_LEVEL + (averageHealth - enemyUnitConfig.Health) / enemyUnitConfig.HealthStep;
            var place = _enemyWavesSpawner.GetRandomPlaceForWave(unitCount); //TODO: user correct unit radius    
            
            if (averageLevel < EnemyUnitModel.MIN_LEVEL)
            {
                SpawnWave(Mathf.RoundToInt(health / enemyUnitConfig.Health), EnemyUnitModel.MIN_LEVEL, place);
            }
            else
            {
                var partition = averageLevel % 1;
                var highLevelCount = (int)(partition * unitCount);
                var lowerLevelCount = Mathf.RoundToInt(unitCount - highLevelCount);
                var lowerLevel = (int)Math.Floor(averageLevel);
                var highLevel = lowerLevel + 1;
                SpawnWave(highLevelCount, highLevel, place);
                SpawnWave(lowerLevelCount, lowerLevel, place);
            }
        }

        private void SpawnWave(int count, int level, Vector3 place)
        {
            Log($"Spawning wave of {count} units of level {level}");
            _enemyWavesSpawner.SpawnWaveInPlace(new EnemyWaveConfig()
            {
                Count = count,
                EnemyId = ENEMY_ID,
                EnemyLevel = level
            }, place);
        }

        private static void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);            
#endif            
        }
    }
}