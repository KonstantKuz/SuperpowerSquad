using System;
using System.Collections;
using Feofun.Config;
using Logger.Assets.Scripts.Extension;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Session.Messages;
using Survivors.Units.Enemy.Config;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Enemy.Spawn
{
    public class EnemyHpsSpawner : MonoBehaviour
    {
        private Coroutine _spawnCoroutine;
        
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private HpsSpawnerConfigLoader _config;   
        [Inject] private IMessenger _messenger;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        private HpsSpawnerConfig Config => _config.Config;

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
                var timeToNextWave = Random.Range(Config.MinInterval, Config.MaxInterval);
                yield return new WaitForSeconds(timeToNextWave);
                time += timeToNextWave;
                var health = timeToNextWave * (Config.StartingHPS + Config.HPSSpeed * time);
                SpawnWave(health);
            }
        }

        private void SpawnWave(float health)
        {
            Log($"Spawning wave of health {health}");
            var desiredUnitCount = Random.Range(Config.MinWaveSize, Config.MaxWaveSize + 1);
            var averageHealth = health / desiredUnitCount;
            var enemyUnitConfig = _enemyUnitConfigs.Get(Config.EnemyId);
            var averageLevel = EnemyUnitConfig.MIN_LEVEL + (averageHealth - enemyUnitConfig.Health) / enemyUnitConfig.HealthStep;

            if (averageLevel < EnemyUnitConfig.MIN_LEVEL)
            {
                var level = EnemyUnitConfig.MIN_LEVEL;
                var possibleUnitCount = Mathf.RoundToInt(health / enemyUnitConfig.Health);
                var place = GetWavePlace(possibleUnitCount, level);
                SpawnWave(possibleUnitCount, level, place);
            }
            else
            {
                SpawnMixedWave(averageLevel, desiredUnitCount);
            }
        }

        private SpawnPlace GetWavePlace(float unitCount, int level)
        {
            return _enemyWavesSpawner.GetPlaceForWave(GetWaveConfig(Mathf.RoundToInt(unitCount), level));
        }

        private void SpawnMixedWave(float averageLevel, float unitCount)
        {
            var partition = averageLevel % 1;
            var highLevelCount = (int)(partition * unitCount);
            var lowerLevelCount = Mathf.RoundToInt(unitCount - highLevelCount);
            var lowerLevel = (int)Math.Floor(averageLevel);
            var highLevel = lowerLevel + 1;
            
            var place = GetWavePlace(unitCount, highLevel);
            SpawnWave(highLevelCount, highLevel, place);
            SpawnWave(lowerLevelCount, lowerLevel, place);
        }

        private void SpawnWave(int count, int level, SpawnPlace place)
        {
            Log($"Spawning wave of {count} units of level {level}");
            _enemyWavesSpawner.SpawnWave(GetWaveConfig(count, level), place);
        }

        private EnemyWaveConfig GetWaveConfig(int count, int level)
        {
            return new EnemyWaveConfig
            {
                Count = count,
                EnemyId = Config.EnemyId,
                EnemyLevel = level
            };
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