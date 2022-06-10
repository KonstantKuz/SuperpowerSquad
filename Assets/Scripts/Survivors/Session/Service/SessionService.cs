using System.Linq;
using Feofun.Config;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using Survivors.Player.Model;
using Survivors.Player.Service;
using Survivors.Session.Config;
using Survivors.Session.Messages;
using Survivors.Squad;
using Survivors.Units;
using Survivors.Units.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Session.Service
{
    public class SessionService : IWorldScope
    {
        private readonly IntReactiveProperty _kills = new IntReactiveProperty(0);
        
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyHpsSpawner _enemyHpsSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;     
        [Inject] private SquadFactory _squadFactory; 
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;       
        [Inject] private UnitService _unitService;
        [Inject] private SessionRepository _repository;
        [Inject] private readonly StringKeyedConfigCollection<LevelMissionConfig> _levelsConfig;
        [Inject] private PlayerProgressService _playerProgressService;
        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        
        private Model.Session Session => _repository.Require();
        
        public IReadOnlyReactiveProperty<int> Kills => _kills;
        
        public void OnWorldSetup()
        {
            Dispose();
            _unitService.OnEnemyUnitDeath += OnEnemyUnitDeath;
            ResetKills();
        }
        
        public void Start()
        {
            CreateSession();
            CreateSquad();
            SpawnUnits();
        }
        public LevelMissionConfig GetLevelConfig() => _levelsConfig.Values[Mathf.Min(PlayerProgress.WinCount, _levelsConfig.Count() - 1)];
        private void CreateSession()
        {
            var newSession = Model.Session.Build(GetLevelConfig());
            _repository.Set(newSession);
            Debug.Log($"Kill enemies:= {GetLevelConfig().KillCount}");
        }
    
        private void CreateSquad()
        {
            var squad = _squadFactory.CreateSquad();
            _world.Squad = squad;
            squad.OnDeath += OnSquadDeath;
        }
        private void SpawnUnits()
        {
            _unitFactory.CreatePlayerUnit(UnitFactory.SIMPLE_PLAYER_ID);
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
            _enemyHpsSpawner.StartSpawn();
        }

        private void ResetKills() => _kills.Value = 0;
        private void OnEnemyUnitDeath(IUnit unit)
        {
            Session.AddKill();
            _kills.Value = Session.Kills;
            Debug.Log($"Killed enemies:= {Session.Kills}");
            if (Session.IsMaxKills) {
                EndSession(UnitType.PLAYER);
            }
        }

        private void OnSquadDeath()
        {
            EndSession(UnitType.ENEMY);
        }
        private void EndSession(UnitType winner)
        {
            Dispose();
            Session.SetResultByUnitType(winner);
            
            _unitService.DeactivateAll();
            _world.Squad.IsActive = false;

            _messenger.Publish(new SessionEndMessage(Session.Result.Value));

        }
        private void Dispose()
        {
            _unitService.OnEnemyUnitDeath -= OnEnemyUnitDeath;
            if (_world.Squad != null) {
                _world.Squad.OnDeath -= OnSquadDeath;
            }
        }
        public void OnWorldCleanUp()
        {
            Dispose();
        }
        
    }
}