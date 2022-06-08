using SuperMaxim.Messaging;
using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Squad;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;
using UniRx;

namespace Survivors.Session
{
    public class SessionService : IWorldScope
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyHpsSpawner _enemyHpsSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;     
        [Inject] private SquadFactory _squadFactory; 
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;       
        [Inject] private UnitService _unitService;
        [Inject] private CompositeDisposable _disposable;

        private IntReactiveProperty _kills = new IntReactiveProperty(0);
        public IReadOnlyReactiveProperty<int> Kills => _kills;
        
        private Model.Session Session { get; set; }

        public void OnWorldSetup()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _unitService.OnEnemyUnitDeath += OnEnemyUnitDeath;
        }
        
        private void OnEnemyUnitDeath(IUnit unit)
        {
            Session.AddKill();
            if (Session.IsMaxKills) {
                EndSession(UnitType.PLAYER);
            }
        }

        public void Start()
        {
            Session = Model.Session.Build();
            
            var squad = _squadFactory.CreateSquad();
            _world.Squad = squad;
            squad.OnDeath += OnSquadDeath;
            _unitFactory.CreatePlayerUnit(UnitFactory.SIMPLE_PLAYER_ID);
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
            _enemyHpsSpawner.StartSpawn();
        }
        private void OnSquadDeath()
        {
            EndSession(UnitType.ENEMY);
        }
        private void EndSession(UnitType winner)
        {
            Dispose();
            Session.SetWinnerByUnitType(winner);
            _messenger.Publish(new SessionEndMessage {
                    Winner = winner,
            });
        }
        private void Dispose()
        {
            _unitService.OnEnemyUnitDeath -= OnEnemyUnitDeath;
            _disposable?.Dispose();
            _disposable = null;
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