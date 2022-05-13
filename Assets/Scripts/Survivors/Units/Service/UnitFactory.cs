using System;
using System.Linq;
using Feofun.Config;
using JetBrains.Annotations;
using Survivors.Location.Service;
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
        private LocationObjectFactory _locationObjectFactory;

        [Inject]
        private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        
        public PlayerUnit LoadPlayerUnit()
        {
            var unitId = _playerUnitConfigs.First().Id;
            var unitObj = _locationObjectFactory.CreateObject(unitId);
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