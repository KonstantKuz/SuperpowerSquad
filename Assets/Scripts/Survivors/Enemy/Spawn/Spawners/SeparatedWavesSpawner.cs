using System.Collections;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Session.Messages;
using Survivors.Units;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class SeparatedWavesSpawner : IEnemySpawner
    {
        [Inject] private IMessenger _messenger;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitService _unitService;
        [Inject] private World _world;
        [Inject] private EnemyWaveSpawner _enemyWaveSpawner;

        private IUpdatableScope _updatableScope;
        private TimedEnemySpawner _timedEnemySpawner;
        private ISpawnPlaceProvider _placeProvider;
        private ICoroutine _spawnCoroutine;
        private int _currentWaveId;
        
        private ICoroutineRunner CoroutineRunner => _updatableScope.CoroutineRunner;

        public void Init(IUpdatableScope updatableScope, TimedEnemySpawner timedEnemySpawner)
        {
            _updatableScope = updatableScope;
            _timedEnemySpawner = timedEnemySpawner;
            _placeProvider = new SequentialSpawnPlaceProvider(_enemyWaveSpawner, _world);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }
        
        public void StartSpawn()
        {
            Stop();
            _spawnCoroutine = CoroutineRunner.StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            for (int i = 0; i < _enemyWavesConfig.EnemySpawns.Count; i++)
            {
                var waveId = (i + 1).ToString();
                SpawnWave(waveId);
                yield return new WaitUntil(() => _timedEnemySpawner.IsSpawnFinished);
                yield return new WaitUntil(() => !_unitService.HasUnitOfType(UnitType.ENEMY));
            }
        }

        private void SpawnWave(string waveId)
        {
            _timedEnemySpawner.Init(_updatableScope, _placeProvider, _enemyWavesConfig.GetWave(waveId));
            _timedEnemySpawner.StartSpawn();
        }
        
        private void OnSessionFinished(SessionEndMessage evn)
        {
            Stop();
        }
        
        private void Stop()
        {
            if (_spawnCoroutine != null) {
                CoroutineRunner.StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
    }
}