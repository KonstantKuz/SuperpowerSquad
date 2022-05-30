using Feofun.Config;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Player.Model;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        public const string SIMPLE_PLAYER_ID = "StandardUnit";
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";
        
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        
        public Unit CreatePlayerUnit(string unitId)
        {
            var unit = _worldObjectFactory.CreateObject(unitId).RequireComponent<Unit>();
            ConfigurePlayerUnit(unit);
            _world.Squad.AddUnit(unit);
            return unit;
        }
        public Unit CreateEnemy()
        {
            var enemy = _worldObjectFactory.CreateObject(SIMPLE_ENEMY_ID).RequireComponent<Unit>();
            var config = _enemyUnitConfigs.Get(SIMPLE_ENEMY_ID);
            var model = new EnemyUnitModel(config);
            enemy.Init(model);
            return enemy;
        }
        private void ConfigurePlayerUnit(Unit unit)
        {
            var config = _playerUnitConfigs.Get(unit.ObjectId);
            var model = new PlayerUnitModel(config);
            unit.Init(model);
        }
    }
}