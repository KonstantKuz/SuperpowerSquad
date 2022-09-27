using System.Collections.Generic;
using System.Linq;
using Feofun.Config;
using SuperMaxim.Messaging;
using Survivors.App;
using Survivors.App.Config;
using Survivors.Enemy.Spawn.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Session.Messages;
using Survivors.Units.Enemy.Config;

namespace Survivors.Enemy.Spawn
{
    public class EnemySpawnService : IWorldScope, IEnemySpawner
    {
        private readonly UpdatableScope _updatableScope = new UpdatableScope();

        private readonly TimedEnemySpawner _timedEnemySpawner;
        private readonly EnemyHpsSpawner _enemyHpsSpawner;     
        private readonly BossSpawner _bossSpawner;
        
        private readonly StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        private readonly EnemyWavesConfig _enemyWavesConfig;   
        
        private readonly ConstantsConfig _constantsConfig;
        private readonly UpdateManager _updateManager;

        public IUpdatableScope UpdatableScope => _updatableScope;


        private EnemySpawnService(TimedEnemySpawner timedEnemySpawner,
                                  
                                  EnemyHpsSpawner enemyHpsSpawner,
                                  ConstantsConfig constantsConfig,
                                  UpdateManager updateManager,
                                  IMessenger messenger,
                                  BossSpawner bossSpawner,
                                  StringKeyedConfigCollection<EnemyUnitConfig> enemyUnitConfigs,
                                  EnemyWavesConfig enemyWavesConfig)
        { 
            _timedEnemySpawner = timedEnemySpawner;
            _enemyHpsSpawner = enemyHpsSpawner;
            _constantsConfig = constantsConfig;
            _updateManager = updateManager;
            _bossSpawner = bossSpawner;
            _enemyUnitConfigs = enemyUnitConfigs;
            _enemyWavesConfig = enemyWavesConfig;
            messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
            InitSpawners();
        }

        private void InitSpawners()
        {
            _timedEnemySpawner.Init(_updatableScope, GetEnemyWavesConfig(false));
            _bossSpawner.Init(_updatableScope, GetEnemyWavesConfig(true));
            _enemyHpsSpawner.Init(_updatableScope);
    
        }
        public IEnumerable<EnemyWaveConfig> GetEnemyWavesConfig(bool isBoss)
        { 
            return _enemyWavesConfig.EnemySpawns
                                    .OrderBy(it => it.SpawnTime)
                                    .Where(it => _enemyUnitConfigs.Get(it.EnemyId).IsBoss == isBoss);
        }
        public void OnWorldSetup() => _updateManager.StartUpdate(UpdateScope);
        public void OnWorldCleanUp() => _updateManager.StopUpdate(UpdateScope);
        private void UpdateScope() => _updatableScope.Update();

        public void StartSpawn()
        {
            _updatableScope.Reset();

            _bossSpawner.StartSpawn();
            _timedEnemySpawner.StartSpawn();
            
            if (_constantsConfig.EnemyHpsSpawnerEnabled) {
                _enemyHpsSpawner.StartSpawn();
            }
        }
        private void OnSessionFinished(SessionEndMessage obj)
        {
            _updatableScope.Reset();
            _updateManager.StopUpdate(UpdateScope);
        }
        
    }
}