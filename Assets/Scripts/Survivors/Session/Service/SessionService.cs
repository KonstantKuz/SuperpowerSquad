using System.Linq;
using Feofun.Config;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using Survivors.Player.Progress.Model;
using Survivors.Player.Progress.Service;
using Survivors.Session.Config;
using Survivors.Session.Messages;
using Survivors.Session.Model;
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
        [Inject] private Analytics.Analytics _analytics;
        
        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        public Model.Session Session => _repository.Require();
        
        public IReadOnlyReactiveProperty<int> Kills => _kills;
        public LevelMissionConfig LevelConfig => _levelsConfig.Values[LevelId];
        public int LevelId => Mathf.Min(PlayerProgress.LevelNumber, _levelsConfig.Count() - 1);
        public float SessionTime => Session.SessionTime;
        public bool SessionCompleted => _repository.Exists() && Session.Completed;
        
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

        private void CreateSession()
        {
            var levelConfig = LevelConfig;
            var newSession = Model.Session.Build(levelConfig);
            _repository.Set(newSession);
            _playerProgressService.OnSessionStarted(levelConfig.Level);
            Debug.Log($"Kill enemies:= {levelConfig.KillCount}");
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
        private void OnEnemyUnitDeath(IUnit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
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

            _analytics.ReportLevelFinished(Session.Result == SessionResult.Win);
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