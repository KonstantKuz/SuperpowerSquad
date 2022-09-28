using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SuperMaxim.Messaging;
using Survivors.Enemy.Messages;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.PlaceProviders;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Scope.WaitConditions;
using Survivors.Session.Messages;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class BossSpawner : IEnemySpawner
    {
        private const float ALERT_SHOWING_DURATION = 4f;
        
        [Inject] private UnitService _unitService;      
        [Inject] private EnemyWaveSpawner _enemySpawner;      
        [Inject] private IMessenger _messenger;
        [Inject] private World _world;
        
        private IEnumerable<EnemyWaveConfig> _bossSpawns;
        private ISpawnPlaceProvider _placeProvider;
        
        private IUpdatableScope _updatableScope;
        private ICoroutine _spawnCoroutine;

        private ICoroutineRunner CoroutineRunner => _updatableScope.CoroutineRunner;
        
        public void Init(IUpdatableScope updatableScope, IEnumerable<EnemyWaveConfig> bossSpawns)
        {
            _updatableScope = updatableScope;
            _bossSpawns = bossSpawns;
            _placeProvider = new SideDrivenPlaceProvider(_enemySpawner, _world, SpawnSide.Top);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }
        public void StartSpawn()
        {
            Stop();
            _spawnCoroutine = CoroutineRunner.StartCoroutine(SpawnBosses());
            
        }
        private IEnumerator SpawnBosses()
        {
            var currentTime = 0;
            foreach (var bossSpawn in _bossSpawns)
            {
                yield return new WaitForSeconds(_updatableScope.ScopeTime, bossSpawn.SpawnTime - currentTime - ALERT_SHOWING_DURATION);
                _messenger.Publish(new BossAlertShowingMessage(ALERT_SHOWING_DURATION));
                yield return new WaitForSeconds(_updatableScope.ScopeTime, ALERT_SHOWING_DURATION);
                DeleteAllEnemy();
                SpawnBoss(bossSpawn);
                currentTime = bossSpawn.SpawnTime;
            }
            Stop();
        }
        
        private void DeleteAllEnemy()
        {
            _updatableScope.IsPaused = true;
            _unitService.GetAllUnits(UnitType.ENEMY).ToList().ForEach(it => {
                it.Kill(DeathCause.Removed);
            });
            
        }
        private void SpawnBoss(EnemyWaveConfig bossSpawn)
        {
            _enemySpawner.SpawnWave(bossSpawn, _placeProvider);
        }
        private void OnSessionFinished(SessionEndMessage evn) => Stop();
        private void Stop()
        {
            if (_spawnCoroutine != null) {
                CoroutineRunner.StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
          
        }
    }
}