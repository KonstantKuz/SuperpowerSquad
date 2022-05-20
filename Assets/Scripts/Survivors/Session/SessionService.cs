using SuperMaxim.Messaging;
using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Session
{
    public class SessionService : IWorldCleanUp
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private UnitService _unitService;
        [Inject] private IMessenger _messenger;

        public void Start()
        {
            _unitFactory.CreatePlayerUnit(UnitFactory.SIMPLE_PLAYER_ID);
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
            _unitService.OnPlayerUnitDeath += OnPlayerUnitDeath;
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