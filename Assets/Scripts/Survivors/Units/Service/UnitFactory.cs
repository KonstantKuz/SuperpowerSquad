using System;
using Feofun.Config;
using Survivors.Location;
using Survivors.Location.Service;
 using Survivors.Units.Enemy.Config;
 using Survivors.Units.Enemy.Model;
 using Zenject;
 using Survivors.Units.Player.Config;
 using Survivors.Units.Player.Model;


 namespace Survivors.Units.Service
{
    public class UnitFactory
    {
        private const string SIMPLE_ENEMY_ID = "SimpleEnemy";
        public const string SIMPLE_PLAYER_ID = "SniperUnit";

        [Inject]
        private World _world;
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject]
        private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;

        public void CreatePlayerUnit(string unitId)
        {
            var unitObj = _worldObjectFactory.CreateObject(unitId);
            var unit = unitObj.GetComponentInChildren<Unit>()
                       ?? throw new NullReferenceException($"Unit is null, objectId:= {unitId}, gameObject:= {unitObj.name}");
            ConfigurePlayerUnit(unit);
        }

        private void ConfigurePlayerUnit(Unit unit)
        {
            _world.Squad.AddUnit(unit);
            var config = _playerUnitConfigs.Get(unit.ObjectId);
            var model = new PlayerUnitModel(config);
            unit.Init(model);
        }

        public Unit CreateEnemy()
        {
            var enemy = _worldObjectFactory.CreateObject(SIMPLE_ENEMY_ID).GetComponent<Unit>();
            var config = _enemyUnitConfigs.Get(SIMPLE_ENEMY_ID);
            var model = new EnemyUnitModel(config);
            enemy.Init(model);
            return enemy;
        }
    }
}