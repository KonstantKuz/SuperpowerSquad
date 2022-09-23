using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using ModestTree;
using SuperMaxim.Messaging;
using Survivors.Enemy.Messages;
using Survivors.Enemy.Spawn.Config;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Scope.WaitConditions;
using Survivors.Session.Messages;
using Survivors.Units;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Enemy.Spawn.Spawners
{
    public class BossSpawner : IEnemySpawner
    {
        private const float ALERT_SHOWING_DURATION = 4f;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private UnitService _unitService;      
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;      
        [Inject] private IMessenger _messenger;
        
        private IScopeUpdatable _scopeUpdatable;
        private ICoroutine _spawnCoroutine;

        private IScopeUpdatable ScopeUpdatable => _scopeUpdatable;

        private ICoroutineRunner CoroutineRunner => ScopeUpdatable.CoroutineRunner;
        
        public void Init(IScopeUpdatable scopeUpdatable)
        {
            _scopeUpdatable = scopeUpdatable;
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }
        public void StartSpawn()
        {
            Stop();
            var bossSpawns = _enemyWavesConfig.EnemySpawns.OrderBy(it => it.SpawnTime)
                                              .Where(it => _enemyUnitConfigs.Get(it.EnemyId).IsBoss)
                                              .ToList();
            if (bossSpawns.IsEmpty()) {
                return;
            }
            _spawnCoroutine = CoroutineRunner.StartCoroutine(SpawnBosses(bossSpawns));
            
        }
        private IEnumerator SpawnBosses(IEnumerable<EnemyWaveConfig> bossSpawns)
        {
            var currentTime = 0;
            foreach (var bossSpawn in bossSpawns)
            {
                yield return new WaitForSeconds(bossSpawn.SpawnTime - currentTime - ALERT_SHOWING_DURATION);
                _messenger.Publish(new BossAlertShowingMessage(ALERT_SHOWING_DURATION));
                yield return new WaitForSeconds(ALERT_SHOWING_DURATION);
                DeleteAllEnemy();
                SpawnBoss(bossSpawn);
                currentTime = bossSpawn.SpawnTime;
            }
            Stop();
        }
        
        private void DeleteAllEnemy()
        {
            ScopeUpdatable.IsPaused = true;
            _unitService.GetAllUnits(UnitType.ENEMY).ToList().ForEach(it => {
                it.Kill(DeathCause.Removed);
            });
            
        }
        private void SpawnBoss(EnemyWaveConfig bossSpawn)
        {
            _enemyWavesSpawner.SpawnWave(bossSpawn, _enemyWavesSpawner.GetPlaceForWave(bossSpawn));
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