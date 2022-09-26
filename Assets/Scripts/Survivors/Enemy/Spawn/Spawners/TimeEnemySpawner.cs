using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Session.Messages;
using Survivors.Units.Enemy.Config;
using Zenject;
using WaitForSeconds = Survivors.Scope.WaitConditions.WaitForSeconds;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class TimeEnemySpawner : IEnemySpawner
    {
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;     
        
        [Inject] private EnemyWaveSpawner _enemyWaveSpawner;
 
        private IScopeUpdatable _scopeUpdatable;
        private ISpawnPlaceProvider _placeProvider;
        private ICoroutine _spawnCoroutine;
        
        private ICoroutineRunner CoroutineRunner => _scopeUpdatable.CoroutineRunner;
        
        public void Init(IScopeUpdatable scopeUpdatable)
        {
            _scopeUpdatable = scopeUpdatable;
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        public void StartSpawn()
        {
            Stop();
            InitPlaceProvider();
            var orderedConfigs = _enemyWavesConfig.EnemySpawns
                                                  .OrderBy(it => it.SpawnTime)
                                                  .Where(it => !_enemyUnitConfigs.Get(it.EnemyId).IsBoss);
            _spawnCoroutine = CoroutineRunner.StartCoroutine(SpawnWaves(orderedConfigs));
        }

        private void InitPlaceProvider()
        {
            _placeProvider = new CompositeSpawnPlaceProvider(_enemyWaveSpawner, _world);
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            Stop();
        }
        private IEnumerator SpawnWaves(IEnumerable<EnemyWaveConfig> waves)
        {
            var currentTime = 0;
            foreach (var wave in waves)
            {
                yield return new WaitForSeconds(wave.SpawnTime - currentTime);
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
