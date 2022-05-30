using Feofun.Config;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Squad.Config;
using Survivors.Squad.Model;
using Survivors.Units;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Player.Model;
using Zenject;

namespace Survivors.Squad
{
    public class SquadFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";
        public const string SIMPLE_PLAYER_ID = "StandardUnit";      
        private const string SQUAD_NAME = "Squad";

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private SquadConfig _squadConfig;
        
        
        public Unit CreatePlayerUnit(string unitId)
        {
            var unitObj = _worldObjectFactory.CreateObject(unitId);
            var unit = unitObj.RequireComponent<Unit>();
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
        public Squad CreateSquad()
        {
            var model = new SquadModel(_squadConfig.Params, _playerUnitConfigs.Get(SIMPLE_PLAYER_ID).Health);
            var squad = _worldObjectFactory.CreateObject(SQUAD_NAME, _world.transform).RequireComponent<Squad>();
            squad.transform.SetPositionAndRotation(_world.Spawn.transform.position, _world.Spawn.transform.rotation);
            squad.Init(model);
            return squad;
        }
        
        private void ConfigurePlayerUnit(Unit unit)
        {
            var config = _playerUnitConfigs.Get(unit.ObjectId);
            var model = new PlayerUnitModel(config);
            unit.Init(model);
        }

      
    }
}