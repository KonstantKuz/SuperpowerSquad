using SuperMaxim.Messaging;
using Survivors.App;
using Survivors.App.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Session.Messages;

namespace Survivors.Enemy.Spawn.Service
{
    public class EnemySpawnService : IWorldScope, IEnemySpawner
    {
        private readonly UpdatableScope _updatableScope = new UpdatableScope();

        private readonly SeparatedWavesSpawner _separatedWavesSpawner;
        private readonly TimedEnemySpawner _timedEnemySpawner;
        private readonly EnemyHpsSpawner _enemyHpsSpawner;     
        private readonly BossSpawner _bossSpawner;
        private readonly EnemyWaves _enemyWaves;
        
        
        private readonly ConstantsConfig _constantsConfig;
        private readonly UpdateManager _updateManager;

        public IUpdatableScope UpdatableScope => _updatableScope;


        private EnemySpawnService(SeparatedWavesSpawner separatedWavesSpawner,
                                  TimedEnemySpawner timedEnemySpawner,
                                  EnemyHpsSpawner enemyHpsSpawner,
                                  ConstantsConfig constantsConfig,
                                  UpdateManager updateManager,
                                  IMessenger messenger,
                                  BossSpawner bossSpawner,
                                  EnemyWaves enemyWaves)
        {
            _separatedWavesSpawner = separatedWavesSpawner;
            _timedEnemySpawner = timedEnemySpawner;
            _enemyHpsSpawner = enemyHpsSpawner;
            _constantsConfig = constantsConfig;
            _updateManager = updateManager;
            _bossSpawner = bossSpawner;
            _enemyWaves = enemyWaves;
            messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
            InitSpawners();
        }

        private void InitSpawners()
        {
            _separatedWavesSpawner.Init(_updatableScope, _timedEnemySpawner);
            _bossSpawner.Init(_updatableScope, _enemyWaves.GetWavesConfigs(true));
            _enemyHpsSpawner.Init(_updatableScope);
        }

        public void OnWorldSetup() => _updateManager.StartUpdate(UpdateScope);
        public void OnWorldCleanUp() => _updateManager.StopUpdate(UpdateScope);
        private void UpdateScope() => _updatableScope.Update();

        public void StartSpawn()
        {
            _updatableScope.Reset();

            _bossSpawner.StartSpawn();
            _separatedWavesSpawner.StartSpawn();
            
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