using System.Collections;
using System.Collections.Generic;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Session.Messages;
using Zenject;
using WaitForSeconds = Survivors.Scope.WaitConditions.WaitForSeconds;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class TimedEnemySpawner : IEnemySpawner
    {
        [Inject] private IMessenger _messenger;
        [Inject] private EnemyWaveSpawner _enemyWaveSpawner;

        private IEnumerable<EnemyWaveConfig> _waves;
        private ISpawnPlaceProvider _placeProvider;

        private ICoroutine _spawnCoroutine;
        private IUpdatableScope _updatableScope;

        private ICoroutineRunner CoroutineRunner => _updatableScope.CoroutineRunner;
        public bool IsSpawnFinished { get; private set; }

        public void Init(IUpdatableScope updatableScope, ISpawnPlaceProvider placeProvider, IEnumerable<EnemyWaveConfig> waves)
        {
            _updatableScope = updatableScope;
            _waves = waves;
            _placeProvider = placeProvider;
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        public void StartSpawn()
        {
            Stop();
            IsSpawnFinished = false;
            _spawnCoroutine = CoroutineRunner.StartCoroutine(SpawnWaves());
        }

        private IEnumerator SpawnWaves()
        {
            var currentTime = 0;
            foreach (var wave in _waves)
            {
                yield return new WaitForSeconds(_updatableScope.ScopeTime, wave.SpawnTime - currentTime);
                currentTime = wave.SpawnTime; 
                _enemyWaveSpawner.SpawnWave(wave, _placeProvider);
            } 
            IsSpawnFinished = true;
            Stop();
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
