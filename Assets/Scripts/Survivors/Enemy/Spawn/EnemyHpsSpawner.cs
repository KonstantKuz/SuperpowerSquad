using System;
using System.Collections;
using System.Linq;
using Feofun.Config;
using Logger.Extension;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Session.Messages;
using Survivors.Session.Service;
using Survivors.Units.Enemy.Config;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn
{
    public class EnemyHpsSpawner : MonoBehaviour
    {
        private const int SELECT_ENEMY_MAX_ATTEMPT_COUNT = 5;
        
        private Coroutine _spawnCoroutine;
        
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private HpsSpawnerConfig _config;   
        [Inject] private IMessenger _messenger;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private StringKeyedConfigCollection<SpawnableEnemyConfig> _spawnableEnemyConfigs;
        [Inject] private SessionService _sessionService;
        
        private void Awake()
        {
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }
        
        public void StartSpawn()
        {
            Stop();
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }
        private void OnSessionFinished(SessionEndMessage evn)
        {
            Stop();
        }
        private void Stop()
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
                var timeToNextWave = Random.Range(_config.MinInterval, _config.MaxInterval);
                yield return new WaitForSeconds(timeToNextWave);
                time += timeToNextWave;
                var health = timeToNextWave * (_config.StartingHPS + _config.HPSSpeed * time);
                SpawnWave(health);
            }
        }

        private void SpawnWave(float health)
        {
            Log($"Spawning wave of health {health}");
            var spawnConfig = GetSuitableSpawnConfig();
            var desiredUnitCount = Random.Range(spawnConfig.MinWaveSize, spawnConfig.MaxWaveSize + 1);
            var averageHealth = health / desiredUnitCount;
            var enemyUnitConfig = _enemyUnitConfigs.Get(spawnConfig.Id);
            var averageLevel = EnemyUnitConfig.MIN_LEVEL + (averageHealth - enemyUnitConfig.Health) / enemyUnitConfig.HealthStep;

            if (averageLevel < EnemyUnitConfig.MIN_LEVEL)
            {
                var level = EnemyUnitConfig.MIN_LEVEL;
                var possibleUnitCount = Mathf.RoundToInt(health / enemyUnitConfig.Health);
                var waveConfig = EnemyWaveConfig.Create(enemyUnitConfig.Id, possibleUnitCount, level);
                var place = GetWavePlace(waveConfig);
                SpawnWave(waveConfig, place);
            }
            else
            {
                var waveConfig = EnemyWaveConfig.Create(enemyUnitConfig.Id, desiredUnitCount, (int) Math.Floor(averageLevel));
                SpawnMixedWave(waveConfig);
            }
        }

        private SpawnableEnemyConfig GetSuitableSpawnConfig()
        {
            for (int i = 0; i < SELECT_ENEMY_MAX_ATTEMPT_COUNT; i++)
            {
                var randomConfig = GetRandomEnemyConfig();
                if (randomConfig != null)
                {
                    return randomConfig;
                }
            }
            
            return _spawnableEnemyConfigs.OrderBy(it => it.Chance).First();
        }

        private SpawnableEnemyConfig GetRandomEnemyConfig()
        {
            var spawnChance = Random.value;
            foreach (var spawnConfig in _spawnableEnemyConfigs.OrderBy(it => it.Chance))
            {
                if (spawnChance > spawnConfig.Chance) continue;
                if (_sessionService.PlayTime.Value < spawnConfig.Delay) continue;

                return spawnConfig;
            }

            return null;
        }

        private SpawnPlace GetWavePlace(EnemyWaveConfig waveConfig)
        {
            return _enemyWavesSpawner.GetPlaceForWave(waveConfig);
        }

        private void SpawnMixedWave(EnemyWaveConfig waveConfig)
        {
            var partition = waveConfig.EnemyLevel % 1;
            var highLevelCount = partition * waveConfig.Count;
            var lowerLevelCount = waveConfig.Count - highLevelCount;
            var lowerLevel = waveConfig.EnemyLevel;
            var highLevel = lowerLevel + 1;

            var lowerLevelConfig = EnemyWaveConfig.Create(waveConfig.EnemyId, lowerLevelCount, lowerLevel);
            var highLevelConfig = EnemyWaveConfig.Create(waveConfig.EnemyId, highLevelCount, highLevel);
            var place = GetWavePlace(waveConfig);
            SpawnWave(lowerLevelConfig, place);
            SpawnWave(highLevelConfig, place);
        }

        private void SpawnWave(EnemyWaveConfig waveConfig, SpawnPlace place)
        {
            Log($"Spawning wave of {waveConfig.Count} units of level {waveConfig.EnemyLevel}");
            _enemyWavesSpawner.SpawnWave(waveConfig, place);
        }

        private void OnDestroy()
        {
            _messenger.Unsubscribe<SessionEndMessage>(OnSessionFinished);
        }
        private void Log(string message)
        {
#if UNITY_EDITOR
            this.Logger().Trace(message);            
#endif            
        }
    }
}