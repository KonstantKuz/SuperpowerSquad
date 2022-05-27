using SuperMaxim.Messaging;
using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Squad.Config;
using Survivors.Squad.Model;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Session
{
    public class SessionService : IWorldScope
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private UnitService _unitService;        
        [Inject] private World _world;      
        [Inject] private SquadConfig _squadConfig;
        [Inject] private IMessenger _messenger;
        public void OnWorldSetup()
        {
            _unitService.OnPlayerUnitDeath += OnPlayerUnitDeath;
        }
        public void Start()
        {
            InitSquad();
            _unitFactory.CreatePlayerUnit(UnitFactory.SIMPLE_PLAYER_ID);
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
          
        }
        private void InitSquad()
        {
            var model = new SquadModel(_squadConfig.Params);
            _world.Squad.Init(model);
        }
        private void OnPlayerUnitDeath(IUnit unit)
        {
            if (_unitService.HasUnitOfType(UnitType.PLAYER)) {
                return;
            }
            EndSession(UnitType.ENEMY);
        }

        private void EndSession(UnitType winner)
        {
            _unitService.OnPlayerUnitDeath -= OnPlayerUnitDeath;
            _messenger.Publish(new SessionEndMessage {
                    Winner = winner,
            });
        }
        
        public void OnWorldCleanUp()
        {
            _unitService.OnPlayerUnitDeath -= OnPlayerUnitDeath;
        }
    }
}