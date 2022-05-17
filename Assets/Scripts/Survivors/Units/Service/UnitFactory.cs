﻿using System;
using Feofun.Config;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Units.Enemy;
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
        private World _world;
        [Inject]
        private WorldObjectFactory _worldObjectFactory;

        [Inject]
        private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;

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
        public EnemyAi CreateEnemy()
        {
            return _worldObjectFactory.CreateObject(SIMPLE_ENEMY_ID, _world.SpawnContainer).GetComponent<EnemyAi>();
        }
    }
}