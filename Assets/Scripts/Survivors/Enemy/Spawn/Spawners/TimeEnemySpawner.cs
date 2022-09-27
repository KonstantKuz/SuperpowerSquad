using System.Collections;
using System.Collections.Generic;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Session.Messages;
using Zenject;
using WaitForSeconds = Survivors.Scope.WaitConditions.WaitForSeconds;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class TimedEnemySpawner : IEnemySpawner
    {
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;
        [Inject] private EnemyWaveSpawner _enemyWaveSpawner;

        private IEnumerable<EnemyWaveConfig> _waves;
        private ISpawnPlaceProvider _placeProvider;

        private ICoroutine _spawnCoroutine;
        private IUpdatableScope _updatableScope;

        private ICoroutineRunner CoroutineRunner => _updatableScope.CoroutineRunner;

        public void Init(IUpdatableScope updatableScope, IEnumerable<EnemyWaveConfig> waves)
        {
            _updatableScope = updatableScope;
            _waves = waves;
            _placeProvider = new CompositeSpawnPlaceProvider(_enemyWaveSpawner, _world);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        public void StartSpawn()
        {
            Stop();
            _spawnCoroutine = CoroutineRunner.StartCoroutine(SpawnWaves());
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            Stop();
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
