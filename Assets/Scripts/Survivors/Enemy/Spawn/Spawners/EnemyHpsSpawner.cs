﻿using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using Feofun.Config;
using Feofun.Extension;
using Logger.Extension;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Session.Messages;
using Survivors.Units.Enemy.Config;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using WaitForSeconds = Survivors.Scope.WaitConditions.WaitForSeconds;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class EnemyHpsSpawner : IEnemySpawner
    {
        [Inject] private EnemyWaveSpawner _enemySpawner;
        [Inject] private HpsSpawnerConfig _config;
        [Inject] private IMessenger _messenger;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private StringKeyedConfigCollection<SpawnableEnemyConfig> _spawnableEnemyConfigs;
        [Inject] private EnemyWaveSpawner _enemyWaveSpawner;
        [Inject] private World _world;
        
        private ISpawnPlaceProvider _placeProvider;

        private IUpdatableScope _updatableScope;
        private ICoroutine _spawnCoroutine;
        
        private ICoroutineRunner CoroutineRunner => _updatableScope.CoroutineRunner;
        
        public void Init(IUpdatableScope updatableScope)
        {
            _updatableScope = updatableScope;
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }
        public void StartSpawn()
        {
            Stop();
            InitPlaceProvider();
            _spawnCoroutine = CoroutineRunner.StartCoroutine(SpawnCoroutine());
        }
        private void InitPlaceProvider()
        {
            _placeProvider = new CompositeSpawnPlaceProvider(_enemyWaveSpawner, _world);
        }
        private void OnSessionFinished(SessionEndMessage evn)
        {
            Stop();
        }

        private void Stop()
        {
            if (_spawnCoroutine == null) return;

            CoroutineRunner.StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        private IEnumerator SpawnCoroutine()
        {
            var time = 0.0f;
            while (true) {
                var timeToNextWave = Random.Range(_config.MinInterval, _config.MaxInterval);
                yield return new WaitForSeconds(_updatableScope.ScopeTime, timeToNextWave);
                time += timeToNextWave;
                var health = timeToNextWave * (_config.StartingHPS + _config.HPSSpeed * time);
                SpawnWave(health);
            }
        }

        private void SpawnWave(float health)
        {
            Log($"Spawning wave of health {health}");
            var spawnConfig = GetRandomEnemyConfig();
            var desiredUnitCount = Random.Range(spawnConfig.MinWaveSize, spawnConfig.MaxWaveSize + 1);
            var averageHealth = health / desiredUnitCount;
            var enemyUnitConfig = _enemyUnitConfigs.Get(spawnConfig.Id);
            var averageLevel = EnemyUnitConfig.MIN_LEVEL + (averageHealth - enemyUnitConfig.Health) / enemyUnitConfig.HealthStep;

            if (averageLevel < EnemyUnitConfig.MIN_LEVEL) {
                var level = EnemyUnitConfig.MIN_LEVEL;
                var possibleUnitCount = Mathf.RoundToInt(health / enemyUnitConfig.Health);
                var waveConfig = EnemyWaveConfig.Create(enemyUnitConfig.Id, possibleUnitCount, level);
                var place = GetWavePlace(waveConfig);
                SpawnWave(waveConfig, place);
            } else {
                SpawnMixedWave(enemyUnitConfig.Id, desiredUnitCount, averageLevel);
            }
        }

        private SpawnableEnemyConfig GetRandomEnemyConfig()
        {
            var possibleEnemies = _spawnableEnemyConfigs.Where(it => it.Delay <= _updatableScope.ScopeTime.Time).ToList();
            var configsWithChance = possibleEnemies.Select(it => Tuple.Create(it, it.Chance)).ToList();
            return configsWithChance.SelectRandomWithChance();
        }

        private SpawnPlace GetWavePlace(EnemyWaveConfig waveConfig)
        {
            return _enemySpawner.FindEmptyPlace(waveConfig, _placeProvider);
        }

        private void SpawnMixedWave(string enemyId, int count, float averageLevel)
        {
            var partition = averageLevel % 1;
            var highLevelCount = (int) partition * count;
            var lowerLevelCount = Mathf.RoundToInt(count - highLevelCount);
            var lowerLevel = (int) Math.Floor(averageLevel);
            var highLevel = lowerLevel + 1;

            var configForPlace = EnemyWaveConfig.Create(enemyId, count, highLevel);
            var place = GetWavePlace(configForPlace);

            var lowerLevelConfig = EnemyWaveConfig.Create(enemyId, lowerLevelCount, lowerLevel);
            var highLevelConfig = EnemyWaveConfig.Create(enemyId, highLevelCount, highLevel);
            SpawnWave(lowerLevelConfig, place);
            SpawnWave(highLevelConfig, place);
        }

        private void SpawnWave(EnemyWaveConfig waveConfig, SpawnPlace place)
        {
            Log($"Spawning wave of {waveConfig.Count} units of level {waveConfig.EnemyLevel}");
            _enemySpawner.SpawnWave(waveConfig, place);
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