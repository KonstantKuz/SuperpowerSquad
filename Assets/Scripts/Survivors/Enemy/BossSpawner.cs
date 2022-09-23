using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using ModestTree;
using SuperMaxim.Core.Extensions;
using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Config;
using Survivors.Scope;
using Survivors.Scope.Coroutine;
using Survivors.Scope.WaitConditions;
using Survivors.Units;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Enemy
{
    public class BossSpawner
    {
        [Inject]
        private EnemyWavesConfig _enemyWavesConfig;
        [Inject]
        private ConfigCollection<string, EnemyUnitConfig> _enemyUnitConfig;
        [Inject]
        private EnemySpawnService _enemySpawnService;  
        [Inject]
        private UnitService _unitService;      
        [Inject]
        private EnemyWavesSpawner _enemyWavesSpawner;
        
        private ICoroutine _timeCoroutine;

        private IScopeUpdatable ScopeUpdatable => _enemySpawnService.ScopeUpdatable;

        private ICoroutineRunner CoroutineRunner => ScopeUpdatable.CoroutineRunner;

        public BossSpawner(EnemyWavesConfig enemyWavesConfig, ConfigCollection<string, EnemyUnitConfig> enemyUnitConfig)
        {
            _enemyWavesConfig = enemyWavesConfig;
            _enemyUnitConfig = enemyUnitConfig;
            var bossSpawns = enemyWavesConfig.EnemySpawns.OrderBy(it => it.SpawnTime)
                                             .Where(it => enemyUnitConfig.Get(it.EnemyId).IsBoss)
                                             .ToList();
            if (bossSpawns.IsEmpty()) {
                return;
            }
            _timeCoroutine = CoroutineRunner.StartCoroutine(StartShowBossTimeout(bossSpawns));
        }
        
        private IEnumerator StartShowBossTimeout(IEnumerable<EnemyWaveConfig> bossSpawns)
        {
            var currentTime = 0;
            foreach (var bossSpawn in bossSpawns)
            {
                yield return new WaitForSeconds(ScopeUpdatable.Timer, bossSpawn.SpawnTime - currentTime - 5f);
                ShowAllert();
                yield return new WaitForSeconds(ScopeUpdatable.Timer, 5f);
                
                DeleteAllEnemy();
                SpawnBoss(bossSpawn);
                currentTime = bossSpawn.SpawnTime; 

            }
            Stop();
        }

        private void ShowAllert()
        {
            
        }   
        private void DeleteAllEnemy()
        {
            _enemySpawnService.Pause = true;
            _unitService.GetAllUnits(UnitType.ENEMY).ForEach(it => {
                it.Kill(DeathCause.Removed);
            });
            
        }
        
        private void SpawnBoss(EnemyWaveConfig bossSpawn)
        {
            _enemyWavesSpawner.SpawnWave(bossSpawn, _enemyWavesSpawner.GetPlaceForWave(bossSpawn));
        }
        

        private void Stop()
        {
            if (_timeCoroutine != null) {
                CoroutineRunner.StopCoroutine(_timeCoroutine);
                _timeCoroutine = null;
            }
          
        }
    }
}