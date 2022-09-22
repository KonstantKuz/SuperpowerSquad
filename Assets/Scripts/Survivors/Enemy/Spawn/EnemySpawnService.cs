using EasyButtons;
using SuperMaxim.Messaging;
using Survivors.App;
using Survivors.App.Config;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Session.Messages;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Spawn
{
    public class EnemySpawnService : MonoBehaviour, IWorldScope
    {
        private readonly ScopeUpdatable _scopeUpdatable = new ScopeUpdatable();

        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyHpsSpawner _enemyHpsSpawner;
        [Inject] private ConstantsConfig _constantsConfig;
        [Inject] private UpdateManager _updateManager;
        [Inject] private IMessenger _messenger;
        
        
        public IScopeUpdatable ScopeUpdatable => _scopeUpdatable;
        private void Awake()
        {
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        private void OnSessionFinished(SessionEndMessage obj)
        {
            _scopeUpdatable.Reset();
        }

        public bool Pause
        {
            get => _scopeUpdatable.Pause;
            set => _scopeUpdatable.Pause = value;
        }

        [Button]
        public void PauseToggle() => Pause = !Pause;
        
        public void OnWorldSetup()
        {
            _enemyWavesSpawner.Init(_scopeUpdatable);
            _enemyHpsSpawner.Init(_scopeUpdatable);
            _scopeUpdatable.Reset();
        }
        public void OnWorldCleanUp()
        {
            _scopeUpdatable.Reset();
        }

        private void Update()
        {
            _scopeUpdatable.Update(Time.deltaTime);
        }
        public void Spawn()
        {
            _enemyWavesSpawner.Init(_scopeUpdatable);
            _enemyHpsSpawner.Init(_scopeUpdatable);
            _scopeUpdatable.Reset();
            
            _enemyWavesSpawner.StartSpawn();
            if (_constantsConfig.EnemyHpsSpawnerEnabled) {
                _enemyHpsSpawner.StartSpawn();
            }
        }
    }
}