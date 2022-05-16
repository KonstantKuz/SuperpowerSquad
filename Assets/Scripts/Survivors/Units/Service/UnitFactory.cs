using System;
using System.Linq;
using Feofun.Config;
using JetBrains.Annotations;
using Survivors.GameWorld.Service;
using Survivors.Units.Player;
using Survivors.Units.Player.Config;
using Survivors.Units.Player.Model;
using Zenject;

namespace Survivors.Units.Service
{
    [PublicAPI]
    public class UnitFactory
    {
        [Inject]
        private WorldObjectFactory _worldObjectFactory;

        [Inject]
        private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        
        public PlayerUnit LoadPlayerUnit()
        {
            var unitId = _playerUnitConfigs.First().Id;
            var unitObj = _worldObjectFactory.CreateObject(unitId);
            var unit = unitObj.GetComponentInChildren<PlayerUnit>()
                       ?? throw new NullReferenceException($"Unit is null, objectId:= {unitId}, gameObject:= {unitObj.name}");
            Configure(unit);
            return unit;
        }
        private void Configure(PlayerUnit playerUnit)
        {
            var model = new PlayerUnitModel(_playerUnitConfigs.Get(playerUnit.ObjectId));
            playerUnit.Init(model);
        }
    }
}