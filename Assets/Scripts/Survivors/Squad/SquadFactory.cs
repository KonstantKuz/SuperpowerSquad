using Feofun.Config;
using Feofun.Modifiers;
using Survivors.App.Config;
using Survivors.Config;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Modifiers.Config;
using Survivors.Player.Inventory.Service;
using Survivors.Squad.Config;
using Survivors.Squad.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Squad
{
    public class SquadFactory
    {
        private const string SQUAD_NAME = "Squad";

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private SquadConfig _squadConfig;
        [Inject] private ConstantsConfig _constantsConfig;

        [Inject(Id = Configs.META_UPGRADES)]
        private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject] private InventoryService _inventoryService;
        [Inject] private ModifierFactory _modifierFactory;

        public Squad CreateSquad()
        {
            var squad = _worldObjectFactory.CreateObject<Squad>(SQUAD_NAME, _world.transform);
            squad.transform.SetPositionAndRotation(_world.Spawn.transform.position, _world.Spawn.transform.rotation);
            squad.Init(BuildSquadModel());
            return squad;
        }

        private SquadModel BuildSquadModel()
        {
            var startingHealth = _playerUnitConfigs.Get(_constantsConfig.FirstUnit).Health;
            return new SquadModel(_squadConfig, startingHealth, CreateParameterCalculator());
        }

        private MetaParameterCalculator CreateParameterCalculator() =>
                new MetaParameterCalculator(_inventoryService.Inventory.UnitsUpgrades, _modifierConfigs, _modifierFactory);
    }
}