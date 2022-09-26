﻿using SuperMaxim.Messaging;
using Survivors.App;
using Survivors.App.Config;
using Survivors.Enemy.Spawn.Spawners;
using Survivors.Location;
using Survivors.Scope;
using Survivors.Session.Messages;
using UnityEngine;

namespace Survivors.Enemy.Spawn
{
    public class EnemySpawnService : IWorldScope
    {
        private readonly ScopeUpdatable _scopeUpdatable = new ScopeUpdatable();

        private readonly TimeEnemySpawner _timeEnemySpawner;
        private readonly EnemyHpsSpawner _enemyHpsSpawner;     
        private readonly BossSpawner _bossSpawner;
        
        private readonly ConstantsConfig _constantsConfig;
        private readonly UpdateManager _updateManager;

        public IScopeUpdatable ScopeUpdatable => _scopeUpdatable;

        private EnemySpawnService(TimeEnemySpawner timeEnemySpawner,
                                  EnemyHpsSpawner enemyHpsSpawner,
                                  ConstantsConfig constantsConfig,
                                  UpdateManager updateManager,
                                  IMessenger messenger,
                                  BossSpawner bossSpawner)
        { 
            _timeEnemySpawner = timeEnemySpawner;
            _enemyHpsSpawner = enemyHpsSpawner;
            _constantsConfig = constantsConfig;
            _updateManager = updateManager;
            _bossSpawner = bossSpawner;
            messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
            InitSpawners();
        }

        private void InitSpawners()
        {
            _timeEnemySpawner.Init(_scopeUpdatable);
            _enemyHpsSpawner.Init(_scopeUpdatable);
            _bossSpawner.Init(_scopeUpdatable);
        }
        
        public void OnWorldSetup() => _updateManager.StartUpdate(UpdateScope);
        public void OnWorldCleanUp() => _updateManager.StopUpdate(UpdateScope);
        private void UpdateScope() => _scopeUpdatable.Update(Time.deltaTime);

        public void Spawn()
        {
            _scopeUpdatable.Reset();

            _bossSpawner.StartSpawn();
            _timeEnemySpawner.StartSpawn();
            
            if (_constantsConfig.EnemyHpsSpawnerEnabled) {
                _enemyHpsSpawner.StartSpawn();
            }
        }
        
        private void OnSessionFinished(SessionEndMessage obj)
        {
            _scopeUpdatable.Reset();
            _updateManager.StopUpdate(UpdateScope);
        }
    }
}