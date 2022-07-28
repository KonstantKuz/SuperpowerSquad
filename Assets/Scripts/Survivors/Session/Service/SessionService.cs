using System.Linq;
using Feofun.Config;
using Feofun.Extension;
using Feofun.UI.Dialog;
using Logger.Extension;
using SuperMaxim.Messaging;
using Survivors.App.Config;
using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Config;
using Survivors.Location;
using Survivors.Player.Progress.Model;
using Survivors.Player.Progress.Service;
using Survivors.Session.Config;
using Survivors.Session.Messages;
using Survivors.Session.Model;
using Survivors.Squad;
using Survivors.UI.Dialog.ReviveDialog;
using Survivors.Units;
using Survivors.Units.Service;
using UniRx;
using UnityEngine;
using Zenject;
using UnityEngine.Assertions;

namespace Survivors.Session.Service
{
    public class SessionService : IWorldScope
    {
        private readonly IntReactiveProperty _kills = new IntReactiveProperty(0);
        
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private LevelWavesSpawner _levelWavesSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;     
        [Inject] private SquadFactory _squadFactory; 
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;       
        [Inject] private UnitService _unitService;
        [Inject] private SessionRepository _repository;
        [Inject] private WavesByLevelConfig _wavesByLevelConfig;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private ConstantsConfig _constantsConfig;
        [Inject] private DialogManager _dialogManager;
        
        private CompositeDisposable _disposable;
        
        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        public Model.Session Session => _repository.Require();
        
        public IReadOnlyReactiveProperty<int> Kills => _kills;
        public LevelWavesConfig LevelConfig => _wavesByLevelConfig.LevelConfigs[LevelId];
        public int LevelId => Mathf.Min(PlayerProgress.LevelNumber, _wavesByLevelConfig.LevelsCount - 1);
        public float SessionTime => Session.SessionTime;
        public bool SessionCompleted => _repository.Exists() && Session.Completed;
        
        public void OnWorldSetup()
        {
            Dispose();
            _unitService.OnEnemyUnitDeath += OnEnemyUnitDeath;
            ResetKills();
            _disposable = new CompositeDisposable();
        }
        
        public void Start()
        {
            CreateSession();
            CreateSquad();
            SpawnUnits();
        }
        public void ChangeStartUnit(string unitId)
        {
            CheckSquad();
            _world.Squad.RemoveUnits();
            _unitFactory.CreatePlayerUnits(unitId, _world.Squad.Model.StartingUnitCount.Value);
        }
        private void CreateSession()
        {
            var levelConfig = LevelConfig;
            var newSession = Model.Session.Build(levelConfig);
            _repository.Set(newSession);
            _playerProgressService.OnSessionStarted(levelConfig.Level);
            this.Logger().Debug($"Kill enemies:= {levelConfig.Waves.Sum(it => it.Count)}");
        }
    
        private void CreateSquad()
        {
            var squad = _squadFactory.CreateSquad();
            _world.Squad = squad;
            squad.OnZeroHealth += OnSquadZeroHealth;
            squad.OnDeath += OnSquadDeath;
            squad.Model.StartingUnitCount.Diff().Subscribe(CreatePlayerUnits).AddTo(_disposable);
        }

        private void CreatePlayerUnits(int count)
        {
            Assert.IsTrue(count >= 0, "Should add non-negative count of units");
            _unitFactory.CreatePlayerUnits(_constantsConfig.FirstUnit, count);
        }
        
        private void CheckSquad() => Assert.IsNotNull(_world.Squad, "Squad is null, should call this method only inside game session");
        private void SpawnUnits()
        {
            CheckSquad();
            CreatePlayerUnits(_world.Squad.Model.StartingUnitCount.Value);
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig); 
            _levelWavesSpawner.StartSpawn(LevelConfig);
        }

        private void ResetKills() => _kills.Value = 0;
        private void OnEnemyUnitDeath(IUnit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
            Session.AddKill();
            _playerProgressService.AddKill();
            _kills.Value = Session.Kills;
            this.Logger().Trace($"Killed enemies:= {Session.Kills}");
            if (Session.IsMaxKills) {
                EndSession(UnitType.PLAYER);
                return;
            }
            if (!_unitService.HasUnitOfType(UnitType.ENEMY))
            {
                _messenger.Publish(new WaveClearedMessage());
            }
        }

        private void OnSquadZeroHealth()
        {
            _dialogManager.Show<ReviveDialog>();
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
            _disposable?.Dispose();
            _disposable = null;
            _unitService.OnEnemyUnitDeath -= OnEnemyUnitDeath;
            var squad = _world.Squad;
            if (squad != null) {
                squad.OnZeroHealth -= OnSquadZeroHealth;
                squad.OnDeath -= OnSquadDeath;
            }
        }
        public void OnWorldCleanUp()
        {
            Dispose();
        }

        public void AddRevive()
        {
            Session.AddRevive();
        }

    
    }
}