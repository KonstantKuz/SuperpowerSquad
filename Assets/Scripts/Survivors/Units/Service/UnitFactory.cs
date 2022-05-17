using Feofun.Config;
using System;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Units.Enemy;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Player.Model;
using Survivors.Units.Player.Movement;
using Zenject;

namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";
        public const string SIMPLE_PLAYER_ID = "StandardUnit";

        [Inject]
        private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject]
        private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject]
        private World _world;
        [Inject]
        private WorldObjectFactory _worldObjectFactory;

        public Unit LoadPlayerUnit(string unitId)
        {
            var unitObj = _worldObjectFactory.CreateObject(unitId);
            var unit = unitObj.GetComponentInChildren<Unit>()
                       ?? throw new NullReferenceException($"Unit is null, objectId:= {unitId}, gameObject:= {unitObj.name}");
            ConfigurePlayerUnit(unit);
            return unit;
        }

        private void ConfigurePlayerUnit(Unit unit)
        {
            _world.Squad.AddUnit(unit.GetComponent<MovementController>());
            var config = _playerUnitConfigs.Get(unit.ObjectId);
            var model = new PlayerUnitModel(config);
            unit.Init(model);
        }

        public EnemyUnit CreateEnemy()
        {
            var enemy = _worldObjectFactory.CreateObject(SIMPLE_ENEMY_ID, _world.SpawnContainer).GetComponent<EnemyUnit>();
            var config = _enemyUnitConfigs.Get(SIMPLE_ENEMY_ID);
            var health = new EnemyHealthModel(config);
            enemy.Init(health);
            return enemy;
        }
    }
}