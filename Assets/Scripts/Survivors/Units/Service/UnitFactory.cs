using System;
using Feofun.Config;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Units.Config;
using Survivors.Units.Enemy;
using Survivors.Units.Player;
using Survivors.Units.Player.Config;
using Survivors.Units.Player.Model;
using Survivors.Units.Player.Movement;
using Survivors.Units.Model;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";   
        public const string SIMPLE_PLAYER_ID = "StandardUnit";

        [Inject]
        private World _world;
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private EnemyUnitConfigs _enemyUnitConfigs;
        [Inject]
        private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;

        public PlayerUnit LoadPlayerUnit(string unitId)
        {
            var unitObj = _worldObjectFactory.CreateObject(unitId);
            var unit = unitObj.GetComponentInChildren<PlayerUnit>()
                       ?? throw new NullReferenceException($"Unit is null, objectId:= {unitId}, gameObject:= {unitObj.name}");
            ConfigurePlayerUnit(unit);
            return unit;
        }

        private void ConfigurePlayerUnit(PlayerUnit playerUnit)
        {
            _world.Squad.AddUnit(playerUnit.GetComponent<MovementController>());
            var model = new PlayerUnitModel(_playerUnitConfigs.Get(playerUnit.ObjectId));
            playerUnit.Init(model);
        }
        public EnemyUnit CreateEnemy()
        {
            var enemy =_worldObjectFactory.CreateObject(SIMPLE_ENEMY_ID, _world.SpawnContainer).GetComponent<EnemyUnit>();
            var config = _enemyUnitConfigs.GetConfig(SIMPLE_ENEMY_ID);
            var health = new EnemyHealthModel(config);
            enemy.Init(health);
            return enemy;
        }
    }
}