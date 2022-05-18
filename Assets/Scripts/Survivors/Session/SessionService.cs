using SuperMaxim.Messaging;
using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.Units;
using Survivors.Units.Player.Movement;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Session
{
    public class SessionService
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;
        [Inject] private UnitService _unitService;
        [Inject] private IMessenger _messenger;
        [Inject] private World _world;

        public void Start()
        {
            _unitFactory.LoadPlayerUnit(UnitFactory.SIMPLE_PLAYER_ID);
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
            _unitService.OnPlayerUnitDeath += OnPlayerUnitDeath;
        }

        private void OnPlayerUnitDeath(IUnit unit)
        {
            _world.Squad.RemoveUnit(unit.Object.GetComponent<MovementController>());
            if (_unitService.ExistUnitType(UnitType.PLAYER)) {
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

        public void Term()
        {
            _unitService.OnPlayerUnitDeath -= OnPlayerUnitDeath;
        }
    }
}