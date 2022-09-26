using SuperMaxim.Messaging;
using Survivors.App;
using Survivors.App.Config;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Session.Messages;

namespace Survivors.Enemy.Spawn
{
    public class EnemySpawnService : IWorldScope
    {
        private readonly UpdatableScope _updatableScope = new UpdatableScope();

        private readonly EnemyWavesSpawner _enemyWavesSpawner;
        private readonly EnemyHpsSpawner _enemyHpsSpawner;
        private readonly ConstantsConfig _constantsConfig;
        private readonly UpdateManager _updateManager;

        public IUpdatableScope UpdatableScope => _updatableScope;

        public bool IsPaused
        {
            get => _updatableScope.IsPaused;
            set => _updatableScope.IsPaused = value;
        }

        private EnemySpawnService(EnemyWavesSpawner enemyWavesSpawner,
                                  EnemyHpsSpawner enemyHpsSpawner,
                                  ConstantsConfig constantsConfig,
                                  UpdateManager updateManager,
                                  IMessenger messenger)
        {
            _enemyWavesSpawner = enemyWavesSpawner;
            _enemyHpsSpawner = enemyHpsSpawner;
            _constantsConfig = constantsConfig;
            _updateManager = updateManager;
            messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
            InitSpawners();
        }

        private void InitSpawners()
        {
            _enemyWavesSpawner.Init(_updatableScope);
            _enemyHpsSpawner.Init(_updatableScope);
        }
        
        public void OnWorldSetup() => _updateManager.StartUpdate(UpdateScope);
        public void OnWorldCleanUp() => _updateManager.StopUpdate(UpdateScope);
        private void UpdateScope() => _updatableScope.Update();

        public void Spawn()
        {
            _updatableScope.Reset();

            _enemyWavesSpawner.StartSpawn();
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