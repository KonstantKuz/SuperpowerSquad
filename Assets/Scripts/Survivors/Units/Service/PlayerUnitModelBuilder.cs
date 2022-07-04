using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Survivors.Config;
using Survivors.Modifiers.Config;
using Survivors.Player.Inventory.Service;
using Survivors.Units.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Player.Model.Session;
using Zenject;

namespace Survivors.Units.Service
{
    [PublicAPI]
    public class PlayerUnitModelBuilder
    {
        [Inject(Id = Configs.META_UPGRADES)]
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject]
        private readonly StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject]
        private readonly InventoryService _inventoryService;
        [Inject]
        private readonly ModifierFactory _modifierFactory;
        public IUnitModel BuildUnit(string unitId)
        {
            return new PlayerUnitModel(_playerUnitConfigs.Get(unitId), 
                                       new MetaParameterCalculator(_inventoryService.Inventory.UnitsUpgrades, _modifierConfigs, _modifierFactory));
        }
    }
}