using SuperMaxim.Messaging;
using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Squad;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Session
{
    public class SessionService : IWorldScope
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private HpsSpawner _hpsSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;     
        [Inject] private SquadFactory _squadFactory; 
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;
        
        public void OnWorldSetup()
        {
            
        }
        public void Start()
        {
            var squad = _squadFactory.CreateSquad();
            _world.Squad = squad;
            squad.OnDeath += OnSquadDeath;
            _unitFactory.CreatePlayerUnit(UnitFactory.SIMPLE_PLAYER_ID);
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
            _hpsSpawner.StartSpawn();
        }
        private void OnSquadDeath()
        {
            EndSession(UnitType.ENEMY);
        }
        private void EndSession(UnitType winner)
        {
            Dispose();
            _messenger.Publish(new SessionEndMessage {
                    Winner = winner,
            });
        }
        private void Dispose()
        {
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